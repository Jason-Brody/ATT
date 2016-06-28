using SharedLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ATT.Scripts
{
    public class ScheduleData 
    {

        public DateTime Start { get; set; } = DateTime.Now.AddDays(-1);

        public DateTime ExpireDate { get; set; } = DateTime.Now;

        public DateTime GetEnd() {
            return Start.AddHours(Interval);
        }

        public void GetNext() {
            Start = Start.AddHours(Interval);
        }
        
        //[XmlIgnore]
        //public ATTDate Start { get; set; }

        //[XmlIgnore]
        //public ATTDate ExpireDate{ get; set; }

        //public DateTime GetStart() {
        //    if (Start != null) {
        //        return new DateTime(Start.Year, Start.Month, Start.Day, Start.Hour, 0, 0);
        //    }
        //    throw new ArgumentNullException("No Date Set");
        //}

        //public DateTime GetEnd() {
        //    return GetStart().AddHours(Interval);
        //}

        //public void SetNextDate() {
        //    DateTime dt = GetStart();
            
        //}
        
        [XmlIgnore]
        public int Interval { get; set; } = 1;

        
    }
}
