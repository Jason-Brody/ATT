using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Young.Data.Attributes;

namespace ATT.Data
{
    public class MsgIdConverterAttribute : DataConverterMethodAttribute
    {
        private static IDataConverter _dateConverter;
        static MsgIdConverterAttribute()
        {
            _dateConverter = new MsgIdConverter();
        }
        public override IDataConverter GetConverter()
        {
            return _dateConverter;
        }
    }

    public class MsgIdConverter : IDataConverter
    {
        public object Convert(object data)
        {
            return data.ToString().Trim().Split(' ')[1];
        }
    }

   

   
}
