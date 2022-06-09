using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCIProjekat.model
{
    public class Timetable
    {
        public List<Departure> Departures { get; set; }
        
        public Timetable()
        {
            Departures = new List<Departure>();
        }

        public override string ToString()
        {
            string s = "";
            foreach (Departure departure in Departures)
            {
                s += $"{departure.DepartureDateTime} - {departure.ArrivalDateTime}  |";
            }
            return s;
        }

        public override bool Equals(object? obj)
        {
            if (obj == null) return false;
            try { _ = (Timetable)obj; } catch { return false; }
            var other = (Timetable)obj;
            if(other.Departures.Count != Departures.Count) return false;
            foreach(var otherDeparture in other.Departures)
            {
                foreach(var departure in Departures )
                {
                    if (otherDeparture.DepartureDateTime != departure.DepartureDateTime) return false;
                    if (otherDeparture.ArrivalDateTime != departure.ArrivalDateTime) return false;
                    if (otherDeparture.To != departure.To && otherDeparture.From != departure.From) return false;
                }
            }
            return true;
        }

        public Timetable Copy()
        {
            Timetable copy = new Timetable();
            copy.Departures = Departures.Select(x => x.Copy()).ToList();
            return copy;
        }
    }
}
