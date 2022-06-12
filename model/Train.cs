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
        public DateOnly CreationDate { get; set; }
        public double PricePerMinute { get; set; }
        public bool Deleted { get; set; }

        public Train()
        {
            LeftRows = new List<Row>();
            RightRows = new List<Row>();
            Stations = new Dictionary<Station,int>();
            CreationDate = DateOnly.FromDateTime(DateTime.Now.AddDays(-3));
        }

        public Train(string name, Dictionary<Station, int> stations, List<Departure> timetable, double pricePerMinute)
        {
            this.Name = name;
            this.LeftRows = new List<Row>();
            this.RightRows = new List<Row>();
            this.Stations = stations;
            this.Timetable = new Timetable { Departures = timetable };
            this.PricePerMinute = pricePerMinute;
            this.Deleted = false;
        }
        public Train(string name, Dictionary<Station, int> stations, double pricePerMinute)
        {
            this.Name = name;
            this.LeftRows = new List<Row>();
            this.RightRows = new List<Row>();
            this.Timetable = new Timetable { Departures = new List<Departure>() };
            this.PricePerMinute = pricePerMinute;
            this.Deleted = false;
        }

        public Train(string name, List<Departure> timetable, double pricePerMinute)
        {
            this.Name = name;
            this.LeftRows = new List<Row>();
            this.RightRows = new List<Row>();
            this.Timetable = new Timetable { Departures = timetable };
            this.PricePerMinute = pricePerMinute;
            this.Deleted = false;
        }

        internal List<Station> GetCriticalStations(Station from, Station to)
        {
            List<Station> CriticalStations = new List<Station>();
            int fromOrder = Stations[from];
            int toOrder = Stations[to];
            foreach (Station station in this.Stations.Keys)
            {
                if(Stations[station] > fromOrder && Stations[station] < toOrder)
                {
                    CriticalStations.Add(station);
                }
            }
            return CriticalStations;
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

        public Station GetFirstStation()
        {
            return Stations.Where(k => k.Value == 1).Select(k => k.Key).First();
        }

        public Station GetLastStation()
        {
            return Stations.Where(k => k.Value == Stations.Count).Select(k => k.Key).First();
        }

        public Station GetStationByIndex(int i)
        {
            return Stations.Where(k => k.Value == i).Select(k => k.Key).First();
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
                if (s.Location.Equals(station.Location))
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
                foreach (Station s in Stations.Keys)
                {
                    if (i > idRemoved)
                        Stations[s] = Stations[s] - 1;
                    i++;
                }
            }
        }

        internal int getStationNumber(Location location)
        {
            foreach (Station s in Stations.Keys)
            {
                if (s.Location.Equals(location))
                {
                    return Stations[s];
                }
            }
            return -1;
        }

        public void SetSeatLabels()
        {
            
            int lefts = 0;
            int rights = 0;
            int maxLeft = 0;
            int seatOrder;
            foreach (var row in LeftRows)
            {
                seatOrder = 0;
                lefts++;
                foreach (var rowSeat in row.Seats)
                {
                    rowSeat.Label = $"{(char)('A' + seatOrder++)}-{lefts}-L";
                }
                maxLeft = Math.Max(maxLeft, seatOrder);
            }

            foreach (var row in RightRows)
            {
                seatOrder = 0;
                rights++;
                foreach (var rowSeat in row.Seats)
                {
                    rowSeat.Label = $"{(char)('A' + maxLeft + seatOrder++)}-{rights}-R";
                }
            }
        }

        public void PrintSeatLabels()
        {
            foreach (var row in LeftRows)
            {
                foreach (var seat in row.Seats)
                    System.Diagnostics.Debug.WriteLine($"{seat.Label} ");
            }
            System.Diagnostics.Debug.WriteLine("-------------------");
            foreach (var row in RightRows)
            {
                foreach (var seat in row.Seats)
                    System.Diagnostics.Debug.WriteLine($"{seat.Label} ");
            }
        }

        public override bool Equals(object? obj)
        {
            if (obj == null) return false;
            Train other;
            try { other = (Train)obj; } catch(Exception ex) { return false; }
            if (other.Name != Name) return false;
            if (LeftRows.Count() != other.LeftRows.Count()) return false;
            if (RightRows.Count() != other.RightRows.Count()) return false;
            for(int i = 0; i < LeftRows.Count(); i++)
            {
                if (LeftRows[i].RowType != other.LeftRows[i].RowType) return false;
                if (LeftRows[i].Seats.Count() != other.LeftRows[i].Seats.Count()) return false;
            }

            for (int i = 0; i < RightRows.Count(); i++)
            {
                if (RightRows[i].RowType != other.RightRows[i].RowType) return false;
                if (RightRows[i].Seats.Count() != other.RightRows[i].Seats.Count()) return false;
            }

            return true;
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
