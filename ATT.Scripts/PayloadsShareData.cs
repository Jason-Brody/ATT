using SharedLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATT.Scripts
{ 
    public class PayloadsShareData
    {
      

        public int TaskId { get; set; }
        public string TaskFolder {
            get { return GlobalConfig.CreateDirectory(Path.Combine(GlobalConfig.AttWorkDir, "Payloads",TaskId.ToString())); }
        }

        public string WorkFolder {
            get { return GlobalConfig.CreateDirectory(Path.Combine(GlobalConfig.AttWorkDir, "Payloads")); }
        }

       


        

        public string SourceFolder {
            get {
                return GlobalConfig.CreateDirectory(Path.Combine(TaskFolder, "Source"));
            }
        }

        public string TargetFolder {
            get {
                return GlobalConfig.CreateDirectory(Path.Combine(TaskFolder, "Target"));
            }
        }

        public string SentFolder {
            get {
                return GlobalConfig.CreateDirectory(Path.Combine(TaskFolder, "Sent"));
            }
        }



    }
}
