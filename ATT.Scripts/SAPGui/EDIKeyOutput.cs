using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATT.Scripts.SAPGui
{
    public class EDIKeyOutput : FileConfig
    {
        private string guid;

        public EDIKeyOutput() : base("EDIKeyFiles")
        { 
        }

        public void NewInterfaceId(string Interface)
        {
            guid = Interface + "_"+ Guid.NewGuid().ToString();
        }

        public string IDocReportFile
        {
            get { return getFile($"IDocReport_{guid}.txt"); }
        }

        public string EDIKeyFile
        {
            get { return getFile($"EDIArchiveKey_{guid}.txt"); }
        }
    }
}
