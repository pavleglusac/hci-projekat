using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCIProjekat.model
{
    public class Departure
    {
        public DateTime DepartureDateTime { get; set; }
        public DateTime ArrivalDateTime { get; set; }

        public Departure(DateTime departureDateTime, DateTime arrivalDateTime)
        {
            DepartureDateTime = departureDateTime;
            ArrivalDateTime = arrivalDateTime;
        }

        public string GetTripTime()
        {
            TimeSpan span = ArrivalDateTime.Subtract(DepartureDateTime);
            return (span.Hours + 24 * span.Days).ToString() + ":" +
                ((span.Minutes).ToString().Length == 1 ? "0" + (span.Minutes).ToString() : (span.Minutes).ToString()) + " Hrs";
        }

        public int GetTripTimeInMinutes()
        {
            TimeSpan span = ArrivalDateTime.Subtract(DepartureDateTime);
            return span.Minutes + (span.Hours + 24 * span.Days) * 60;
        }
    }
}
