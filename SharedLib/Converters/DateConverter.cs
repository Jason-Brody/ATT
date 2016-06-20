using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Young.Data.Attributes;

namespace SharedLib.Converters
{
    public class DateConverter : IDataConverter
    {
        public object Convert(object data) {
            if (data == null)
                return null;
            var date = data.ToString();
            if (string.IsNullOrEmpty(date))
                return null;
            var dates = date.Split('.');
            var d = int.Parse(dates[0]);
            var m = int.Parse(dates[1]);
            var y = int.Parse(dates[2]);
            return new DateTime(y, m, d);
        }
    }
}
