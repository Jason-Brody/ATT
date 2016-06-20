using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Young.Data.Attributes;

namespace SharedLib.Converters
{
    public class TimeConverterAttribute : DataConverterMethodAttribute
    {
        private static IDataConverter _timeConverter;

        static TimeConverterAttribute() {
            _timeConverter = new TimeConverter();
        }

        public override IDataConverter GetConverter() {
            return _timeConverter;
        }
    }
}
