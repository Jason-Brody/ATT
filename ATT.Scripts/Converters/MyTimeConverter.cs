using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Young.Data.Attributes;

namespace ATT.Scripts.Converters
{
    public class MyTimeConverter : IDataConverter
    {
        public object Convert(object data) {
            var time = data.ToString();
            var h = int.Parse(time.Substring(0, 2));
            var m = int.Parse(time.Substring(2, 2));
            var s = int.Parse(time.Substring(4, 2));
            return new TimeSpan(h, m, s);
        }
    }
}
