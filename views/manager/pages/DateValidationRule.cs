using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Reflection;
using System.Windows.Markup;

namespace HCIProjekat.views.manager.pages
{
 

    public class DateValidationRule : ValidationRule
    {

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            try
            {
                DateTime time;
                if (!DateTime.TryParse((value ?? "").ToString(),
                    CultureInfo.CurrentCulture,
                    DateTimeStyles.AssumeLocal | DateTimeStyles.AllowWhiteSpaces,
                    out time)) return new ValidationResult(false, "Datum nije validan!");

                return time.Date <= DateTime.Now.Date
                ? new ValidationResult(false, "Datum mora biti budući!")
                : ValidationResult.ValidResult;
            }
            catch(InvalidCastException ex)
            {
                return new ValidationResult(false, "Datum nije validan");
            }
            catch (Exception ex)
            {
                return new ValidationResult(false, ex.Message);
            }
        }
    }
}
