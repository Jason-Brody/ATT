using SharedLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Young.Data.Attributes;

namespace ATT.Scripts
{
    public class LHTrackData : ScheduleData
    {
        public LHTrackData() {
            LH4 = new SAPLoginData();
        }

        private const string _subFolder = "ITGTrack";

        private const string _filePrefix = "ITGTrack";

        private string _guid;

        public SAPLoginData LH4 { get; set; }

        public void NewGuid() {
            _guid = GlobalConfig.GetGuid();
        }

        public string File {
            get {
                var f = GlobalConfig.GetAttFile(_subFolder, _filePrefix+"_"+_guid, "txt");
                return f;
            }
        }


        public string UserId { get; set; } = "ATT";

        public string IDocStatus { get; set; } = "*";

    }


}
