using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAPAutomation;
using SAPFEWSELib;
using System.Net;
using Young.Data;
using ATT.Scripts;
using System.Transactions;
using System.Data.SqlClient;
using ScriptRunner.Interface;
using System.IO.Compression;
using System.Text.RegularExpressions;
using System.IO;
using System.Xml;
using ATT.Data;
using ATT.Data.Entity;
using System.Data.Entity;
using System.Diagnostics;
using System.Xml.Serialization;

namespace ATT
{
    public class TestA
    {
        public static int A = 1;
    }

    public class TestB : TestA
    {
        public TestB() {
            A = 2;
        }
    }

    public class TestC : TestA
    {
        public TestC() {
            if (A == 1) {
                DoC();
            }
        }

        public void DoC() {
            Console.WriteLine("Funck");
        }
    }



    partial class Program
    {
        public static void TrackStatus(List<string> _iDocNumbers) {
            
            SAPTestHelper.Current.SAPGuiSession.StartTransaction("ZIDOCAUDREP");
            SAPTestHelper.Current.MainWindow.FindByName<GuiCTextField>("S_CREDAT-LOW").Text = (new DateTime(2016, 4, 23)).ToString("dd.MM.yyyy");
            SAPTestHelper.Current.MainWindow.FindByName<GuiCTextField>("S_CREDAT-HIGH").Text = (new DateTime(2016, 5, 5)).ToString("dd.MM.yyyy");
            SAPTestHelper.Current.MainWindow.FindByName<GuiCTextField>("S_CRETIM-LOW").Text = "00:00:00";
            SAPTestHelper.Current.MainWindow.FindByName<GuiCTextField>("S_CRETIM-HIGH").Text = "24:00:00";
            SAPTestHelper.Current.MainWindow.FindByName<GuiCTextField>("S_STATUS-LOW").Text = "*";
            SAPTestHelper.Current.MainWindow.FindByName<GuiCTextField>("P_DIRECT").Text = "2";


            SAPTestHelper.Current.MainWindow.FindByName<GuiRadioButton>("R_OTHERS").Select();
            SAPTestHelper.Current.MainWindow.FindByName<GuiCTextField>("S_MESTYP-LOW").Text = "ACC_DOCUMENT";

            SAPTestHelper.Current.MainWindow.FindByName<GuiCheckBox>("P_DOWNLD").Selected = true;
            SAPTestHelper.Current.MainWindow.FindByName<GuiRadioButton>("P_FILE").Select();
            SAPTestHelper.Current.MainWindow.FindByName<GuiTextField>("P_CPATH").Text = @"C:\AIF\Temp.txt";

            SAPTestHelper.Current.MainWindow.FindByName<GuiRadioButton>("P_DOC").Select();

            SAPTestHelper.Current.MainWindow.FindByName<GuiButton>("%_S_IDOCNO_%_APP_%-VALU_PUSH").Press();
            SAPTestHelper.Current.PopupWindow.FindDescendantByProperty<GuiTableControl>().SetBatchValues(_iDocNumbers);
            SAPTestHelper.Current.PopupWindow.FindByName<GuiButton>("btn[8]").Press();

            SAPTestHelper.Current.MainWindow.FindByName<GuiButton>("btn[8]").Press();

            if (SAPTestHelper.Current.PopupWindow != null)
                SAPTestHelper.Current.PopupWindow.FindByName<GuiButton>("btn[0]").Press();

            SAPTestHelper.Current.CloseSession();
        }


        static void loginLH7() {
            SAPLoginData d = new SAPLoginData();
            d.UserName = "21688419";
            d.Password = "1qaz@wsx";
            d.Address = "saplh7.sapnet.hp.com";
            d.Client = "100";
            UIHelper.Login(d);
        }

        static void Test(DateTime dt) {

            dt = dt.AddDays(2);
        }

