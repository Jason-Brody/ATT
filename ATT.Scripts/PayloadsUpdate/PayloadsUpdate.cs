using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ScriptRunner.Interface;
using ScriptRunner.Interface.Attributes;
using System.IO;
using System.Xml;
using System.Text.RegularExpressions;
using System.Data.SqlClient;
using System.IO.Compression;
using ATT.Data;
using ATT.Data.ATT;

namespace ATT.Scripts
{
    [Script("Payloads Transform")]
    public class PayloadsUpdate : ScriptBase<PayloadsUpdateData>
    {
       

        private Dictionary<string, EDIKeyTemp> dic = null;

        private DateTime _downloadDt;

        private XmlDocument _xDoc;

        private ATTLog _log;

        public PayloadsUpdate() {
            _xDoc = new XmlDocument();
        }

        public override void Initial(PayloadsUpdateData data, IProgress<ProgressInfo> StepReporter) {
            base.Initial(data, StepReporter);
            _log = new ATTLog(_data);
        }

       

        [Step(Id = 1, Name = "Unzip the file")]
        public void UnzipFile() {
           
            string file = _data.TaskFolder + ".zip";
            FileInfo f = new FileInfo(file);
            _downloadDt = f.CreationTimeUtc;
            _log.WriteLog(_data.UnzipLog, LogType.Normal);
            ZipFile.ExtractToDirectory(file, _data.SourceFolder);
            _log.WriteLog(_data.UnzipLog, LogType.Success);
            File.Delete(file);
        }

        [Step(Id = 2, Name = "Get EDIKey Info from file")]
        public void GetEDIKeyInfo() {
            dic = new Dictionary<string, EDIKeyTemp>();
            _log.WriteLog(_data.AnalysisLog, LogType.Normal);
            foreach (var f in Directory.GetFiles(_data.SourceFolder)) {
                string keyId = FetchFileName(f);
                dic.Add(keyId, getDocInfo(f));
            }
            _log.WriteLog(_data.AnalysisLog, LogType.Success);
        }

        [Step(Id = 3, Name = "Update EDIKey Info to DB")]
        public void UpdateEDIKeyInfo() {

            List<IDocTypes> iDocTypes = null;
           
            List<ProAwsys> proAwsys = null;
            _log.WriteLog(_data.UploadLog, LogType.Normal);
            using (var db = new ATTDbContext()) {
              
                iDocTypes = db.IDocTypes.Where(i => i.Name != null).ToList();
                proAwsys = db.ProAwsys.Where(p => p.Name != null).ToList();

                var msgIds = db.MsgIDs.Where(m => m.TaskId == _data.TaskId).ToList();
                foreach (var i in msgIds) {
                    if (dic.ContainsKey(i.MsgId)) {

                        var iDocType = iDocTypes.Where(s => s.Name.Trim().ToLower() == dic[i.MsgId].IDocType.Trim().ToLower()).FirstOrDefault();

                        if (iDocType == null) {
                            iDocType = new IDocTypes() { Name = dic[i.MsgId].IDocType };
                            db.IDocTypes.Add(iDocType);
                            i.IDocTypes = iDocType;
                        } else {
                            i.IDocTypeId = iDocType.Id;
                        }

                        //i.IDocTypeId = iDocTypes.Where(s => s.Name.Trim().ToLower() == dic[i.MsgId].IDocType.Trim().ToLower()).FirstOrDefault()?.Id;
                        i.ProAwsysId = proAwsys.Where(s => s.Name.Trim().ToLower() == dic[i.MsgId].Awsys?.Trim().ToLower()).FirstOrDefault()?.Id;
                        i.IsDownload = true;
                        i.DownloadDt = _downloadDt;
                    }
                }

                db.SaveChanges();
                _log.WriteLog(_data.UploadLog, LogType.Success);
            }
            #region Hide
            //using (var db = new ATT.Data.AttDbContext()) {
            //    using (var transaction = db.Database.BeginTransaction()) {
            //        foreach (var item in dic) {
            //            var ediKey = db.Database.SqlQuery<MsgIDs>("exec SP_UpdateSource @id,@Awsys,@IDocType,@DownloadDt",
            //                new SqlParameter("id", item.Value.Id),
            //                new SqlParameter("Awsys", item.Value.Awsys),
            //                new SqlParameter("IDocType", item.Value.IDocType),
            //                new SqlParameter("DownloadDt", _downloadDtUtc)).SingleOrDefault();

            //            if (ediKey != null)
            //                ediKeys.Add(ediKey);
            //        }
            //        try {
            //            transaction.Commit();
            //        }
            //        catch {
            //            transaction.Rollback();
            //        }
            //    }
            //}
            #endregion
        }


        [Step(Id = 4, Name = "Copy and transform xml")]
        public void CopyAndTransform() {
            _log.WriteLog(_data.GetXPathConfigLog, LogType.Normal);
            using (var db = new ATTDbContext()) {

                var para = new SqlParameter("TaskId", _data.TaskId);

                var xPathConfigs = db.Database.SqlQuery<ATT.Data.VW_EDITransFormConfig>("exec SP_GetXPathConfig @TaskId", para).ToList();
                _log.WriteLog(_data.GetXPathConfigLog, LogType.Success);

                _log.WriteLog(_data.UpdateTransformConfigLog, LogType.Normal);
                para = new SqlParameter("TaskId", _data.TaskId);
                db.Database.ExecuteSqlCommand("exec SP_UpdateTransform @TaskId", para);
                _log.WriteLog(_data.UpdateTransformConfigLog, LogType.Success);

                _log.WriteLog(_data.TransformLog, LogType.Normal);
                var msgIds = db.MsgIDs.Where(m => m.TaskId == _data.TaskId && m.MsgId != null).ToList();

                if (msgIds.Count > 0) {
                    foreach (var k in msgIds) {
                        var configs = xPathConfigs.Where(x => x.IDocTypeId == k.IDocTypeId && x.ProAwsysId == k.ProAwsysId);
                        transformFile(configs, k);
                    }
                    db.SaveChanges();
                }
                _log.WriteLog(_data.TransformLog,LogType.Success);

            }

        }

        private void transformFile(IEnumerable<Data.VW_EDITransFormConfig> Configs, MsgIDs k) {

            var sourceFile = Path.Combine(_data.SourceFolder, $"{k.MsgId}.xml");
            var targetFile = Path.Combine(_data.TargetFolder, $"{k.MsgId}.xml");

            if (k.IsNeedTransform != null) {
                if(k.IsNeedTransform.Value == true) {
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
    }

    public class EDIKeyTemp
    {
        public int Id { get; set; }

        public string Awsys { get; set; }

        public string IDocType { get; set; }
    }
}
