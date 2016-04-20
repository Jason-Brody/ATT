using ScriptRunner.Interface;
using ScriptRunner.Interface.Attributes;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ATT.Scripts.PayLoads
{
    [Script("Payloads Download Script")]
    public class PayloadsDownloader : ScriptBase<PayloadsDownloaderModel>
    {
        private PayloadsDownloaderModel _data;
        private PayloadsOutput _output;

        public PayloadsDownloader() {
            _output = new PayloadsOutput();
        }

        public override void SetInputData(PayloadsDownloaderModel data) {
            this._data = data;
        }


        //private void getCookie()
        //{
        //    CookieCollection cookies = new CookieCollection();
        //    HttpWebRequest request = HttpWebRequest.Create(_data.CookieUrl) as HttpWebRequest;
        //    request.Credentials = new NetworkCredential(_data.Username, _data.Password);
        //    HttpWebResponse response = request.GetResponse() as HttpWebResponse;

        //    for (int i = 0; i < response.Headers.Count; i++)
        //    {
        //        string name = response.Headers.GetKey(i);
        //        if (name != "Set-Cookie")
        //            continue;
        //        string value = response.Headers.Get(i);
        //        foreach (var cookie in value.Split(','))
        //        {
        //            Match match = Regex.Match(cookie, "(.+?)=(.+?);");
        //            if (match.Captures.Count == 0)
        //                continue;

        //            cookies.Add(new Cookie(match.Groups[1].ToString()
        //                , match.Groups[2].ToString()
        //                , "/"
        //                , request.Host.Split(':')[0]));

        //        }
        //    }
        //}

        [Step(Id = 1, Name = "Download XML Files")]
        public void Download() {
            List<ATT.Data.EDIKeys> edikeys = null;

            using (var db = new ATT.Data.AttDbContext()) {
                edikeys = db.Database.SqlQuery<ATT.Data.EDIKeys>("exec SP_GetEDIKeys @num", new SqlParameter("num", _data.DownloadPatchSize)).ToList();
            }

            if (edikeys.Count > 0) {

                WebClient client = new WebClient();
                client.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
                client.Credentials = new NetworkCredential(_data.Username, _data.Password);
               
                

                //HttpWebRequest request = HttpWebRequest.Create(_data.DownloadUrl) as HttpWebRequest;
                //request.Credentials = new NetworkCredential(_data.Username, _data.Password);
                //request.Method = "POST";
                //request.ContentType = "application/x-www-form-urlencoded";
                string postData = "msgids=";

                foreach (var edikey in edikeys) {
                    postData += edikey.EDIKey + Environment.NewLine;
                }


              

                postData += "&lastVersion=false&fullEnvelope=false";

               

                client.UploadProgressChanged += (s, e) => {
                    Console.WriteLine(e.ProgressPercentage);
                };

                client.UploadDataCompleted += (s, e) => {
                    Console.WriteLine("Complete");
                };

                
                byte[] result = client.UploadData(_data.DownloadUrl, "POST", System.Text.Encoding.UTF8.GetBytes(postData));
                var f = Path.Combine(_output.WorkDir, $"{edikeys.Last().Id.ToString()}.zip");
                if (File.Exists(f)) {
                    File.Delete(f);
                }

                MemoryStream ms = new MemoryStream(result);
                using (var fs = File.OpenWrite(f)) {
                    ms.CopyTo(fs);
                }

                //var data = Encoding.ASCII.GetBytes(postData);

                //using (var stream = request.GetRequestStream()) {
                //    stream.Write(data, 0, data.Length);
                //}

                ///// Need to add codes if file can't be downloaded
                //HttpWebResponse response = request.GetResponse() as HttpWebResponse;

                //using (var responseStream = response.GetResponseStream()) {
                //    var f = Path.Combine(_output.WorkDir,  $"{edikeys.Last().Id.ToString()}.zip");
                //    if (File.Exists(f)) {
                //        File.Delete(f);
                //    }
                //    using (var fs = File.OpenWrite(f)) {
                //        responseStream.CopyTo(fs);
                //    }

                //}
            }
        }

        
    }
}
