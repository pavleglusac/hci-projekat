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
    }
}
