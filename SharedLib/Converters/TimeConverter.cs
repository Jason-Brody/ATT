using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Young.Data.Attributes;

namespace SharedLib.Converters
{
    public class TimeConverter : IDataConverter
    {
        public object Convert(object data) {
            if (data == null)
                return null;
            var time = data.ToString();
            if (string.IsNullOrEmpty(time))
                return null;
            var times = time.Split(':');
            var h = int.Parse(times[0]);
            var m = int.Parse(times[1]);
            var s = int.Parse(times[2]);
            return new TimeSpan(h, m, s);
        }
    }
}
