using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATT.Scripts
{
    public class PayloadsTransformData:PayloadsShareData
    {
        public string DownloadUrl { get; set; }

        public string XPath_AWSYS { get; } = "//AWSYS";

        public string XPath_OBJ_SYS { get; } = "//OBJ_SYS";

        public string XPath_OTC_rec_type { get; } = "//OTC_rec_type";
    }
}
