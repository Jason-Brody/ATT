using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Young.Data.Attributes;

namespace ATT.Data
{
    public class MyDateConverterAttribute : DataConverterMethodAttribute
    {
        private static IDataConverter _dateConverter;
        static MyDateConverterAttribute()
        {
            _dateConverter = new MyDateConverter();
        }
        public override IDataConverter GetConverter()
        {
            return _dateConverter;
        }
    }

    public class MyDateConverter : IDataConverter
    {
        public object Convert(object data)
        {

            var val = data.ToString();
            var y = int.Parse(val.Substring(0, 4));
            var m = int.Parse(val.Substring(4, 2));
            var d = int.Parse(val.Substring(6, 2));
            return new DateTime(y, m, d);
        }
    }

    public class MyTimeConverterAttribute : DataConverterMethodAttribute
    {
        private static IDataConverter _timeConverter;

        static MyTimeConverterAttribute()
        {
            _timeConverter = new MyTimeConverter();
        }
        public override IDataConverter GetConverter()
        {
            return _timeConverter;
        }
    }

    public class MyTimeConverter : IDataConverter
    {
        public object Convert(object data)
        {
            var time = data.ToString();
            var h = int.Parse(time.Substring(0, 2));
            var m = int.Parse(time.Substring(2, 2));
            var s = int.Parse(time.Substring(4, 2));
            return new TimeSpan(h, m, s);
        }
    }
}
