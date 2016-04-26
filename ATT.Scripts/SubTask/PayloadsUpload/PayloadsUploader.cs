using ATT.Data;
using ScriptRunner.Interface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Data.Entity;
using ATT.Data.Entity;
using ScriptRunner.Interface.Attributes;

namespace ATT.Scripts
{
    public class PayloadsUploader:ScriptBase<PayloadsUploaderData>
    {
        

        [Step(Id =1,Name ="Upload payload to XI")]
        public void Upload() {
            using(var db = new AttDbContext()) {
                var msgIds = db.MsgIds.Include(m=>m.ProAwsy).Where(m => m.TaskId == _data.TaskId 
                && m.IsNeedTransform == true 
                && m.IsSend == false
                && m.ProAwsysId != null).ToList();
                var iDocTypeIds = msgIds.GroupBy(g => g.IDocTypeId).Select(g=>g.Key).ToList();
                var SourceIds = msgIds.GroupBy(g => g.ProAwsy.SourceId).Select(g=>g.Key).ToList();
                var senderConfigs = db.SenderConfigs.Where(c => iDocTypeIds.Contains(c.IDocTypeId) && SourceIds.Contains(c.SourceId)).ToList();
                ProgressInfo info = new ProgressInfo(1, msgIds.Count, "");
                foreach(var item in msgIds) {
                    send(item,senderConfigs);
                    item.IsSend = true;
                    item.SentDt = DateTime.UtcNow;
                    info.Msg = item.MsgId;
                    _stepReporter.Report(info);
                    info.Current++;
                }
                db.SaveChanges();
            }
        }

        private void send(MsgID item,List<SenderConfig> configs) {
            var senderConfig = configs.Where(c => c.IDocTypeId == item.IDocTypeId && c.SourceId == item.ProAwsy.SourceId).First();
            var url = buildUrl(senderConfig);
            var request = createWebRequest(url);
            var postXml = getPayload(item.MsgId);
            using(var stream = request.GetRequestStream()) {
                postXml.Save(stream);
            }
            HttpWebResponse resp = request.GetResponse() as HttpWebResponse;
        }

        private void loopSend(HttpWebRequest request,MsgID item,int retryCount=10) {
            try {
                HttpWebResponse resp = request.GetResponse() as HttpWebResponse;
            }
            catch (Exception ex) {
                if (retryCount == 0)
                    throw ex;
                loopSend(request, item, retryCount--);
            }
           
        }

        private XmlDocument getPayload(string msgId) {
            FileInfo f = new FileInfo(Path.Combine(_data.TargetFolder, msgId + ".xml"));
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(f.FullName);
            if(xDoc.FirstChild.NodeType == XmlNodeType.XmlDeclaration) {
                xDoc.RemoveChild(xDoc.FirstChild);
            }
            string before = "<soapenv:Envelope xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\">\n<soapenv:Header/>\n<soapenv:Body>\n";
            string after = "</soapenv:Body>\n" + "</soapenv:Envelope>\n";
            var xml = before + xDoc.OuterXml + after;
            xDoc.LoadXml(xml);
            return xDoc;
        }

        private HttpWebRequest createWebRequest(string url) {
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
            webRequest.Credentials = new NetworkCredential(_data.UserName, _data.Password);
            webRequest.Proxy = new WebProxy(_data.ProxyHost, _data.ProxyHostPort);
            webRequest.Headers.Add("SOAPAction", "http://sap.com/xi/WebService/soap1.1");
            webRequest.ContentType = "text/xml;charset=\"utf-8\"";
            webRequest.Accept = "text/xml";
            webRequest.Method = "POST";
            return webRequest;
        }

        private string buildUrl(SenderConfig config) {
            string urlTemp = WebUtility.UrlEncode(config.sendernamespace + "^" + config.senderinterface);
            string url = $"http://{_data.Host}:{_data.Port}/sap/xi/engine?type=entry&version=3.0&Sender.Service={config.itgsendercomponent}&Interface={urlTemp}&QualityOfService=ExactlyOnce";
            return url;
        }
    }
}
