using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ScriptRunner.Interface;
using ScriptRunner.Interface.Attributes;
using ATT.Data;

namespace ATT.Scripts
{
    [Script("ATT Upload")]
    public class ATTUpload : ScriptBase<ATTUploadData>
    {
        private ATTDbContext _db;

        public ATTUpload() {
            _db = new ATTDbContext();
        }

        [Step(Id =1,Name ="Run")]
        public void Run() {

            var interfaces = _db.SAPInterfaces.Where(s => s.IsSelected==true).ToList();

            var mission = new Missions() {
                StartDt = _data.Start,
                StartHour = _data.Start.Hour
            };

            _db.Missions.Add(mission);

            _db.SaveChanges();

            _data.MessageData.Mid = mission.Id;

         

            foreach (var i in interfaces) {
                _data.MessageData.SAPInterface = i;
                _data.MessageData.Start = _data.Start;
                _data.MessageData.ExpireDate = _data.ExpireDate;
                _data.MessageData.Interval = _data.Interval;
                ScriptEngine<MSGIDTask, MSGIDTaskData> sapScript = new ScriptEngine<MSGIDTask, MSGIDTaskData>();
                sapScript.Run(_data.MessageData);
            }

            ScriptEngine<Payloads, PayloadsData> payloadsScript = new ScriptEngine<Payloads, PayloadsData>();
            PayloadsData d = new PayloadsData();
            d.DownloadData = _data.DownloadData;
            d.UpdateData = _data.UpdateData;
            d.UploadData = _data.UploadData;
            payloadsScript.Run(d);




        }


    }
}
