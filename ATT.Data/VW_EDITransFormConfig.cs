using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATT.Data
{
    public class VW_EDITransFormConfig
    {
        public int ProAwsysId { get; set; }

        public int IDocTypeId { get; set; }

        public string Node { get; set; }

        public string XPath { get; set; }

        public string FromVal { get; set; }

        public string ToVal { get; set; }
    }
}
