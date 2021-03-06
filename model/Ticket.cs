using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCIProjekat.model
{
    public class Ticket
    {
        public User Owner { get; set; }
        public Train Train { get; set; }
        public Departure Departure { get; set; }
        public DateOnly DepartureDate { get; set; }
        public double Price { get; set; }
        public Seat Seat { get; set; }
        public Ticket(Train train, Departure departure, User owner, Seat seat, DateOnly date)
        {
            Train = train;
            Departure = departure;
            Owner = owner;
            Seat = seat;
            DepartureDate = date;
            Price = departure.GetTripTimeInMinutes() * train.PricePerMinute;
        }

        public override string ToString()
        {
            return $"{Train.Name} {DepartureDate} {Departure.DepartureDateTime} {Departure.ArrivalDateTime} {Price} {Seat}";
        }
    }
}
