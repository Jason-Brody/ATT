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
using ATT.Scripts.SAPGui;
using ATT.Scripts.PayLoads;
using ScriptRunner.Interface;
namespace ATT
{
    public class Class1
    {
        public static void Main()
        {




            PayloadsDownloaderModel m = new PayloadsDownloaderModel();
            m.DownloadUrl = "http://sapxip-ent.sapnet.hpecorp.net:50000/hp.com~com.hp.pi.core.web/svc/event/downloadPayloads";
            m.Username = "21746957";
            m.Password = "Ojo@8gat";
            m.DownloadPatchSize = 20;

            ScriptEngine<PayloadsDownloader, PayloadsDownloaderModel> script = new ScriptEngine<PayloadsDownloader, PayloadsDownloaderModel>();
            script.Run(m);


           


            //SAPAutomation.SAPLogon logon = new SAPAutomation.SAPLogon();
            //logon.StartProcess();
            //logon.OpenConnection("saplh1-ent.sapnet.hpecorp.net");
            //logon.Login("21746957", "Ojo@1gat6", "100", "EN");

        }


        public static void EDIKeyTask()
        {
            var data = ExcelHelper.Current.Open("ATT.xlsx").ReadAll();
            EDIKeyDataModel dataModel = new EDIKeyDataModel();

            dataModel.Interfaces = data.Tables["Interfaces"].ToList<SAPInterface>();
            dataModel.CompanyCodes = data.Tables["CompanyCodes"].ToList<CompanyCode>();
            dataModel.DocTypes = data.Tables["DocumentTypes"].ToList<DocType>();

            dataModel.Address = "saplh1-ent.sapnet.hpecorp.net";
            dataModel.UserName = "21746957";
            dataModel.Password = "Ojo@1gat7";
            dataModel.Client = "100";
            dataModel.Language = "EN";
            dataModel.StartDate = "20.04.2016";
            dataModel.EndDate = "20.04.2016";
            dataModel.StartTime = 1;
            dataModel.Interval = 2;
            dataModel.InterfaceCount = 1;


            ScriptEngine<EDIKeyTask, EDIKeyDataModel> script = new ScriptEngine<EDIKeyTask, EDIKeyDataModel>();
            script.Run(dataModel);
        }
    }
}
