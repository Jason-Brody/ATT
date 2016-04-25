using ATT.Data;
using ATT.Data.Entity;
using ScriptRunner.Interface;
using ScriptRunner.Interface.Attributes;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;

namespace ATT.Scripts
{
    public class PayloadsTransform : ScriptBase<PayloadsTransformData>
    {
        private string _sourceFolder;
        private string _targetFolder;

        private Dictionary<string, EDIKeyTemp> dic = null;

        private DateTime _downloadDt;

        private XmlDocument _xDoc;

        public PayloadsTransform() {
            _xDoc = new XmlDocument();
        }


        [Step(Id = 1, Name = "Download XML Files")]
        public void Download() {
            List<MsgIDs> edikeys = null;

            using (var db = new ATT.Data.AttDbContext()) {
                edikeys = db.MsgIds.Where(m => m.TaskId == _data.TaskId).ToList();
            }

            if (edikeys.Count > 0) {
                WebClient client = new WebClient();
                client.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
                client.Credentials = new NetworkCredential(_data.UserName, _data.Password);
                string postData = "msgids=";
                foreach (var edikey in edikeys) {
                    postData += edikey.MsgId + Environment.NewLine;
                }
                postData += "&lastVersion=false&fullEnvelope=false";
                byte[] result = downloadFile(client, postData);
                var f = Path.Combine(_data.WorkFolder, $"{edikeys.Last().TaskId.Value.ToString()}.zip");
                if (File.Exists(f)) {
                    File.Delete(f);
                }
                MemoryStream ms = new MemoryStream(result);
                using (var fs = File.OpenWrite(f)) {
                    ms.CopyTo(fs);
                }
            }
        }

        [Step(Id = 2, Name = "Unzip the file")]
        public void UnzipFile() {
            string folder = Path.Combine(_data.WorkFolder, _data.TaskId.ToString());
            string file = folder + ".zip";
            _sourceFolder = Path.Combine(folder, "Source");
            _targetFolder = Path.Combine(folder, "Target");

            GlobalConfig.CreateDirectory(_sourceFolder);
            GlobalConfig.CreateDirectory(_targetFolder);

            FileInfo f = new FileInfo(file);
            _downloadDt = f.CreationTimeUtc;

            ZipFile.ExtractToDirectory(file, _sourceFolder);
            File.Delete(file);
        }

        [Step(Id = 3, Name = "Get EDIKey Info from file")]
        public void GetEDIKeyInfo() {
            dic = new Dictionary<string, EDIKeyTemp>();
            foreach (var f in Directory.GetFiles(_sourceFolder)) {
                string keyId = FetchFileName(f);
                dic.Add(keyId, getDocInfo(f));
            }
        }

        [Step(Id = 4, Name = "Update EDIKey Info to DB")]
        public void UpdateEDIKeyInfo() {

            List<IDocType> iDocTypes = null;
            List<ProAwsy> proAwsys = null;

            using (var db = new AttDbContext()) {
                iDocTypes = db.IDocTypes.Where(i => i.Name != null).ToList();
                proAwsys = db.ProAwsys.Where(p => p.Name != null).ToList();

                var msgIds = db.MsgIds.Where(m => m.TaskId == _data.TaskId).ToList();
                foreach (var i in msgIds) {
                    if (dic.ContainsKey(i.MsgId)) {

                        var iDocType = iDocTypes.Where(s => s.Name.Trim().ToLower() == dic[i.MsgId].IDocType.Trim().ToLower()).FirstOrDefault();

                        if (iDocType == null) {
                            iDocType = new IDocType() { Name = dic[i.MsgId].IDocType };
                            db.IDocTypes.Add(iDocType);
                            i.IDocType = iDocType;
                        } else {
                            i.IDocTypeId = iDocType.Id;
                        }

                        i.ProAwsysId = proAwsys.Where(s => s.Name.Trim().ToLower() == dic[i.MsgId].Awsys?.Trim().ToLower()).FirstOrDefault()?.Id;
                        i.IsDownload = true;
                        i.DownloadDt = _downloadDt;
                    }
                }

                db.SaveChanges();

            }
        }


        [Step(Id = 5, Name = "Copy and transform xml")]
        public void CopyAndTransform() {
            using (var db = new AttDbContext()) {

                var para = new SqlParameter("TaskId", _data.TaskId);

                var xPathConfigs = db.Database.SqlQuery<ATT.Data.VW_EDITransFormConfig>("exec SP_GetXPathConfig @TaskId", para).ToList();

                para = new SqlParameter("TaskId", _data.TaskId);
                db.Database.ExecuteSqlCommand("exec SP_UpdateTransform @TaskId", para);

                var msgIds = db.MsgIds.Where(m => m.TaskId == _data.TaskId && m.MsgId != null).ToList();

                if (msgIds.Count > 0) {
                    foreach (var k in msgIds) {
                        var configs = xPathConfigs.Where(x => x.IDocTypeId == k.IDocTypeId && x.ProAwsysId == k.ProAwsysId);
                        transformFile(configs, k);
                    }
                    db.SaveChanges();
                }

            }

        }

        private void transformFile(IEnumerable<Data.VW_EDITransFormConfig> Configs, MsgIDs k) {

            var sourceFile = Path.Combine(_sourceFolder, $"{k.MsgId}.xml");
            var targetFile = Path.Combine(_targetFolder, $"{k.MsgId}.xml");

            if (k.IsNeedTransform != null) {
                if (k.IsNeedTransform.Value == true) {
                    if (Configs != null && Configs.Count() > 0) {
                        File.Copy(sourceFile, targetFile);
                        _xDoc.Load(targetFile);
                        foreach (var item in Configs) {
                            var nodes = _xDoc.SelectNodes(item.XPath);
                            foreach (XmlNode n in nodes) {
                                if (string.IsNullOrEmpty(item.FromVal)) {
                                    n.InnerText = item.ToVal;
                                } else if (n.InnerText == item.FromVal) {
                                    n.InnerText = item.ToVal;
                                }
                            }
                        }
                        _xDoc.Save(targetFile);
                        k.IsTransformed = true;
                        k.TransformDt = DateTime.UtcNow;
                    }
                } else {
                    File.Copy(sourceFile, targetFile);
                }
            }
        }



        private EDIKeyTemp getDocInfo(string fileName) {
            _xDoc.Load(fileName);
            var root = _xDoc.DocumentElement;

            string awsys = null;
            XmlNode n = root.SelectSingleNode(_data.XPath_AWSYS);
            if (n == null) {
                n = root.SelectSingleNode(_data.XPath_OBJ_SYS);
            }
            if (n == null) {
                n = root.SelectSingleNode(_data.XPath_OTC_rec_type);
                //if (n != null && n.InnerText != "")
                //    awsys = "USMISCINV";
            }

            awsys = n?.InnerText;



            string idocType = root.Name;
            return new EDIKeyTemp() {
                Awsys = awsys,
                IDocType = idocType
            };
        }

        public static string FetchFileName(string fileName) {
            string regex = ".*\\\\([^\\.]+)\\..*";
            Match match = Regex.Match(fileName, regex);
            return match.Groups[1].ToString();
        }

        private byte[] downloadFile(WebClient client, string postData) {
            try {
                byte[] result = client.UploadData(_data.DownloadUrl, "POST", System.Text.Encoding.UTF8.GetBytes(postData));
                return result;
            }
            catch {
                Task.Delay(2000).Wait();
                return downloadFile(client, postData);
            }
        }
    }
}
