using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace HCIProjekat.views.auth
{
    internal class NonEmptyRule : ValidationRule
    {
        public NonEmptyRule() { }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (value == null || ((string)value).Length == 0)
            {
                return new ValidationResult(false, "Obavezno polje.");
            }
            return ValidationResult.ValidResult;
        }
    }
}
