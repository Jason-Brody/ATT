using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Young.Data.Attributes;

namespace ATT.Data
{
    public class EDIKeyConverterAttribute : DataConverterMethodAttribute
    {
        private static IDataConverter _dateConverter;
        static EDIKeyConverterAttribute()
        {
            _dateConverter = new EDIKeyConverter();
        }
        public override IDataConverter GetConverter()
        {
            return _dateConverter;
        }
    }

    public class EDIKeyConverter : IDataConverter
    {
        public object Convert(object data)
        {
            return data.ToString().Trim().Split(' ')[1];
        }
    }

   

   
}
