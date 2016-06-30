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
        [XmlIgnore]
        public int Mid { get; set; }

        public ScheduleData() {
            DateTime now = DateTime.Now;
            DateTime y = now.AddDays(-1);
            _start = new DateTime(y.Year, y.Month, y.Day, y.Hour, 0, 0);
            _expire = new DateTime(now.Year, now.Month, now.Day, now.Hour, 0, 0);
        }

        private DateTime _start;

        private DateTime _expire;


        [XmlIgnore]
        public DateTime Start {
            get { return _start; }
            set { _start = value; }
        }


        [XmlIgnore]
        public DateTime ExpireDate {
            get { return _expire; }
            set { _expire = value; }
        }

        public DateTime GetEnd() {
            return Start.AddHours(Interval);
        }


        public string End {
            get {
                DateTime dt = Start.AddHours(Interval);
                if(dt.Hour == 0) {
                    return "24:00:00";
                }
                return dt.ToString("HH:mm:ss");
            }
        }


        public void GetNext() {
            Start = Start.AddHours(Interval);
        }

        public void GetPrevious() {
            Start = Start.AddHours(Interval * -1);
        }

        public virtual ScheduleData Copy() {
            return new ScheduleData() {
                Start = this.Start,
                Interval = this.Interval,
                ExpireDate = this.ExpireDate
            };
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
