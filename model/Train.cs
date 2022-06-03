using Microsoft.Maps.MapControl.WPF;
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
        public Dictionary<Station,int> stations { get; set; }
        public Timetable timetable { get; set; }

        public Train(string name, Dictionary<Station, int> stations, Timetable timetable)
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

        internal void updateStations(Dictionary<Station, int> trainsStations)
        {
            stations.Clear();
            foreach (KeyValuePair<Station,int> station in trainsStations)
            {
                stations.Add(station.Key,station.Value);
            }
        }
        internal void tryRemoveStation(Station station)
        {
            List<Station> stationsToRemove = new List<Station>();
            int idRemoved = -1, i = 0;
            foreach (Station s in stations.Keys)
            {
                if (s.location.Equals(station.location))
                {
                    stationsToRemove.Add(s);
                    idRemoved = i;
                    break;
                }
                i++;
            }
            if (stationsToRemove.Count > 0)
            {
                stations.Remove(stationsToRemove.First());
            }
            foreach (Station s in stations.Keys)
            {
                if (i > idRemoved)
                    stations[s] = stations[s] - 1;
                i++;
            }
        }

        internal int getStationNumber(Location location)
        {
            foreach(Station s in stations.Keys)
            {
                if (s.location.Equals(location))
                {
                    return stations[s];
                }
            }
            return -1;
        }
    }
}
