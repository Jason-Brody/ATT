using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATT.Scripts
{
    public class AIFMassUploadData
    {
        public SAPLoginData LH1 { get; set; }

        public SAPLoginData LH7 { get; set; }

        public DateTime Start { get; set; }

        public DateTime End { get; set; }

        public int DataCounts { get; set; } = 100;

        public int IntervalDays { get; set; } = 2;

        public int IntervalCounts { get; set; } = 5;

        public string Status { get; } = "53";

        public string Direction { get; } = "2";


    }
}
