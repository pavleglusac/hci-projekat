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
        public static List<User> Users { get; set; }
        public static List<Ticket> Tickets { get; set; }
        public static User? CurrentUser { get; set; }

        public static String CurrentTrainName { get; set;}

        public static void loadData()
        {
            CurrentTrainName = "";
            System.Diagnostics.Debug.WriteLine("pocetak ucitavanja");
            Stations = new List<Station>();
            Timetable = new List<Timetable>();
            Trains = new List<Train>();
            Users = new List<User>();
            Tickets = new List<Ticket>();
            Stations.Add(new Station("Novi Sad", new Location(45.246813, 19.853059)));
            Stations.Add(new Station("Ečka", new Location(45.315630, 20.442566)));
            Stations.Add(new Station("Borča", new Location(44.880790, 20.465300)));
            Stations.Add(new Station("Pančevo", new Location(43.880790, 21.465300)));
            Stations.Add(new Station("Beograd", new Location(43.880790, 19.465300)));

            System.Diagnostics.Debug.WriteLine("ucitao stanice");
            Train train1 = new();
            train1.Name = "Soko";
            AddRowsToTrain(train1, RowEnum.ALL, 2);
            AddRowsToTrain(train1, RowEnum.TOP, 2);
            train1.SetSeatLabels();
            train1.PricePerMinute = 10;

            Train train2 = new();
            train2.Name = "Orao";
            AddRowsToTrain(train2, RowEnum.ALL, 2);
            AddRowsToTrain(train2, RowEnum.TOP, 2);
            train2.SetSeatLabels();
            train2.PricePerMinute = 11;

            Train train3 = new();
            train3.Name = "Jastreb";
            AddRowsToTrain(train3, RowEnum.ALL, 2);
            AddRowsToTrain(train3, RowEnum.TOP, 2);
            train3.PricePerMinute = 12;

            train1.Stations.Add(Stations[0], 1);
            train1.Stations.Add(Stations[1], 2);
            train1.Stations.Add(Stations[2], 3);
            train1.Stations.Add(Stations[3], 4);
            train1.Stations.Add(Stations[4], 5);

            train2.Stations.Add(Stations[0], 1);
            train2.Stations.Add(Stations[2], 2);
            train2.Stations.Add(Stations[1], 3);

            train3.Stations.Add(Stations[1], 1);
            train3.Stations.Add(Stations[2], 2);

            train3.SetSeatLabels();

            Trains.Add(train1);
            Trains.Add(train2);
            Trains.Add(train3);

            Random random = new();

            // let's call these the original departures
            Trains.ForEach(x =>
            {
                List<Departure> departures = new();
                for (int i = 0; i < 5; i++)
                {
                    TimeOnly start = TimeOnly.Parse("00:00:00").AddHours(i + 5).AddMinutes((i % 2) * 15);
                    TimeOnly end = start.Add(TimeSpan.FromMinutes(30));
                    Station first = x.GetFirstStation(); Station last = x.GetLastStation();
                    departures.Add(new Departure(start, end, first, last));
                }
                x.Timetable = new Timetable();
                x.Timetable.Departures = departures;
            });


            User customer1 = new("Marko", "Markovic", "test", "test", UserType.CUSTOMER);
            User customer2 = new("Nikola", "Nikolic", "test2", "test", UserType.CUSTOMER);
            User manager1 = new("Danica", "Daničić", "adhd", "test", UserType.MANAGER);
            Users.Add(customer1);
            Users.Add(customer2);
            Users.Add(manager1);

            System.Diagnostics.Debug.WriteLine("ucitao korisnike");

            GenerateTestTickets();

            System.Diagnostics.Debug.WriteLine("kraj ucitavanja");
        }

        internal static bool CantBeDeleted(Train train)
        {
            return Tickets.Any(x => TicketAfterNow(x) && x.Train.Name == train.Name);
        }

        internal static bool TicketAfterNow(Ticket x)
        {
            if (x.DepartureDate == DateOnly.FromDateTime(DateTime.Now) && x.Departure.DepartureDateTime > TimeOnly.FromDateTime(DateTime.Now)) return true;
            else if (x.DepartureDate > DateOnly.FromDateTime(DateTime.Now)) return true;
            else return false;
        }

        public static Tuple<double, double> getTicketNumberAndIncomeForTrain(int month, string year, Train train)
        {
            double x = 0;
            double y = 0;
            foreach (Ticket t in Tickets)
            {
                if (t.Train.Name.Equals(train.Name) && t.DepartureDate.Month == month && (String.IsNullOrEmpty(year) || t.DepartureDate.Year == Int32.Parse(year))){
                    x++;
                    y += t.Price;
                }
            }
            Tuple<double, double> tuple = new(x, y);
            return tuple;
        }

        public static Tuple<int, double> GetTicketNumberAndIncomeForMissingDepartures(Train train, List<Departure> departures, DateOnly departureDate)
        {
            List<Ticket> missing = Tickets.Where(x => x.Train.Name == train.Name && !departures.Any(y => CompareDepartures(y, departureDate, x.Departure, x.DepartureDate))).ToList();
            return new(missing.Count, missing.Sum(x => x.Price));
        }

        internal static Tuple<int, double, List<Ticket>> GetTicketNumberAndIncomeForDeparture(Train train, Departure departure, DateOnly departureDate)
        {
            List<Ticket> matching = Tickets.Where(x => CompareDepartures(x.Departure, x.DepartureDate, departure, departureDate) && x.Train.Name.Equals(train.Name))
                .ToList();
            return new(matching.Count, matching.Sum(x => x.Price), matching);
        }

        public static Boolean CompareDepartures(Departure departure1, DateOnly departureDate1, Departure departure2, DateOnly departureDate2)
        {
            if(departureDate1 == departureDate2 && departure1.DepartureDateTime == departure2.DepartureDateTime && departure1.ArrivalDateTime == departure2.ArrivalDateTime)
            {
                return true;
            }
            return false;
        }

        internal static List<Departure> GetMissingDepartures(Train train, DateOnly departureDate)
        {
            var departures = train.Timetable.Departures;
            List<Departure> missing = Tickets.Where(x => x.DepartureDate == departureDate && x.Train.Name == train.Name)
                .Where(x => !departures.Any(y => CompareDepartureTimes(y, x.Departure)))
                .Select(x => x.Departure).ToList();
            return missing;
            
        }

        public static bool CompareDepartureTimes(Departure x, Departure y)
        {
            if(x.DepartureDateTime == y.DepartureDateTime && x.ArrivalDateTime == y.ArrivalDateTime)
            {
                return true;
            }
            return false;
        }

        public static void RecalculateTicketTime(Train train)
        {
            int totalStations = train.Stations.Count;
            System.Diagnostics.Debug.WriteLine($"voz ima {train.Stations.Count} stanica");
            train.Timetable.Departures.ForEach(originalDeparture =>
            {
                Dictionary<Station, TimeOnly> stationArrival = new Dictionary<Station, TimeOnly>();
                TimeOnly totalStart = originalDeparture.DepartureDateTime;
                TimeOnly totalEnd = originalDeparture.ArrivalDateTime;
                TimeSpan span = totalEnd - totalStart;

                int spanInMinutes = span.Days * 24 * 60 + span.Hours * 60 + span.Minutes;
                double timeSpanBetweenStations = totalStations > 1 ? spanInMinutes * 1.0 / (totalStations - 1) : 0;

                List<Station> stations = train.Stations.Keys.ToList();

                stations.Sort(delegate (Station x, Station y)
                {
                    return train.Stations[x].CompareTo(train.Stations[y]);
                });



                for(int i = 0; i < train.Stations.Count; i++)
                {
                    var from = stations[i];

                    int numFromFirstStation = train.Stations[from] - 1;

                    TimeOnly start = totalStart.Add(TimeSpan.FromMinutes(numFromFirstStation * timeSpanBetweenStations));

                    if(!stationArrival.ContainsKey(from))
                        stationArrival.Add(from, start);
                }


                Tickets
                    .Where(x => x.Train.Name == train.Name)
                    .Where(x => BetweenOrEquals(x.Departure.DepartureDateTime,originalDeparture.DepartureDateTime, originalDeparture.ArrivalDateTime))
                    .Where(x => BetweenOrEquals(x.Departure.ArrivalDateTime, originalDeparture.DepartureDateTime, originalDeparture.ArrivalDateTime))
                    .ToList()
                    .ForEach(x =>
                    {
                        var from = x.Departure.From;
                        var to = x.Departure.To;
                        if(train.Stations.ContainsKey(from) && train.Stations.ContainsKey(to))
                        {
                            Departure dep = new Departure();
                            dep.From = from;
                            dep.To = to;
                            dep.DepartureDateTime = stationArrival[from];
                            dep.ArrivalDateTime = stationArrival[to];
                            x.Departure = dep;
                            System.Diagnostics.Debug.WriteLine($"RECALCULATED {x.Departure.From} {x.Departure.To} {x.Departure.ArrivalDateTime} {x.Departure.DepartureDateTime}");
                        }
                    }
               );
            });

        }

        internal static void RemoveDanglingTickets()
        {
            HashSet<Ticket> ticketsToRemove = new HashSet<Ticket>();
            Tickets.ForEach(x =>
            {
                bool has = false;
                foreach (var dep in x.Train.Timetable.Departures)
                {
                    if (BetweenOrEquals(x.Departure.DepartureDateTime, dep.DepartureDateTime, dep.ArrivalDateTime) &&
                        BetweenOrEquals(x.Departure.ArrivalDateTime, dep.DepartureDateTime, dep.ArrivalDateTime)
                        && x.Train.Stations.ContainsKey(x.Departure.From) && x.Train.Stations.ContainsKey(x.Departure.To))
                    {
                        has = true;
                        break;
                    }
                }
                if (!has)
                {
                    ticketsToRemove.Add(x);
                }

            });
            Tickets.RemoveAll(x => ticketsToRemove.Contains(x));
        }

        public static bool BetweenOrEquals(TimeOnly goal, TimeOnly start, TimeOnly end)
        {
            if (goal.IsBetween(start, end) || goal == start || goal == end) return true;
            return false;
        }

        private static void GenerateTestTickets()
        {
            Random random = new();

            var randomTest = new Random();


            Trains.ForEach(x =>
            {
                DateTime startDate = x.CreationDate.ToDateTime(TimeOnly.Parse("00:00:00"));
                TimeSpan timeSpan = DateTime.Now.Subtract(startDate);
                TimeSpan newSpan = new TimeSpan(0, randomTest.Next(0, (int)timeSpan.TotalMinutes), 0);
                DateTime newDate = startDate + newSpan;

                TimeSpan timeSpanFuture = DateTime.Now.AddDays(2).Subtract(startDate);
                TimeSpan newSpanFuture = new TimeSpan(0, randomTest.Next(0, (int)timeSpanFuture.TotalMinutes), 0);
                DateTime newDateFuture = DateTime.Now + newSpan;

                for (int i = 0; i < 10; i++)
                {
                    Tickets.Add(new Ticket(x, x.Timetable.Departures[i % x.Timetable.Departures.Count], Users[random.Next(0, 2)], new Seat(), DateOnly.FromDateTime(newDate)));
                }

                Tickets.Add(new Ticket(x, x.Timetable.Departures[random.Next(0, x.Timetable.Departures.Count)], Users[random.Next(0, 2)], new Seat(), DateOnly.FromDateTime(newDateFuture)));


            });

            Tickets.ForEach(x => System.Diagnostics.Debug.WriteLine($"{x}"));
        }



        public static Timetable GetTimetableForTrainName(string name)
        {
            return Trains.Where(x => x.Name == name).First().Timetable;
        }

        public static List<String> getTrainsNamesWithStation(Location location)
        {
            List<String> trainNames = new List<String>();
            foreach (var train in Trains)
            {
                foreach (var station in train.Stations)
                {
                    if (station.Key.Location.Equals(location))
                    {
                        trainNames.Add(train.Name);
                        break;
                    }
                }
            }
            return trainNames;
        }
        public static List<String> getTrainsNamesWithStation(Location location,String name)
        {
            List<String> trainNames = new List<String>();
            foreach (var train in Trains)
            {
                foreach (var station in train.Stations)
                {
                    if (station.Key.Location.Equals(location) && !train.Name.Equals(name))
                    {
                        trainNames.Add(train.Name);
                        break;
                    }
                }
            }
            return trainNames;
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

        internal static List<Train> FilterTrains(string departureStation, string destinationStation)
        {
            return Trains.FindAll(train => TrainContainsStationName(train, departureStation) && TrainContainsStationName(train, destinationStation) &&
                train.Stations[getStationByName(departureStation)] < train.Stations[getStationByName(destinationStation)]
            );
        }

        internal static List<Train> FilterTrainsEmpty(string departureStation, string destinationStation)
        {
            return Trains.FindAll(train =>
                {
                    int num1 = -1;
                    int num2 = -1;
                    if (departureStation != null)
                    {
                        if(TrainContainsStationName(train, departureStation))
                            num1 = train.Stations[getStationByName(departureStation)];
                        else
                            return false;
                    }
                    if(destinationStation != null)
                    {
                        if (TrainContainsStationName(train, destinationStation))
                            num2 = train.Stations[getStationByName(destinationStation)];
                        else
                            return false;

                    }
                    if ((num1 == -1 && num2 != -1) || (num2 == -1 && num1 != -1) || (num1 == num2 && num1 == -1)) return true;
                    if (num1 < num2) return true;
                    return false;
                }
            );
        }

        internal static Boolean TrainContainsStationName(Train train, string name)
        {
            if (name == null || name.Length == 0) return false;
            return train.Stations.ContainsKey(getStationByName(name));

        }

        public static List<Train> SearchTrainsByName(string name)
        {
            return Trains.Where(x => x.Name.ToLower().StartsWith(name.ToLower())).ToList();
        }

        public static Train GetTrainByName(string name)
        {
            return Trains.Where(x => x.Name == name).First();
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
                if (s.Location.Equals(station.Location))
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
                if (station.Location.Equals(location))
                {
                    return station;
                }
            }
            return addStation(new Station(location));
        }

        public static Station getStationByName(string name)
        {
            foreach (Station station in Stations)
            {
                if (station.Name == name)
                {
                    return station;
                }
            }
            return null;
        }
        public static void setName(Location location, String name)
        {
            foreach (Station station in Stations)
            {
                if (station.Location.Equals(location))
                {
                    station.Name = name;
                    return;
                }
            }
        }

        public static void removeStation(Location location)
        {
            List<Station> removeStation = new List<Station>();
            foreach (Station station in Stations)
            {
                if (station.Location.Equals(location))
                {
                    removeStation.Add(station);
                    break;
                }
            }
            foreach (Train t in Trains)
            {
                t.tryRemoveStation(removeStation.First());
            }
            Stations.Remove(removeStation.First());

        }

        internal static Seat? GetSeatFromTrain(Train train, string seatLabel)
        {
            foreach(Row row in train.LeftRows.Concat(train.RightRows))
            {
                IEnumerable<Seat> seats = row.Seats.Where(x => x.Label == seatLabel);
                if (seats.Count() == 0) continue;
                return seats.First();
            }
            return null;
        }

        public static void UpdateTrain(Train train, Train ct)
        {
            Train oldTrain = Trains.Where(x => x.Name == train.Name).First();
            if (oldTrain == null) return;
            CopyTrain(oldTrain, ct);
        }

        public static void CopyTrain(Train train, Train copy)
        {
            train.Name = copy.Name;
            train.LeftRows = copy.LeftRows;
            train.RightRows = copy.RightRows;
            train.Stations = copy.Stations;
            train.Timetable = copy.Timetable;
        }

        public static List<Train> GetUpdatedTrains(List<Train> oldTrains)
        {
            List<Train> newList = new List<Train>();
            oldTrains.ForEach(
                x =>
                {
                    newList.Add(Trains.Where(y => Object.ReferenceEquals(x, y)).First());
                }
            );
            return newList;
        }

        public static List<Ticket> GetCurrentUsersTickets()
        {
            return Tickets.FindAll(x => x.Owner == CurrentUser);
        }

        public static void AddTickets(List<Ticket> tickets)
        {
            tickets.ForEach(ticket => Tickets.Add(ticket));
        }

        public static User? GetUser(string username, string password)
        {
            return Users.FirstOrDefault(x => x.Username == username && x.Password == password);
        }

        public static bool IsExistingUsername(string username)
        {
            return Users.Any(x => x.Username == username);
        }

        public static void SaveUser(User user)
        {
            Users.Add(user);
        }

        public static void SetCurrentUser(User user)
        {
            CurrentUser = user;
        }

        public static void ClearCurrentUser()
        {
            CurrentUser = null;
        }

    }
}
