using SharedLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIF.Scripts
{
    public class AIFErrorTrackData:AIFBaseData
    {
        private string _folder;

        public AIFErrorTrackData() : this(@"C:\AIF") {

        }

        public AIFErrorTrackData(string folder) {
            GlobalConfig.CreateDirectory(folder);
            this._folder = folder;
        }

        public string FileName { get; set; } = "ErrorMsg.txt";

        public string FilePath {
            get { return Path.Combine(_folder, FileName); }
        }

    }
}
