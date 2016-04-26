using ATT.Data.Entity;
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
        [Step(Id = 1, Name = "Download XML Files")]
        public void Download() {
            Console.WriteLine(_data.TaskId);
            List<MsgID> edikeys = null;

            using (var db = new ATT.Data.AttDbContext()) {
                edikeys = db.MsgIds.Where(m => m.TaskId == _data.TaskId).ToList();
            }

            if (edikeys.Count > 0) {

                WebClient client = new WebClient();
                client.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
                client.Credentials = new NetworkCredential(_data.UserName, _data.Password);
                
                //HttpWebRequest request = HttpWebRequest.Create(_data.DownloadUrl) as HttpWebRequest;
                //request.Credentials = new NetworkCredential(_data.Username, _data.Password);
                //request.Method = "POST";
                //request.ContentType = "application/x-www-form-urlencoded";
                string postData = "msgids=";

                foreach (var edikey in edikeys) {
                    postData += edikey.MsgId + Environment.NewLine;
                }

                postData += "&lastVersion=false&fullEnvelope=false";

                byte[] result = downloadFile(client, postData);
                var f = Path.Combine(_data.WorkFolder, $"{_data.TaskId.ToString()}.zip");
                if (File.Exists(f)) {
                    File.Delete(f);
                }
                
                MemoryStream ms = new MemoryStream(result);
                using (var fs = File.OpenWrite(f)) {
                    ms.CopyTo(fs);
                }

                #region hide
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
                #endregion
            }


        }

        private byte[] downloadFile(WebClient client, string postData) {
            try {
                byte[] result = client.UploadData(_data.DownloadUrl, "POST", System.Text.Encoding.UTF8.GetBytes(postData));
                return result;
            }
            catch  {
                Task.Delay(2000).Wait();
                return downloadFile(client, postData);
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
