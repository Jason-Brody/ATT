using SharedLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATT.Scripts
{
    public class PIITrackData: ScheduleData
    {
        public PIITrackData() {
            SAPAccount = new SAPLoginData();
        }

        public string WorkFolder { get; } = Path.Combine(GlobalConfig.AttWorkDir, "MSGID_Report");

        public SAPLoginData SAPAccount { get; set; }
       
    }
}
