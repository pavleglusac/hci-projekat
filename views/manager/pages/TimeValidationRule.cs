using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace HCIProjekat.views.manager.pages
{
    public class TimeValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            try
            {
                System.Diagnostics.Debug.WriteLine($"ovo je value {value}");
                System.Diagnostics.Debug.WriteLine($"{value == null}");
                if (value == null || ((string)value).Length == 0) return new ValidationResult(true, "Vreme je validno");
                TimeOnly orderDate = TimeOnly.Parse((string)value);
            }
            catch(Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"{ex.Message} {value}");
                return new ValidationResult(false, "Vreme nije validno!");
            }

            return new ValidationResult(true, "Vreme je validno");
        }
    }

}
