using ATT.Data;
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

namespace ATT.Scripts
{
    [Script("Payloads Download Script")]
    public class PayloadsDownloader : ScriptBase<PayloadsDownloaderData>
    {
       

        public override void Initial(PayloadsDownloaderData data, IProgress<ProgressInfo> StepReporter) {
            base.Initial(data, StepReporter);
        }

       

        [Step(Id = 1, Name = "Download XML Files")]
        public void Download() {
           
            List<MsgIDs> edikeys = null;

            var log = _data.GetLog(_data.GetTaskIdLog, LogType.Normal);
            ATTPayLoadsLog.Write(log);
            

            using (var db = new ATTDbContext()) {
                edikeys = db.MsgIDs.Where(m => m.TaskId == _data.TaskId).ToList();
                
            }

            log = _data.GetLog(_data.GetTaskIdLog, LogType.Success);
            ATTPayLoadsLog.Write(log);

            if (edikeys.Count > 0) {

                log = _data.GetLog(_data.DownloadFileLog, LogType.Normal);
                ATTPayLoadsLog.Write(log);
                
                string postData = "msgids=";

                foreach (var edikey in edikeys) {
                    postData += edikey.MsgId + Environment.NewLine;
                }

                postData += "&lastVersion=false&fullEnvelope=false";

                downloadFile(postData);

                ATTPayLoadsLog.Write(_data.GetLog(_data.DownloadFileLog, LogType.Success));
                

            }


        }

        private void downloadFile(string postData,int RetryCount = 50) {
            try {
                HttpWebRequest request = HttpWebRequest.Create(_data.DownloadUrl) as HttpWebRequest;
                request.Credentials = new NetworkCredential(_data.UserName, _data.Password);
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";

                var data = Encoding.ASCII.GetBytes(postData);

                using (var stream = request.GetRequestStream()) {
                    stream.Write(data, 0, data.Length);
                }

                /// Need to add codes if file can't be downloaded
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;

                using (var responseStream = response.GetResponseStream()) {
                    var f = Path.Combine(_data.WorkFolder, $"{_data.TaskId.ToString()}.zip");
                    if (File.Exists(f)) {
                        File.Delete(f);
                    }
                    using (var fs = File.OpenWrite(f)) {
                        responseStream.CopyTo(fs);
                    }

                }
            }
            catch (Exception ex) {
                if (RetryCount == 0)
                    throw ex;
                else {
                    RetryCount--;
                    ATTPayLoadsLog.Write(_data.GetLog($"Fail to Download ,Retry left {RetryCount} times", LogType.Fail));
                   
                }
                Task.Delay(3000).Wait();
                downloadFile(postData, RetryCount);
            }
            


        }

        private byte[] downloadFile(WebClient client, string postData,int RetryCount=10) {
            try {
                byte[] result = client.UploadData(_data.DownloadUrl, "POST", System.Text.Encoding.UTF8.GetBytes(postData));
                return result;
            }
            catch (Exception ex) {
                if(RetryCount == 0) {
                    ATTPayLoadsLog.Write(_data.GetLog(ex.Message, LogType.Error));
                    
                    throw ex;
                } else {
                    RetryCount--;
                    ATTPayLoadsLog.Write(_data.GetLog($"Fail to Download , retry {10 - RetryCount}", LogType.Fail));
                    
                }
                Task.Delay(1000).Wait();
                return downloadFile(client, postData,RetryCount);
            }
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
    }
}
