using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Young.Data.Attributes;

namespace ATT.Scripts.Converters
{
    public class MyTimeConverterAttribute : DataConverterMethodAttribute
    {
        private static IDataConverter _timeConverter;

        static MyTimeConverterAttribute() {
            _timeConverter = new MyTimeConverter();
        }
        public override IDataConverter GetConverter() {
            return _timeConverter;
        }
    }
}
