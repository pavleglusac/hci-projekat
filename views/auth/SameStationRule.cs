using HCIProjekat.model;
using HCIProjekat.views.manager.dialogs;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace HCIProjekat.views.auth
{
    internal class SameStationRule : ValidationRule
    {
        public SameStationRule() { }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            Boolean exists = false;
            foreach(Station station in Database.Stations)
            {
                if (station.Name.Equals(value) && !station.Name.Equals(StationName.currentStationName))
                {
                    exists = true;
                    break;
                }
            }
            if (exists)
            {
                return new ValidationResult(false, "Stanica sa datim imenom već postoji.");
            }
            return ValidationResult.ValidResult;
        }
    }
}
