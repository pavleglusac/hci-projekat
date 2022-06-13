using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace HCIProjekat.converters
{
    public class TimeConcatConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameters, CultureInfo culture)
        {
            TimeOnly departure = (TimeOnly)values[0];
            TimeOnly arrival = (TimeOnly)values[1];
            TimeSpan span = arrival - (departure);
            return (span.Hours + 24*span.Days).ToString() + ":" +
                ((span.Minutes).ToString().Length == 1 ? "0" +(span.Minutes).ToString() : (span.Minutes).ToString()) + " Hrs";
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
