using Microsoft.Maps.MapControl.WPF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCIProjekat.model
{
    internal class Database
    {
        public static List<Station> stations { get; set; }
        public static List<Timetable> timetable { get; set; }
        public static List<Train> train { get; set; }

        public static void loadData()
        {
            stations = new List<Station>();
            timetable = new List<Timetable>();
            train = new List<Train>();
            stations.Add(new Station(new Location(45.246813, 19.853059)));
            stations.Add(new Station(new Location(46.246813, 19)));
            stations.Add(new Station(new Location(44.246813, 18)));
        }

        public static Station addStation(Station station)
        {
            stations.Add(station);
            foreach (Station s in stations)
            {
                if (s.location.Equals(station.location))
                {
                    return s;
                }
            }
            return station;
        }

        public static Station getOrAddStation(Location location)
        {
            foreach(Station station in stations)
            {
                if (station.location.Equals(location))
                {
                    return station;
                }
            }
            return addStation(new Station(location));

        }

        public static void removeStation(Location location)
        {
            List<Station> removeStation = new List<Station>();
            foreach (Station station in stations)
            {
                if (station.location.Equals(location))
                {
                    removeStation.Add(station);
                    break;
                }
            }
            stations.Remove(removeStation.First());
            foreach(Train t in train)
            {
                t.tryRemoveStation(removeStation.First());
            }

        }
    }
}
