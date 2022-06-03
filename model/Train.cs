using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCIProjekat.model
{
    internal class Train
    {
        public String name { get; set; }
        public List<Station> stations { get; set; }
        public Timetable timetable { get; set; }

        public Train(string name, List<Station> stations, Timetable timetable)
        {
            this.name = name;
            this.stations = stations;
            this.timetable = timetable;
        }

        public Train()
        {
        }

        public Train(string name, Timetable timetable)
        {
            this.name = name;
            this.timetable = timetable;
        }
    }
}
