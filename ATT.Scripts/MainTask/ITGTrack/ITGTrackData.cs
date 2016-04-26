using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Young.Data.Attributes;

namespace ATT.Scripts
{
    public class ITGTrackData : GUIShareData
    {
        private const string _subFolder = "ITGTrack";

        private const string _filePrefix = "ITGTrack";

        private string _guid;

        public void NewGuid() {
            _guid = GlobalConfig.GetGuid();
        }

        public string File {
            get {
                var f = GlobalConfig.GetFile(_subFolder, _filePrefix+"_"+_guid, "txt");
                return f;
            }
        }


        public string UserId { get; set; } = "ATT";

    }


}
