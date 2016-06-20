using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Young.Data.Attributes;

namespace SharedLib.Converters
{
    public class MsgIdConverter : IDataConverter
    {
        public object Convert(object data) {
            if (data.ToString().Trim() == "")
                return "";
            return data.ToString().Trim().Split(' ')[1];
        }
    }
}
