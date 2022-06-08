using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCIProjekat.model
{
    public class Departure
    {
        public TimeOnly DepartureDateTime { get; set; }
        public TimeOnly ArrivalDateTime { get; set; }
        public Station From { get; set; }
        public Station To { get; set; }

        public Departure()
        {

        }

        public Departure(TimeOnly departureDateTime, TimeOnly arrivalDateTime, Station from, Station to)
        {
            DepartureDateTime = departureDateTime;
            ArrivalDateTime = arrivalDateTime;
            From = from;
            To = to;
        }

        public string GetTripTime()
        {
            TimeSpan span = ArrivalDateTime - DepartureDateTime;
            return (span.Hours + 24 * span.Days).ToString() + ":" +
                ((span.Minutes).ToString().Length == 1 ? "0" + (span.Minutes).ToString() : (span.Minutes).ToString()) + " Hrs";
        }

        public int GetTripTimeInMinutes()
        {
            TimeSpan span = ArrivalDateTime - DepartureDateTime;
            return span.Minutes + (span.Hours + 24 * span.Days) * 60;
        }

        public Departure Copy()
        {
            Departure departure = new Departure();
            departure.To = To;
            departure.From = From;
            departure.DepartureDateTime = DepartureDateTime;
            departure.ArrivalDateTime = ArrivalDateTime;
            return departure;
        }
    }
}
