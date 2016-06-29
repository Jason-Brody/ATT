using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATT.Scripts
{
    public class PayloadsUpdateData:PayloadsShareData
    {
        public string XPath_AWSYS { get; } = "//AWSYS";

        public string XPath_OBJ_SYS { get; } = "//OBJ_SYS";

        public string XPath_OTC_rec_type { get; } = "//OTC_rec_type";

        public string UnzipLog { get; } = "Unzip the file";

        public string AnalysisLog { get; } = "Analysis the XML file";

        public string UploadLog { get; } = "Upload the doc info to DB";

        public string GetXPathConfigLog { get; } = "Get XPath Config from DB";

        public string UpdateTransformConfigLog { get; } = "Update Transform Config";

        public string TransformLog { get; } = "Transform the XML File";

        public PayloadsUpdateData Copy() {
            return new PayloadsUpdateData();
        }
    }
}
