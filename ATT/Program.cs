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
    public static class TestC {
        public static int A { get; } = 1;

        public static Lazy<TestB> Test { get; } = new Lazy<TestB>(()=> { return new TestB("a");  });
    }

    public class TestB
    {
        public TestB(string v) {
            Console.WriteLine(v);
        }
    }



    partial class Program
    {

        //public static void UnzipFile(string file) {

        //    string regex = ".*\\\\([^\\.]+)\\..*";
        //    Match match = Regex.Match(file, regex);
        //    var dir = Path.Combine(GlobalConfig.PayLoadsDownload.Value.WorkFolder, match.Groups[1].ToString());
        //    ZipFile.ExtractToDirectory(file, dir);
        //}


        public static void Test2() {
            SAPLoginData d = new SAPLoginData();
            d.Address = "pi-itg-01-idoc.sapnet.hpecorp.net";
            d.UserName = "21746957";
            d.Password = "Ojo@6gat";
            d.Client = "020";
            d.Language = "EN";
            UIHelper.Login(d);
            
        }

        static Task MyTask(int i) {

            

            return Task.Run(() => {
                Console.WriteLine("Thread {0} is running task {1}", System.Threading.Thread.CurrentThread.ManagedThreadId, i);
            });
        }

        public static void Main() {





        
            var exeFile = Path.Combine(@"E:\GitHub\SAPTestCenter\ATT\ATT.Robot\bin\Debug", "ATT.Robot.exe");
            ProcessStartInfo psInfo = new ProcessStartInfo();
            psInfo.FileName = exeFile;
            psInfo.Arguments = $"{ATTTask.GetMessageId} {0}";
            Process.Start(psInfo);

            //EDIKeyTask();



            //using (var db = new AttDbContext()) {
            //    db.Database.Log = (s) => { Console.WriteLine(s); };
            //    var interfaces = db.SAPInterfaces.Take(2).Include(s => s.SAPCompanyCodes).Include(s => s.SAPDocTypes).ToList();
            //}


            //var table = Tools.ReadToTable(@"C:\ATT\1.txt");

            //var dts = Tools.GetDataEntites<ATT.Data.Entity.MsgIDs_ITG>(@"C:\ATT\1.txt");






            //Console.WriteLine("Start {0}", DateTime.Now);
            //Dictionary<string, EDIKeyTemp> dic = new Dictionary<string, EDIKeyTemp>();

            //for (int i = 0; i < 100; i++) {
            //    dic.Add("adsf" + i.ToString(), new EDIKeyTemp() { Id = 449 + i, Awsys = "PN1200", IDocType = "FIDCC2X" });
            //}

            //Console.WriteLine("Open DB {0}", DateTime.Now);
            //using (var db = new ATT.Data.AttDbContext()) {

            //    //db.Database.Log += (s) => {
            //    //    Console.WriteLine(s);
            //    //};
            //    Console.WriteLine(DateTime.Now);
            //    using (var transaction = db.Database.BeginTransaction()) {

            //        foreach (var item in dic) {
            //            db.Database.ExecuteSqlCommand("exec SP_UpdateSource @id,@Awsys,@IDocType,@DownloadDt",
            //                new SqlParameter("id", item.Value.Id),
            //                new SqlParameter("Awsys", item.Value.Awsys),
            //                new SqlParameter("IDocType", item.Value.IDocType),
            //                new SqlParameter("DownloadDt", DateTime.UtcNow));
            //        }

            //        transaction.Commit();
            //        Console.WriteLine(DateTime.Now);
            //    }
            //}


            //using (var db = new ATT.Data.AttDbContext()) {
            //    var configs = db.Database.SqlQuery<ATT.Data.VW_EDITransFormConfig>("select * from VW_EDITransFormConfig").ToList();
            //}








            //using (var db = new ATT.Data.AttDbContext()) {

            //    //db.Database.Log += (s) => {
            //    //    Console.WriteLine(s);
            //    //};

            //    using (var transaction = db.Database.BeginTransaction()) {

            //        foreach (var item in dic) {
            //            db.Database.ExecuteSqlCommand("exec SP_UpdateSource @id,@Awsys,@IDocType,@DownloadDt",
            //                new SqlParameter("id", item.Value.Id),
            //                new SqlParameter("Awsys", item.Value.Awsys),
            //                new SqlParameter("IDocType", item.Value.IDocType),
            //                new SqlParameter("DownloadDt", DateTime.UtcNow));
            //        }

            //        transaction.Commit();

            //    }
            //}





            //SAPAutomation.SAPLogon logon = new SAPAutomation.SAPLogon();
            //logon.StartProcess();
            //logon.OpenConnection("saplh1-ent.sapnet.hpecorp.net");
            //logon.Login("21746957", "Ojo@1gat6", "100", "EN");

        }

        public static void UpdateEDIKeyTask() {
            PayloadsUpdateData d = new PayloadsUpdateData();
            d.SetTaskId(40);
           
            ScriptEngine<PayloadsUpdate, PayloadsUpdateData> script = new ScriptEngine<PayloadsUpdate, PayloadsUpdateData>();
            script.Run(d);
        }

        public static void PayloadTask() {
            PayloadsDownloaderData m = new PayloadsDownloaderData();
            m.DownloadUrl = "http://sapxip-ent.sapnet.hpecorp.net:50000/hp.com~com.hp.pi.core.web/svc/event/downloadPayloads";
            m.UserName = "21746957";
            m.Password = "Ojo@8gat";
          

            ScriptEngine<PayloadsDownloader, PayloadsDownloaderData> script = new ScriptEngine<PayloadsDownloader, PayloadsDownloaderData>();
            script.Run(m);
        }

        public static void EDIKeyTask()
        {
            //var data = ExcelHelper.Current.Open("ATT.xlsx").ReadAll();
            MSGIDTaskData d = new MSGIDTaskData();

            //dataModel.Interfaces = data.Tables["Interfaces"].ToList<SAPInterface>();
            //dataModel.CompanyCodes = data.Tables["CompanyCodes"].ToList<CompanyCode>();
            //dataModel.DocTypes = data.Tables["DocumentTypes"].ToList<DocType>();

            d.Address = "saplh1-ent.sapnet.hpecorp.net";
            d.UserName = "21746957";
            d.Password = "Ojo@1gat7";
            d.Client = "100";
            d.Language = "EN";
            d.Start = new ATTDate(2016, 4, 25,1);
            
            d.Interval = 2;
            d.InterfaceCount = 10;


            ScriptEngine<MSGIDTask, MSGIDTaskData> script = new ScriptEngine<MSGIDTask, MSGIDTaskData>();
            script.Run(d);
        }
    }
}
