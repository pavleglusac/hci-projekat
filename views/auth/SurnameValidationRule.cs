using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace HCIProjekat.views.auth
{
    public class SurnameValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            try
            {
                if (value == null || ((string)value).Length == 0) return new ValidationResult(false, "Polje ne može biti prazno!");
                if (((string)value).Trim().Length != ((string)value).Length) return new ValidationResult(false, "Nedozvoljeni razmaci na početku ili kraju!");
                if (!IsValid((string)value)) return new ValidationResult(false, "Prezime sadrži nedozvoljene karaktere!");
                if (((string)value).Length > 32) return new ValidationResult(false, "Prezime ne sme biti duže od 32 karaktera!");
            }
            catch (Exception ex)
            {
                return new ValidationResult(false, "Prezime nije validno!");
            }

            return new ValidationResult(true, "Prezime je validno");
        }

        public static bool IsValid(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return false;

            try
            {
                Match match = Regex.Match(name, @"^[\p{L}\s]+$");
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
