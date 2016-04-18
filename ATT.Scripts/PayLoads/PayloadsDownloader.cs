using ScriptRunner.Interface;
using ScriptRunner.Interface.Attributes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ATT.Scripts.DataFetch
{
    [Script("Payloads Download Script")]
    public class PlayloadsDownloader:IScriptRunner<PayloadsDownloaderModel>
    {
        private PayloadsDownloaderModel _data;

        public void SetInputData(PayloadsDownloaderModel data, IProgress<ProgressInfo> MyProgress)
        {
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

        [Step(Id =1, Name ="Download XML Files")]

        public void Download()
        {

            HttpWebRequest request = HttpWebRequest.Create(_data.DownloadUrl) as HttpWebRequest;
            request.Credentials = new NetworkCredential(_data.Username, _data.Password);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            string postData = "msgids=";
            foreach(var msgId in _data.MessageIds)
            {
                postData += msgId + Environment.NewLine;
            }

            postData += "&lastVersion=false&fullEnvelope=false";
            var data = Encoding.ASCII.GetBytes(postData);

            using (var stream = request.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }

            HttpWebResponse response = request.GetResponse() as HttpWebResponse;

            using (var responseStream = response.GetResponseStream())
            {
                using (var fs = File.OpenWrite(_data.ZipFileName))
                {
                    responseStream.CopyTo(fs);
                }
            }
        }

        
    }
}
