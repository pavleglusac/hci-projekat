using System;
using Microsoft.Maps.MapControl.WPF;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCIProjekat.model
{
    public class Train
    {
        public String Name { get; set; }
        public List<Row> LeftRows { get; set; }
        public List<Row> RightRows { get; set; }
        public Dictionary<Station, int> Stations { get; set; }
        public Timetable Timetable { get; set; }

        public Train()
        {
            LeftRows = new List<Row>();
            RightRows = new List<Row>();
        }

        public Train(string name, Dictionary<Station, int> stations, Timetable timetable)
        {
            this.Name = name;
            this.Stations = stations;
            this.Timetable = timetable;
        }


        public Train(string name, Timetable timetable)
        {
            this.Name = name;
            this.Timetable = timetable;
        }

        public override string ToString()
        {
            string s = "LEFT\n";
            foreach(Row row in LeftRows)
            {
                s += $"{row.RowType} -> {row.Seats.Count()}\n";
            }
            s += "RIGHT\n";
            foreach (Row row in RightRows)
            {
                s += $"{row.RowType} -> {row.Seats.Count()}\n";
            }
            return s;
        }


        internal void updateStations(Dictionary<Station, int> trainsStations)
        {
            Stations.Clear();
            foreach (KeyValuePair<Station, int> station in trainsStations)
            {
                Stations.Add(station.Key, station.Value);
            }
        }
        internal void tryRemoveStation(Station station)
        {
            List<Station> stationsToRemove = new List<Station>();
            int idRemoved = -1, i = 0;
            foreach (Station s in Stations.Keys)
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
                Stations.Remove(stationsToRemove.First());
            }
            foreach (Station s in Stations.Keys)
            {
                if (i > idRemoved)
                    Stations[s] = Stations[s] - 1;
                i++;
            }
        }

        internal int getStationNumber(Location location)
        {
            foreach (Station s in Stations.Keys)
            {
                if (s.location.Equals(location))
                {
                    return Stations[s];
                }
            }
            return -1;
        }
    }

    public enum RowEnum
    {
        ALL,
        TOP,
        BOTTOM
    }

    public class Seat
    {
        public String Label { get; set; }
    }

    public class Row
    {
        public List<Seat> Seats { get; set; }
        public RowEnum RowType;
        public Row()
        {
            Seats = new List<Seat>();
        }
    }
}
