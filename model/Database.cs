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
        public static List<Station> Stations { get; set; }
        public static List<Timetable> Timetable { get; set; }
        public static List<Train> Trains { get; set; }

        public static void loadData()
        {
            System.Diagnostics.Debug.WriteLine("pocetak ucitavanja");
            Stations = new List<Station>();
            Timetable = new List<Timetable>();
            Trains = new List<Train>();
            Stations.Add(new Station(new Location(45.246813, 19.853059)));
            Stations.Add(new Station(new Location(46.246813, 19)));
            Stations.Add(new Station(new Location(44.246813, 18)));

            System.Diagnostics.Debug.WriteLine("ucitao stanice");
            Train train1 = new Train();
            train1.Name = "Soko";
            AddRowsToTrain(train1, RowEnum.ALL, 2);
            AddRowsToTrain(train1, RowEnum.TOP, 2);

            Train train2 = new Train();
            train2.Name = "Orao";
            AddRowsToTrain(train2, RowEnum.ALL, 2);
            AddRowsToTrain(train2, RowEnum.TOP, 2);

            Train train3 = new Train();
            train3.Name = "Jastreb";
            AddRowsToTrain(train2, RowEnum.ALL, 2);
            AddRowsToTrain(train2, RowEnum.TOP, 2);

            Trains.Add(train1);
            Trains.Add(train2);
            Trains.Add(train3);

            System.Diagnostics.Debug.WriteLine("kraj ucitavanja");
        }

        public static void AddRowsToTrain(Train train, RowEnum type, int numOfRows)
        {
            for (int i = 0; i < numOfRows; i++)
            {
                Row row = new Row();
                row.RowType = type;
                for(int j = 0; j < 4; j++)
                {
                    Seat seat = new Seat();
                    seat.Label = "";
                    row.Seats.Add(seat);
                }
                train.LeftRows.Add(row);
            }
        }

        public static List<Train> SearchTrainsByName(string name)
        {
            return Trains.Where(x => x.Name.ToLower().StartsWith(name.ToLower())).ToList();
        }

        public static IEnumerable<string> getTrainNames()
        {
            List<string> trainNames = new List<string>();
            foreach (Train train in Trains)
            {
                trainNames.Add(train.Name);
            }
            return trainNames;
        }

        public static bool trainNameExists(string trainName)
        {
            foreach (Train train in Trains)
            {
                if (train.Name == trainName)
                {
                    return true;
                }
            }
            return false;

        }

        public static Station addStation(Station station)
        {
            Stations.Add(station);
            foreach (Station s in Stations)
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
            foreach (Station station in Stations)
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
            foreach (Station station in Stations)
            {
                if (station.location.Equals(location))
                {
                    removeStation.Add(station);
                    break;
                }
            }
            Stations.Remove(removeStation.First());
            foreach (Train t in Trains)
            {
                t.tryRemoveStation(removeStation.First());
            }

        }
    }
}
