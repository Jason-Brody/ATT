using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Young.Data.Attributes;

namespace SharedLib.Converters
{
    class TimeStampConverterAttribute : DataConverterMethodAttribute
    {
        private static IDataConverter _dataConverter;

       static TimeStampConverterAttribute() {
            _dataConverter = new TimeStampConverter();
        }

        public override IDataConverter GetConverter() {
            return _dataConverter;
        }
    }

    class TimeStampConverter : IDataConverter
    {
        public object Convert(object data) {
            var datetime = data.ToString();
            var y = int.Parse(datetime.Substring(0, 4));
            var m = int.Parse(datetime.Substring(4, 2));
            var d = int.Parse(datetime.Substring(6, 2));
            var h = int.Parse(datetime.Substring(8, 2));
            var mm = int.Parse(datetime.Substring(10, 2));
            var ss = int.Parse(datetime.Substring(12, 2));
            return new DateTime(y, m, d, h, mm, ss);
        }
    }
}
