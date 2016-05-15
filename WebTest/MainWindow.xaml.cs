using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Net;
using System.IO;
using System.IO.IsolatedStorage;
using System.Text.RegularExpressions;
using System.Collections.Specialized;
using System.Xml;

namespace WebTest
{

    public class Student
    {
        public string Name { get; set; }
    }
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string test = "abc";
        //Student stu = new Student() { Name = "1" };
        //Student stu1 = new Student() { Name = "2" };
        //List<Student> stus = new List<Student>();
        public MainWindow() {
            InitializeComponent();
            //stus.Add(stu);
            //stus.Add(stu1);
            //dg1.DataContext = stus;
            //dg2.DataContext = stus;

            Student stu = new Student();
            stu.Name = "Test1";
            tb1.DataContext = stu;
            tb2.DataContext = stu;
        }

        //public void Send() {
        //    string userName = "21746957";
        //    string pwd = "Ojo@6gat";
        //    string component = "com_hp_U26_010_uat";
        //    string senderInterface = "FIDCC1.FIDCCP01.ZEXFIDCCP01";
        //    string senderNamespace = "urn:sap-com:document:sap:idoc:messages";
        //    string host = "pi-itg-01-idoc.sapnet.hpecorp.net";
        //    string port = "63100";
        //    string msgId = "001F296E7D2E1EE682D0AD3AEC5F1204";
        //    string url = buildUrl(host, port, senderNamespace, component, senderInterface);

        //    HttpWebRequest req = CreateWebRequest(url, "http://sap.com/xi/WebService/soap1.1");
        //    req.Credentials = new NetworkCredential(userName, pwd);
        //    req.Proxy = new WebProxy("web-proxy.austin.hp.com", 8080);
        //    var postXml = getPayload(msgId);
           

        //    using (var stream = req.GetRequestStream()) {

        //        postXml.Save(stream);
        //    }

        //    HttpWebResponse resp = req.GetResponse() as HttpWebResponse;
           
        //}


        //public XmlDocument getPayload(string messageId) {
        //    string dir = @"C:\ATT\PayloadsDownloads\3014\Target";
        //    DirectoryInfo di = new DirectoryInfo(dir);
        //    var f = di.GetFiles().Where(f1 => f1.Name.Contains(messageId)).FirstOrDefault();
        //    if (f != null) {
        //        System.Xml.XmlDocument xDoc = new System.Xml.XmlDocument();
        //        xDoc.Load(f.FullName);
        //        if (xDoc.FirstChild.NodeType == System.Xml.XmlNodeType.XmlDeclaration) {
        //            xDoc.RemoveChild(xDoc.FirstChild);
        //        }

        //        string before = "<soapenv:Envelope xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\">\n<soapenv:Header/>\n<soapenv:Body>\n";
        //        string after = "</soapenv:Body>\n" +"</soapenv:Envelope>\n";
        //        var xml = before + xDoc.OuterXml + after;
        //        xDoc.LoadXml(xml);
        //        return xDoc;
               
        //    }
            
        //    return null;
        //}

        //private string buildUrl(string host, string port, string senderNameSpace, string component, string senderInterface) {
        //    string urlTemp = WebUtility.UrlEncode(senderNameSpace + "^" + senderInterface);
        //    string url = $"http://{host}:{port}/sap/xi/engine?type=entry&version=3.0&Sender.Service={component}&Interface={urlTemp}&QualityOfService=ExactlyOnce";
        //    // http://%s:%s/sap/xi/engine?type=entry&version=3.0&Sender.Service=%s&Interface=%s&QualityOfService=ExactlyOnce
        //    return url;
        //}


        //private static HttpWebRequest CreateWebRequest(string url, string action) {
        //    HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
        //    webRequest.Headers.Add("SOAPAction", action);
        //    webRequest.ContentType = "text/xml;charset=\"utf-8\"";
        //    webRequest.Accept = "text/xml";
        //    webRequest.Method = "POST";
        //    return webRequest;
        //}

      

       


        public void load() {
            //CookieCollection cookies = new CookieCollection();


            //HttpWebRequest req = HttpWebRequest.Create("http://sapxip-ent.sapnet.hpecorp.net:50000/hp.com~com.hp.pi.core.web/svc/object/config/all") as HttpWebRequest;

            //req.Credentials = new NetworkCredential("21746957", "Ojo@8gat");


            //HttpWebResponse resp = req.GetResponse() as HttpWebResponse;

            //for(int i =0;i<resp.Headers.Count;i++)
            //{
            //    string name = resp.Headers.GetKey(i);
            //    if (name != "Set-Cookie")
            //        continue;
            //    string value = resp.Headers.Get(i);
            //    foreach(var v in value.Split(','))
            //    {
            //        Match match = Regex.Match(v, "(.+?)=(.+?);");
            //        if (match.Captures.Count == 0)
            //            continue;

            //        cookies.Add(new Cookie(match.Groups[1].ToString(),match.Groups[2].ToString(),"/",req.Host.Split(':')[0]));
            //    }
            //}


            HttpWebRequest req = HttpWebRequest.Create("http://sapxip-ent.sapnet.hpecorp.net:50000/hp.com~com.hp.pi.core.web/svc/event/downloadPayloads") as HttpWebRequest;
            req.Credentials = new NetworkCredential("21746957", "Ojo@8gat");
            req.Method = "POST";
            req.CookieContainer = new CookieContainer();
            //req.CookieContainer.Add(cookies);

            req.ContentType = "application/x-www-form-urlencoded";

            string postData = $"msgids=002481D9B8041ED681A06429CABA4700{Environment.NewLine}002481D9B8041ED681A064300FCE4700{Environment.NewLine}&lastVersion=false&fullEnvelope=false";
            var data = Encoding.ASCII.GetBytes(postData);

            using (var stream = req.GetRequestStream()) {
                stream.Write(data, 0, data.Length);
            }


            HttpWebResponse resp = req.GetResponse() as HttpWebResponse;




            using (var output = resp.GetResponseStream()) {

                FileStream fs = File.OpenWrite(@"c:\ATT\text.zip");
                output.CopyTo(fs);
                fs.Close();
            }

            using (IsolatedStorageFile isf = IsolatedStorageFile.GetUserStoreForSite()) {
                using (IsolatedStorageFileStream isfs = isf.OpenFile("CookieExCookies",
                    FileMode.OpenOrCreate, FileAccess.Write)) {
                    using (StreamWriter sw = new StreamWriter(isfs)) {
                        foreach (Cookie cookieValue in resp.Cookies) {
                            sw.WriteLine("Cookie: " + cookieValue.ToString());
                        }
                        sw.Close();
                    }
                }

            }
        }

        private void Button_Click(object sender, RoutedEventArgs e) {
            //stu.Name = "Zhou Yang_abc";
        }
    }
}
