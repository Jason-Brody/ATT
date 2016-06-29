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
using System.Threading;
using System.Timers;
using SharedLib;

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

    public static class TestStatic
    {
        public static T AppendTo<T>(this T item,ICollection<T> collections) {
            collections.Add(item);
            return item;
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
            d.Password = "2wsx#edc";
            d.Address = "saplh7.sapnet.hp.com";
            d.Client = "100";

            SAPLogon l = new SAPLogon();
            l.StartProcess();
            l.OpenConnection(d.Address);
            //l.Login(d.UserName, d.Password, d.Client, d.Language);
            
        }

        static void Test(DateTime dt) {

            dt = dt.AddDays(2);
        }

        static string lastDoc;
        static int Count;
        static void BD87(string iDoc) {

            SAPTestHelper.Current.SAPGuiSession.StartTransaction("BD87");
            if (SAPTestHelper.Current.SAPGuiSession.Info.SystemName != "LH7") {
                SAPTestHelper.Current.MainWindow.FindByName<GuiButton>("btn[15]").Press();
                SAPTestHelper.Current.SAPGuiSession.StartTransaction("BD87");
                Console.WriteLine(lastDoc + "---" + Count.ToString());
            }

            SAPTestHelper.Current.MainWindow.FindByName<GuiCTextField>("SX_DOCNU-LOW").Text = iDoc;


            SAPTestHelper.Current.MainWindow.FindByName<GuiCTextField>("SX_CREDA-LOW").Text = (new DateTime(2016, 5, 5)).ToString("dd.MM.yyyy");
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

       static async void Test(int i) {
            
            await Task.Run(()=> { Task.Delay(2000).Wait(); });
            Console.WriteLine(i);
            
            
        }

        static async Task AsyncTest1(int seconds) {
            await Task.Delay(new TimeSpan(0,0,seconds));
            Console.WriteLine($"Delay {seconds}");
        }

        static async void test() {
            await AsyncTest1(5);
            await AsyncTest1(1);
        }

        static void show(int i) {
            Console.WriteLine(i);
        }

        public static void Main() {

            Task[] tasks = new Task[2];
            tasks[0] = Task.Run(() => loginLH7());
            //tasks[1] = Task.Run(() => Task.Delay(500).Wait());
            tasks[1] = Task.Run(() => loginLH7());
            Task.WaitAll(tasks);

            //DateTime dt = DateTime.Now;
            //Thread.Sleep(2000);
            //DateTime dt1 = DateTime.Now;
            //Console.WriteLine(dt1 < dt);


            //List<System.Timers.Timer> timers = new List<System.Timers.Timer>();

            //for(int i = 0; i < 10; i++) {
            //    System.Timers.Timer timer = new System.Timers.Timer(1000);
            //    timer.Elapsed += (o, e) => { Console.WriteLine(Thread.CurrentThread.ManagedThreadId);Thread.Sleep(2000); };
            //    timers.Add(timer);
            //}



            //ThreadPool.SetMinThreads(2, 2);
            //ThreadPool.SetMaxThreads(2, 2);

            //timers.ForEach(t => t.Start());

            //timers.ForEach(t => {  t.Stop(); Thread.Sleep(5000); });

            Console.ReadLine();
            //List<Task> t = new List<Task>();
            //for (int i = 0; i < 100; i++) {

            //    Console.WriteLine("Hi #:" + i.ToString());

            //    for(int j = 0; j < 5; j++) {

            //        Action a = new Action(() => { Thread.Sleep(50); Console.WriteLine(i*5+j); });
            //        Task.Factory.StartNew(a).AppendTo(t);

            //    }

                



                
            //    //tempT.Start();
            //    //Task.Run(()=> { Thread.Sleep(1000); Console.WriteLine(i); }).AppendTo(t);
            //}

            //Task.WaitAll(t.ToArray());

            //Console.WriteLine("Finished");

            //Console.ReadLine();
            //TrackError();
            //  SampleFill();
           
            //var tpp = typeof(int?);
            //var tpp2 = typeof(Nullable<int>);
            //var args = tpp.GenericTypeArguments[0];
            //if (tpp.IsGenericType && tpp.GetGenericTypeDefinition() == typeof(Nullable<>) && tpp.GenericTypeArguments[0].IsPrimitive) {
            //    Console.WriteLine(true);
            //}
            //var result = tpp == tpp2;
            //TrackError();
            //loginLH7();

            //using (var db = new ATT.Data.AIF.AIFDbContext()) {
            //    var iDocs = db.IDocs.Where(s => s.Status == "64").ToList();
            //    foreach (var item in iDocs) {
            //        BD87(item.IDocNumber);
            //    }
            //    TrackStatus(iDocs.Select(i => i.IDocNumber).ToList());
            //}




            //DateTime start = DateTime.Now;
            //DirectoryInfo di = new DirectoryInfo(@"C:\AIF");
            //var files = di.GetFiles().Where(f => f.Name.Contains("AIFIDoc_ITG"));
            //int total = files.Count();
            //int current = 0;
            //using (var db = new ATT.Data.AIF.AIFDbContext()) {
            //    foreach (var f in files) {

            //        var taskid = int.Parse(f.Name.Split('_')[2]);
            //        var tempItems = Tools.GetDataEntites<IDoc>(f.FullName);
            //        foreach (var item in tempItems) {
            //            var idoc = item.GetIDocs();
            //            idoc.Tid = taskid;
            //            db.IDocs.Add(idoc);
            //        }
            //        current++;
            //        Console.WriteLine("{0}/{1}", current, total);
            //        db.SaveChanges();

            //    }

            //}

            //Console.WriteLine(DateTime.Now.Subtract(start));

            //AIFMassUploadData d = new AIFMassUploadData();
            ////d.AIFTask = new Data.AIF.Tasks() {
            ////    Interfaces = new Data.AIF.Interfaces() {
            ////        MsgCode = "TW",
            ////        MsgType = "ACC_DOCUMENT",
            ////        MsgFunction = "ICS",
            ////        PartnerNo = "HPS"
            ////    }
            ////};
            //d.LH1 = new SAPLoginData() {
            //    UserName = "20242630",
            //    Password = "1qaz@wsx",
            //    Client = "100",
            //    Address = "saplh1-ent.sapnet.hpecorp.net"
            //};
            //d.LH7 = new SAPLoginData() {
            //    UserName = "21688419",
            //    Password = "1qaz@wsx",
            //    Address = "saplh7.sapnet.hp.com",
            //    Client = "100"
            //};
            //d.Start = DateTime.Now.AddDays(-2);
            //d.End = DateTime.Now;

            //AIFMassUpload s3 = new AIFMassUpload();
            //s3.SetInputData(d);
            //s3.LoginLH7();
            //s3.TrackStatus();

            //using (var db = new ATT.Data.AIF.AIFDbContext()) {
            //    db.Database.Log += (s) => Console.WriteLine(s);
            //    //db.Tasks.Add(d.AIFTask);
            //    db.SaveChanges();
            //}
            //ScriptEngine<AIFMassUpload, AIFMassUploadData> script = new ScriptEngine<AIFMassUpload, AIFMassUploadData>();
            //script.Run(d);

            ////AIFMassUpload s = new AIFMassUpload();
            ////s.SetInputData(new AIFMassUploadData() { UserName = "20242630", Password = "1qaz@wsx", Client = "100", Address = "saplh1-ent.sapnet.hpecorp.net" });
            ////s.Login();

            //var a = Tools.GetDataEntites<ATT.Scripts.AIFIDocNumbers>(@"E:\AIF.txt", '|');

            //TestC c = new TestC();
            //TestB b = new TestB();
            //Console.WriteLine(TestC.A);
            //SampleFill();

            ////PayloadsUploader p = new PayloadsUploader();
            ////PayloadsUploaderData d = new PayloadsUploaderData();
            ////d.UserName = "21746957";
            ////d.Password = "Ojo@6gat";
            ////d.SetTaskId(3014);
            ////p.SetInputData(d);

            ////p.Upload();



        }

       
    }
}
