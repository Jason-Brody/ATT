using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Young.Data.Attributes;

namespace ATT.Scripts.Converters
{
    public class DateConverterAttribute : DataConverterMethodAttribute
    {
        private static IDataConverter _dateConverter;

        static DateConverterAttribute() {
            _dateConverter = new DateConverter();
        }

        public override IDataConverter GetConverter() {
            return _dateConverter;
        }
    }
}
