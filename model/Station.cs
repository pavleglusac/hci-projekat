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
        public Location location { get; set; }

        public Station(Location location)
        {
            this.location = location;
        }

        public Station()
        {
        }
    }
}

