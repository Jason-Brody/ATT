using ATT.Data;
using ATT.Data.ATT;
using ATT.Scripts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ATT
{
    partial class Program
    {
        static void AddTask<T>(T obj,ATTTask t) {
            XmlSerializer xs = new XmlSerializer(typeof(T));
            StringBuilder sb = new StringBuilder();
            StringWriter sw = new StringWriter(sb);
            xs.Serialize(sw, obj);

            using (var db = new AttDbContext()) {
                var t1 = new TaskDataConfigs();
                t1.AttTask = t;
                t1.Data = sb.ToString();
                t1.TypeName = typeof(T).FullName;
                t1.Name = t.ToString();
                db.TaskDataConfigs.Add(t1);
                db.SaveChanges();
            }
        }

        //static void SampleFill() {
        //    using (var db = new AttDbContext()) {
        //        db.TaskDataConfigs.RemoveRange(db.TaskDataConfigs.ToList());
        //        db.SaveChanges();
        //    }


        //    LHTrackData d = new LHTrackData();
        //    d.UserName = "21688419";
        //    d.Password = "4rfv%tgb";
        //    d.Client = "100";
        //    d.Address = "saplh4.sapnet.hp.com";
        //    d.Interval = 2;
        //    d.IDocStatus = "*";
        //    d.Start = new ATTDate(2016, 4, 20);
            
        //    AddTask(d,ATTTask.LHTrack);

        //    MSGIDTaskData d1= new MSGIDTaskData();
        //    d1.Address = "saplh1-ent.sapnet.hpecorp.net";
        //    d1.UserName = "21746957";
        //    d1.Password = "Ojo@1gat7";
        //    d1.Client = "100";
        //    d1.Language = "EN";
        //    d1.Start = new ATTDate(2016, 4, 26, 0);
        //    d1.Interval = 2;
        //    //d1.InterfaceCount = 1;
        //    AddTask(d1, ATTTask.GetMessageId);

        //    PayloadsUpdateData d2 = new PayloadsUpdateData();
        //    AddTask(d2, ATTTask.UpdatePayloads);

        //    PayloadsUploaderData d3 = new PayloadsUploaderData();
        //    d3.UserName = "21746957";
        //    d3.Password = "Ojo@6gat";
        //    d3.Host = "pi-itg-01-idoc.sapnet.hpecorp.net";
        //    d3.Port = 63100;
        //    d3.ProxyHost = "web-proxy.austin.hp.com";
        //    d3.ProxyHostPort = 8080;
        //    AddTask(d3, ATTTask.UploadPayloads);

        //    PIITrackData d4 = new PIITrackData();
        //    d4.Address = "pi-itg-01-idoc.sapnet.hpecorp.net";
        //    d4.UserName = "21746957";
        //    d4.Password = "Ojo@6gat";
        //    d4.Client = "020";
        //    d4.Language = "EN";
        //    d4.Start = new ATTDate(2016, 4, 26, 0);
        //    AddTask(d4, ATTTask.PIITrack);

        //    PayloadsDownloaderData d5 = new PayloadsDownloaderData();
        //    d5.DownloadUrl = "http://sapxip-ent.sapnet.hpecorp.net:50000/hp.com~com.hp.pi.core.web/svc/event/downloadPayloads";
        //    d5.UserName = "21746957";
        //    d5.Password = "Ojo@8gat";
        //    AddTask(d5, ATTTask.DownloadPayloads);

        //    PayloadsData d6 = new PayloadsData();
        //    d6.DownloadData = d5;
        //    d6.UpdateData = d2;
        //    d6.UploadData = d3;
        //    AddTask(d6, ATTTask.DownloadAndTransform);


        //    AIFMassUploadData d7 = new AIFMassUploadData();
        //    d7.LH1 = new SAPLoginData() {
        //        UserName = "20242630",
        //        Password = "1qaz@wsx",
        //        Client = "100",
        //        Address = "saplh1-ent.sapnet.hpecorp.net"
        //    };
        //    d7.LH7 = new SAPLoginData() {
        //        UserName = "21688419",
        //        Password = "1qaz@wsx",
        //        Address = "saplh7.sapnet.hp.com",
        //        Client = "100"
        //    };
        //    d7.Start = DateTime.Now.AddDays(-2);
        //    d7.End = DateTime.Now;
        //    AddTask(d7, ATTTask.AIFMassUpload);

        //}
    }
}
