using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace HCIProjekat.converters
{
    public class DateTimeFormatConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return "";

            if (value.GetType() == typeof(DateTime))
            {
                return ((DateTime)value).ToString("dd.MM.yyyy. HH:mm");
            }
            else if(value.GetType() == typeof(DateOnly))
            {
                return ((DateOnly)value).ToString("dd.MM.yyyy.");
            }
            else if (value.GetType() == typeof(TimeOnly))
            {
                return ((TimeOnly)value).ToString("HH:mm");
            }
            return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
