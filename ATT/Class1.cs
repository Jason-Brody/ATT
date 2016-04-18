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

namespace ATT
{
    public class Class1
    {
        public static void Main()
        {
            int i = int.Parse(Console.ReadLine());
            Random rd = new Random();
            using (var dbEntity = new ATT.Data.AttDbContext())
            {
                dbEntity.Database.Log += (s) => Console.WriteLine(s);

                using (var dbTransaction = dbEntity.Database.BeginTransaction(System.Data.IsolationLevel.Serializable))
                {
                    

                    var keys = dbEntity.EDIKeys.Where(c => c.StatusId < i).ToList();
                    foreach (var k in keys)
                    {
                        k.StatusId = rd.Next(1000, 2000);
                        Task.Delay(20).Wait();
                    }
                    dbEntity.SaveChanges();
                    dbTransaction.Commit();
                }

                
            }
            






            //    var edikeys = Tools.GetDataEntites<ATT.Data.EDIKeys>(@"C:\ATT\MessageDownloads\EDIArchiveKey_APF_57784172-c4c9-4282-b472-ea746080a5d0.txt", "|");
            //ATT.Data.AttDbContext db = new Data.AttDbContext();
            //db.EDIKeys.AddRange(edikeys);
            //db.SaveChanges();

            //var data = ExcelHelper.Current.Open("ATT.xlsx").ReadAll();
            //ATT.Scripts.SAPGui.EDIKeyDataModel dataModel = new Scripts.SAPGui.EDIKeyDataModel();
          
            //dataModel.Interfaces = data.Tables["Interfaces"].ToList<ATT.Scripts.SAPGui.SAPInterface>();
            //dataModel.CompanyCodes = data.Tables["CompanyCodes"].ToList<ATT.Scripts.SAPGui.CompanyCode>();
            //dataModel.DocTypes = data.Tables["DocumentTypes"].ToList<ATT.Scripts.SAPGui.DocType>();

            //dataModel.Address = "saplh1-ent.sapnet.hpecorp.net";
            //dataModel.UserName = "21746957";
            //dataModel.Password = "Ojo@1gat7";
            //dataModel.Client = "100";
            //dataModel.Language = "EN";
            //dataModel.StartDate = "18.04.2016";
            //dataModel.EndDate = "18.04.2016";
            //dataModel.StartTime = 1;
            //dataModel.Interval = 2;
            //dataModel.InterfaceCount = 1;

            //ATT.Scripts.SAPGui.EDIKeyTask t = new ATT.Scripts.SAPGui.EDIKeyTask();
            //ScriptRunner.Interface.ScriptEngine<ATT.Scripts.SAPGui.EDIKeyDataModel> script = new ScriptRunner.Interface.ScriptEngine<ATT.Scripts.SAPGui.EDIKeyDataModel>(t);
            //script.Run(dataModel);
            




           



            //SAPAutomation.SAPLogon logon = new SAPAutomation.SAPLogon();
            //logon.StartProcess();
            //logon.OpenConnection("saplh1-ent.sapnet.hpecorp.net");
            //logon.Login("21746957", "Ojo@1gat6", "100", "EN");

        }


        public static void Test(int a)
        {
            Console.WriteLine(a);
        }
    }
}
