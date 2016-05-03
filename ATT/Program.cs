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



        public static void Main() {


            //AIFMassUpload s = new AIFMassUpload();
            //s.SetInputData(new AIFMassUploadData() { UserName = "20242630", Password = "1qaz@wsx", Client = "100", Address = "saplh1-ent.sapnet.hpecorp.net" });
            //s.Login();



            TestC c = new TestC();
            TestB b = new TestB();
            Console.WriteLine(TestC.A);
            SampleFill();

            PayloadsUploader p = new PayloadsUploader();
            PayloadsUploaderData d = new PayloadsUploaderData();
            d.UserName = "21746957";
            d.Password = "Ojo@6gat";
            d.SetTaskId(3014);
            p.SetInputData(d);

            p.Upload();



        }


    }
}
