using Microsoft.Maps.MapControl.WPF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCIProjekat.model
{
    public class Station
    {
        public String Name { get; set; }
        public Location Location { get; set; }

        public Station(String name, Location location)
        {
            this.Name = name;
            this.Location = location;
        }

        public Station(Location location)
        {
            this.Name = "Unnamed Station";
            this.Location = location;
        }

        public Station(String name)
        {
            Name = name;
            this.Location = new Location();
        }
    }
}

