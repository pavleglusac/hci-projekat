using HCIProjekat.model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace HCIProjekat.views.auth
{
    internal class SameTrainRule : ValidationRule
    {
        public SameTrainRule() { }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            Boolean exists = false;
            foreach (Train train in Database.Trains)
            {
                if (train.Name.Equals(value) && !train.Name.Equals(Database.CurrentTrainName))
                {
                    exists = true;
                    break;
                }
            }
            if (exists)
            {
                return new ValidationResult(false, "Voz sa datim imenom već postoji.");
            }
            return ValidationResult.ValidResult;
        }
    }
}
