using SharedLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace AIF.Scripts
{
    public abstract class AIFBaseData
    {
        [XmlIgnore]
        public int MissionId { get; set; }

        public SAPLoginData LH7 { get; set; }
}
}
