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
    public class ATTUpload:ScriptBase<ATTUploadData>
    {
        private ATTDbContext _db;

        public ATTUpload() {
            _db = new ATTDbContext();
        }

        [Step(Id =0,Name ="Get Message From SAP")]
        public void Step1() {

            var interfaces = _db.SAPInterfaces.Where(s => s.IsSelected).ToList();
            
            foreach(var i in interfaces) {
                ScriptEngine<MSGIDTask, MSGIDTaskData> script = new ScriptEngine<MSGIDTask, MSGIDTaskData>();
                script.Run(new MSGIDTaskData());

                ScriptEngine<Payloads, PayloadsData> script1 = new ScriptEngine<Payloads, PayloadsData>();
                PayloadsData d = new PayloadsData();
                d.DownloadData = _data.DownloadData;
                d.UpdateData = _data.UpdateData;
                d.UploadData = _data.UploadData;
                //script1.Run();
            }



            
            

        }

       
    }
}
