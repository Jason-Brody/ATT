using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATT.Scripts
{
    public class ATTDate
    {
        public ATTDate() {

        }

        public ATTDate(int Year,int Month,int Day) : this(Year, Month, Day, 0) {

        }

        public ATTDate(int Year,int Month,int Day,int Hour) {
            this.Year = Year;
            this.Month = Month;
            this.Day = Day;
            this.Hour = Hour;
        }

        public int Year { get; set; }

        public int Month { get; set; }

        public int Day { get; set; }

        public int Hour { get; set; }

        public ATTDate GetNext(int Hour) {
            DateTime dt = new DateTime(Year, Month, Day, Hour, 0, 0);
            dt.AddHours(Hour);
            return new ATTDate(dt.Year, dt.Month, dt.Day, dt.Hour);
        }

        public DateTime Current {
            get { return new DateTime(Year,Month,Day,Hour,0,0); }
        }

        
    }
}
