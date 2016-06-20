using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace AIF.Client.Converters
{
    class SelectConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            if (value == null)
                return false;
            var val = int.Parse(value.ToString());
            if (val < 0)
                return false;
            return true;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }
    }
}
