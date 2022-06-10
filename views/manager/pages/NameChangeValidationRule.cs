using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace HCIProjekat.views.manager.pages
{
    public class NameChangeValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            try
            {
                if (value == null || ((string)value).Length == 0) return new ValidationResult(false, "Ime ne može biti prazno!");
                if (((string)value).Trim().Length != ((string)value).Length) return new ValidationResult(false, "Nedozvoljeni razmaci na početku ili kraju imena!");
                if (!IsValid((string)value)) return new ValidationResult(false, "Ime nije validno!");
            }
            catch (Exception ex)
            {
                return new ValidationResult(false, "Ime nije validno!");
            }

            return new ValidationResult(true, "Ime je validno");
        }

        public static bool IsValid(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return false;

            try
            {
                Match match = Regex.Match(name, @"^[\w\s^_]+$");
                if (!match.Success) return false;
            }
            catch (ArgumentException)
            {
                return false;
            }

            return true;
        }
    }
}
