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

namespace WebTest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
           
            load();

        }


        public void load()
        {
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

            using (var stream = req.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }


            HttpWebResponse resp = req.GetResponse() as HttpWebResponse;


       
            
            using (var output = resp.GetResponseStream() )
            {
               
                FileStream fs = File.OpenWrite(@"c:\ATT\text.zip");
                output.CopyTo(fs);
                fs.Close();
            }
            
            using (IsolatedStorageFile isf = IsolatedStorageFile.GetUserStoreForSite())
            {
                using (IsolatedStorageFileStream isfs = isf.OpenFile("CookieExCookies",
                    FileMode.OpenOrCreate, FileAccess.Write))
                {
                    using (StreamWriter sw = new StreamWriter(isfs))
                    {
                        foreach (Cookie cookieValue in resp.Cookies)
                        {
                            sw.WriteLine("Cookie: " + cookieValue.ToString());
                        }
                        sw.Close();
                    }
                }

            }
        }
    }
}
