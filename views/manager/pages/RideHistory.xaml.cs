using HCIProjekat.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace HCIProjekat.views.manager.pages
{
    /// <summary>
    /// Interaction logic for RideHistory.xaml
    /// </summary>
    public partial class RideHistory : Page
    {
        List<string> Locations;
        List<RideHistoryEntry> RideHistoryData = new List<RideHistoryEntry>();

        string departureStation;
        string destinationStation;

        public RideHistory()
        {
            InitializeComponent();
            AutoComplete();
            GetLocations();
            ShowLocations();
            //FillRideHistoryData();
        }

        private List<Train> GetTrains()
        {
            return Database.FilterTrainsEmpty(departureStation, destinationStation);
        }

        private void GetLocations()
        {
            Locations = new();
            Database.Stations.ForEach(station => Locations.Add(station.Name));
        }
        private void ShowLocations()
        {
            departureStationComboBox.ItemsSource = Locations;
            destinationStationComboBox.ItemsSource = Locations;
        }

        private void handleFilterClick(object sender, RoutedEventArgs e)
        {
            departureStation = (string)departureStationComboBox.SelectedValue;
            destinationStation = (string)destinationStationComboBox.SelectedValue;
            if (departureDatePicker.SelectedDate == null)
            {
                System.Windows.MessageBox.Show(
                    "Datum ne može biti prazan.",
                    "Greška", System.Windows.MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                DateOnly departureDate = DateOnly.FromDateTime((DateTime)departureDatePicker.SelectedDate);
                ShowData(departureDate);
            }
        }

        public void ClearClick(object sender, EventArgs e)
        {
            var result = MessageBox.Show(
                    "Da li ste sigurni da želite da očistite parametre?",
                    "Potvrda", System.Windows.MessageBoxButton.YesNo, MessageBoxImage.Question);
            if(result == MessageBoxResult.Yes)
            {
                departureStationComboBox.SelectedIndex = -1;
                departureStationComboBox.Items.Refresh();
                destinationStationComboBox.SelectedIndex = -1;
                destinationStationComboBox.Items.Refresh();
                TrainSearchInput.SelectedIndex = -1;
                departureDatePicker.Text = "";
            }
            else
            {

            }
        }

        private void ShowData(DateOnly departureDate)
        {
            List<Train> trains = GetTrains();
            System.Diagnostics.Debug.WriteLine($"{departureStation} -> {destinationStation}");
            System.Diagnostics.Debug.WriteLine($"{trains} {trains.Count}");
            RideHistoryData = new List<RideHistoryEntry>();
            Random random = new();
            trains.ForEach(train =>
            {
                if (TrainSearchInput.Text != null && TrainSearchInput.Text.Length > 0)
                {
                    if (train.Name != TrainSearchInput.Text) return;
                }
                List<Departure> departures = new List<Departure>();
                int totalStations = train.Stations.Count;
                if (totalStations < 2) return;
                train.Timetable.Departures.ForEach(originalDeparture =>
                {
                    TimeOnly totalStart = originalDeparture.DepartureDateTime;
                    TimeOnly totalEnd = originalDeparture.ArrivalDateTime;
                    TimeSpan span = totalEnd - totalStart;

                    int spanInMinutes = span.Days * 24 * 60 + span.Hours * 60 + span.Minutes;
                    double timeSpanBetweenStations = spanInMinutes*1.0 / (totalStations - 1);

                    Station from = departureStation == null || departureStation.Length == 0 ? train.GetFirstStation() : Database.getStationByName(departureStation);
                    Station to = destinationStation == null || destinationStation.Length == 0 ? train.GetLastStation() : Database.getStationByName(destinationStation);

                    int spanFromFirstStation = train.Stations[from] - 1;
                    int spanBetweenStations = train.Stations[to] - train.Stations[from];

                    TimeOnly start = totalStart.Add(TimeSpan.FromMinutes(spanFromFirstStation * timeSpanBetweenStations));
                    TimeOnly end = start.Add(TimeSpan.FromMinutes(spanBetweenStations * timeSpanBetweenStations));
                    if (start == end) return;
                    departures.Add(new Departure(start, end, from, to));
                });

                departures.ForEach(departure =>
                    {
                        Tuple<int, double> res = Database.GetTicketNumberAndIncomeForDeparture(train, departure, departureDate);
                        RideHistoryData.Add(new(train, departure, res.Item1, res.Item2));
                    }
                );
            });
            departuresGrid.ItemsSource = RideHistoryData;
            departuresGrid.Items.Refresh();

            if (RideHistoryData.Any()) timetableComponent.Visibility = Visibility.Visible;
            else timetableComponent.Visibility = Visibility.Collapsed;
        }

        private void departureStationComboBox_SelectionChanged(object sender, RoutedEventArgs e)
        {
            string departureStation = (string)departureStationComboBox.SelectedValue;
            destinationStationComboBox.ItemsSource = Locations.FindAll(location => location != departureStation);
        }

        private void destinationStationComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string destinationStation = (string)destinationStationComboBox.SelectedValue;
            departureStationComboBox.ItemsSource = Locations.FindAll(location => location != destinationStation);
        }


        private void FillRideHistoryData()
        {
            List<Departure> Departures = new List<Departure>();
            Dictionary<Station, int> Stations = new Dictionary<Station, int>();
            Stations.Add(new Station("Novi Sad"), 0);
            Stations.Add(new Station("Zrenjanin"), 1);
            Stations.Add(new Station("Beograd"), 2);
            Stations.Add(new Station("Subotica"), 3);
            Train train = new Train("Soko X", Stations, 10);
            for (int i = 0; i < 10; i++)
            {
                Departures.Add(new Departure(TimeOnly.FromDateTime(DateTime.Parse($"2022-06-01T0{i % 5}:0{(i * 23) % 10}")),
                    TimeOnly.FromDateTime(DateTime.Parse($"2022-06-01T0{i % 3 + 5}:0{(i * 27) % 10}")), Stations.Keys.ToList()[i % 3], Stations.Keys.ToList()[(i + 1) % 3]));
                int rnd = new Random().Next(20, 40);
                RideHistoryData.Add(
                    new RideHistoryEntry
                    {
                        Train = train,
                        Departure = Departures.Last(),
                        Tickets = rnd,
                        Income = rnd * 280.0
                    }
                );
            }

            departuresGrid.ItemsSource = RideHistoryData;
        }

        private void AutoComplete()
        {
            TrainSearchInput.ItemsSource = Database.Trains.Select(x => {
                ComboBoxItem? cbi = new ComboBoxItem
                {
                    Content = x.Name
                }; return cbi;
            }).ToList();
        }

        private class RideHistoryEntry
        {
            public Train Train { get; set; }
            public Departure Departure { get; set; }
            public int Tickets { get; set; }
            public double Income { get; set; }

            public RideHistoryEntry(Train train, Departure departure, int tickets, double income)
            {
                Train = train;
                Departure = departure;
                Tickets = tickets;
                Income = income;
            }
            public RideHistoryEntry()
            {

            }
        }
    }
}
