using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Young.Data.Attributes;

namespace SharedLib.Converters
{
    public class MyDateConverterAttribute : DataConverterMethodAttribute
    {
        private static IDataConverter _dateConverter;
        static MyDateConverterAttribute() {
            _dateConverter = new MyDateConverter();
        }
        public override IDataConverter GetConverter() {
            return _dateConverter;
        }
    }
}
