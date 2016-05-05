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

        public static void Main() {
          //  SampleFill();

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

           var a= Tools.GetDataEntites<ATT.Scripts.AIFIDocNumbers>(@"E:\AIF.txt", '|');

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
