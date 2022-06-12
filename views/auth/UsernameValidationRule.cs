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
    public class UsernameValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            try
            {
                if (value == null || ((string)value).Length == 0) return new ValidationResult(false, "Polje ne može biti prazno!");
                if (((string)value).Trim().Length != ((string)value).Length) return new ValidationResult(false, "Nedozvoljeni razmaci na početku ili kraju!");
                if (!IsValid((string)value)) return new ValidationResult(false, "Korisničko ime sadrži nedozvoljene karaktere!");
                if (((string)value).Length > 24) return new ValidationResult(false, "Korisničko ime ne sme biti duže od 24 karaktera!");
            }
            catch (Exception ex)
            {
                return new ValidationResult(false, "Korisničko ime nije validno!");
            }

            return new ValidationResult(true, "Korisničko ime je validno");
        }

        public static bool IsValid(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return false;

            try
            {
                Match match = Regex.Match(name, @"^[A-Za-z0-9_\.]+$");
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