        static string lastDoc;
        static int Count;
        static void BD87(string iDoc) {
            
            SAPTestHelper.Current.SAPGuiSession.StartTransaction("BD87");
            if(SAPTestHelper.Current.SAPGuiSession.Info.SystemName != "LH7") {
                SAPTestHelper.Current.MainWindow.FindByName<GuiButton>("btn[15]").Press();
                SAPTestHelper.Current.SAPGuiSession.StartTransaction("BD87");
                Console.WriteLine(lastDoc + "---" + Count.ToString());
            }
          
            SAPTestHelper.Current.MainWindow.FindByName<GuiCTextField>("SX_DOCNU-LOW").Text = iDoc;
            

            SAPTestHelper.Current.MainWindow.FindByName<GuiCTextField>("SX_CREDA-LOW").Text =  (new DateTime(2016,5,5)).ToString("dd.MM.yyyy");
            SAPTestHelper.Current.MainWindow.FindByName<GuiCTextField>("SX_CREDA-HIGH").Text = (new DateTime(2016, 5, 5)).ToString("dd.MM.yyyy");
            SAPTestHelper.Current.MainWindow.FindByName<GuiCTextField>("SX_CRETI-LOW").Text = "00:00:00";
            SAPTestHelper.Current.MainWindow.FindByName<GuiCTextField>("SX_CRETI-HIGH").Text = "24:00:00";

            SAPTestHelper.Current.MainWindow.FindByName<GuiCTextField>("SX_UPDDA-LOW").Text = (new DateTime(2016, 5, 5)).ToString("dd.MM.yyyy");
            SAPTestHelper.Current.MainWindow.FindByName<GuiCTextField>("SX_UPDDA-HIGH").Text = (new DateTime(2016, 5, 5)).ToString("dd.MM.yyyy");
            SAPTestHelper.Current.MainWindow.FindByName<GuiCTextField>("SX_UPDTI-LOW").Text = "00:00:00";
            SAPTestHelper.Current.MainWindow.FindByName<GuiCTextField>("SX_UPDTI-HIGH").Text = "24:00:00";


            SAPTestHelper.Current.MainWindow.FindByName<GuiCTextField>("SX_STATU-LOW").Text = "64";
            SAPTestHelper.Current.MainWindow.FindByName<GuiButton>("btn[8]").Press();

            var tree = SAPTestHelper.Current.MainWindow.FindDescendantByProperty<GuiTree>();
            var n = tree.ChooseNode("LH7");
            var number = tree.GetItemText(n, "Column3");

            if (number != "0") {
                var n1 = tree.ChooseNode("LH7->IDoc->IDoc");
                SAPTestHelper.Current.MainWindow.FindByName<GuiButton>("btn[8]").Press();
            }

            lastDoc = iDoc;
            Count++;
        }

        public static void Main() {
            //  SampleFill();

            loginLH7();

           using(var db = new ATT.Data.AIF.AIFDbContext()) {
                var iDocs = db.IDocs.Where(s => s.Status == "64").ToList();
                foreach(var item in iDocs) {
                    BD87(item.IDocNumber);
                }
                TrackStatus(iDocs.Select(i => i.IDocNumber).ToList());
            }

           


            DateTime start = DateTime.Now;
            DirectoryInfo di = new DirectoryInfo(@"C:\AIF");
            var files = di.GetFiles().Where(f => f.Name.Contains("AIFIDoc_ITG"));
            int total = files.Count();
            int current = 0;
            using(var db = new ATT.Data.AIF.AIFDbContext()) {
                foreach (var f in files) {

                    var taskid = int.Parse(f.Name.Split('_')[2]);
                    var tempItems = Tools.GetDataEntites<IDoc>(f.FullName);
                    foreach(var item in tempItems) {
                        var idoc = item.GetIDocs();
                        idoc.Tid = taskid;
                        db.IDocs.Add(idoc);
                    }
                    current++;
                    Console.WriteLine("{0}/{1}", current, total);
                    db.SaveChanges();

                }
                
            }

            Console.WriteLine(DateTime.Now.Subtract(start));

            AIFMassUploadData d = new AIFMassUploadData();
            //d.AIFTask = new Data.AIF.Tasks() {
            //    Interfaces = new Data.AIF.Interfaces() {
            //        MsgCode = "TW",
            //        MsgType = "ACC_DOCUMENT",
            //        MsgFunction = "ICS",
            //        PartnerNo = "HPS"
            //    }
            //};
            d.LH1 = new SAPLoginData() {
                UserName = "20242630",
                Password = "1qaz@wsx",
                Client = "100",
                Address = "saplh1-ent.sapnet.hpecorp.net"
            };
            d.LH7 = new SAPLoginData() {
                UserName = "21688419",
                Password = "1qaz@wsx",
                Address = "saplh7.sapnet.hp.com",
                Client = "100"
            };
            d.Start = DateTime.Now.AddDays(-2);
            d.End = DateTime.Now;

            AIFMassUpload s3 = new AIFMassUpload();
            s3.SetInputData(d);
            s3.LoginLH7();
            s3.TrackStatus();

            using (var db = new ATT.Data.AIF.AIFDbContext()) {
                db.Database.Log += (s) => Console.WriteLine(s);
                //db.Tasks.Add(d.AIFTask);
                db.SaveChanges();
            }
            ScriptEngine<AIFMassUpload, AIFMassUploadData> script = new ScriptEngine<AIFMassUpload, AIFMassUploadData>();
            script.Run(d);

            //AIFMassUpload s = new AIFMassUpload();
            //s.SetInputData(new AIFMassUploadData() { UserName = "20242630", Password = "1qaz@wsx", Client = "100", Address = "saplh1-ent.sapnet.hpecorp.net" });
            //s.Login();

            var a = Tools.GetDataEntites<ATT.Scripts.AIFIDocNumbers>(@"E:\AIF.txt", '|');

            TestC c = new TestC();
            TestB b = new TestB();
            Console.WriteLine(TestC.A);
            SampleFill();

            //PayloadsUploader p = new PayloadsUploader();
            //PayloadsUploaderData d = new PayloadsUploaderData();
            //d.UserName = "21746957";
            //d.Password = "Ojo@6gat";
            //d.SetTaskId(3014);
            //p.SetInputData(d);

            //p.Upload();



        }


    }
}
