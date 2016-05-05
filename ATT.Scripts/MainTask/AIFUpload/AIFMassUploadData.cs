using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ATT.Scripts
{
    public class AIFMassUploadData
    {
        public AIFMassUploadData() {
            _guid = Guid.NewGuid().ToString();
        }

        private string _guid;

        private string getfile(string prefix) {
            return prefix + $"_{TaskId}_{_guid}.txt";
           // return prefix + $"_{AIFTask.Interfaces.MsgType}_{AIFTask.Interfaces.MsgFunction}_{AIFTask.Interfaces.MsgCode}_{AIFTask.Interfaces.PartnerNo}_{_guid}.txt";
        }

        private void removeFile(string file) {
            if (File.Exists(file))
                File.Delete(file);
        }

        public string GetUploadedFile() {
            string file = Path.Combine(GlobalConfig.AIFWorkDir, getfile("AIFUploaded"));
            return file;
        }

        public string GetIDocFile() {
            string file = Path.Combine(GlobalConfig.AIFWorkDir, getfile("AIFIDoc"));
            return file;
        }

        public string GetDownloadFile() {
            return Path.Combine(GlobalConfig.AIFWorkDir,getfile("AIFDownload"));
        }

        public string GetIDocITGFile() {
            return Path.Combine(GlobalConfig.AIFWorkDir, getfile("AIFIDoc_ITG"));
        }

        public SAPLoginData LH1 { get; set; }

        public SAPLoginData LH7 { get; set; }

        public DateTime Start { get; set; }

        public DateTime End { get; set; }

        public int DataCounts { get; set; } = 100;

        public int IntervalDays { get; set; } = 2;

        public int RetryCounts { get; set; } = 5;

        public string Status { get; } = "53";

        public string Direction { get; } = "2";

        [XmlIgnore]
        public int TaskId { get; set; }

        //[XmlIgnore]
        //public ATT.Data.AIF.Tasks AIFTask { get; set; }
        //[XmlIgnore]
        //public string MsgType { get; set; }

        //[XmlIgnore]
        //public string MsgCode { get; set; }

        //[XmlIgnore]
        //public string MsgFunction { get; set; }

        //[XmlIgnore]
        //public string PartnerNo { get; set; }


    }
}
