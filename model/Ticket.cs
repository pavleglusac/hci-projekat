using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCIProjekat.model
{
    internal class Ticket
    {
        public Train Train { get; set; }
        public Departure Departure { get; set; }
        public double Price { get; set; }

        public Ticket(Train train, Departure departure)
        {
            Train = train;
            Departure = departure;
            Price = departure.GetTripTimeInMinutes() * train.PricePerMinute;
        }
    }
}
