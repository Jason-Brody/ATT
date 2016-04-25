using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATT.Scripts
{
    public class MSGID_ReportData: GUIShareData
    {
        public string WorkFolder { get; } = Path.Combine(GlobalConfig.WorkDir, "MSGID_Report");
       
    }
}
