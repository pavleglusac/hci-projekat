using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCIProjekat.model
{
    internal class Departure
    {
        public string TrainName { get; set; }
        public DateTime DepartureDateTime { get; set; }
        public DateTime ArrivalDateTime { get; set; }
        public int HourDuration { get; set; }
        public int MinuteDuration { get; set; }
        public double Price { get; set; }

        public Departure(string trainName, DateTime departureDateTime, DateTime arrivalDateTime, int hourDuration, int minuteDuration, double price)
        {
            TrainName = trainName;
            DepartureDateTime = departureDateTime;
            ArrivalDateTime = arrivalDateTime;
            HourDuration = hourDuration;
            MinuteDuration = minuteDuration;
            Price = price;
        }

        public string GetTripTime()
        {
            return HourDuration.ToString() + ":" + MinuteDuration.ToString();
        }
    }
}
