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
        public Location location { get; set; }

        public Station(Location location)
        {
            this.Name = "Location";
            this.location = location;
        }

        public Station(String name)
        {
            Name = name;
            this.location = new Location();
        }
    }
}

