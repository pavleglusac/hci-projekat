using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCIProjekat.model
{
    internal class Ticket
    {
        public User Owner { get; set; }
        public Train Train { get; set; }
        public Departure Departure { get; set; }
        public double Price { get; set; }
        public Seat Seat { get; set; }
        public TicketStatus Status { get; set; }
        public Ticket(Train train, Departure departure, User owner, Seat seat, TicketStatus status)
        {
            Train = train;
            Departure = departure;
            Owner = owner;
            Seat = seat;
            Status = status;
            Price = departure.GetTripTimeInMinutes() * train.PricePerMinute;
        }
    }

    internal enum TicketStatus
    {
        BOUGHT,
        RESERVED
    }
}
