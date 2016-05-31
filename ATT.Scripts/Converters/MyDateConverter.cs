using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Young.Data.Attributes;

namespace ATT.Scripts.Converters
{
    public class MyDateConverter : IDataConverter
    {
        public object Convert(object data) {

            var val = data.ToString();
            var y = int.Parse(val.Substring(0, 4));
            var m = int.Parse(val.Substring(4, 2));
            var d = int.Parse(val.Substring(6, 2));
           
            if (y == 0 || m == 0 || d == 0) {
                return new DateTime();
            } else {
                return new DateTime(y, m, d);
            }


        }
    }
}
