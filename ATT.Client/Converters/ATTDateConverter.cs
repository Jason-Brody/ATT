using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using ATT.Scripts;
namespace ATT.Client.Converters
{
    class ATTDateConverter :IValueConverter
    {
        //public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture) {
        //    throw new NotImplementedException();
        //}

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            var d = value as ATTDate;
            if (d != null) {
                return new DateTime(d.Year, d.Month, d.Day, d.Hour, 0, 0).ToString();
            }
            return null;

        }

        //public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) {
        //    throw new NotImplementedException();
        //}

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            DateTime dt = (DateTime)value;
            return new ATTDate(dt.Year, dt.Month, dt.Day, dt.Hour);

        }
    }
}
