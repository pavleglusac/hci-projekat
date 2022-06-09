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
    public partial class Report : Page
    {
        List<string> Locations;
        List<RideHistoryEntry> RideHistoryData = new List<RideHistoryEntry>();
        Dictionary<String, int> monthDict = new Dictionary<string, int>();

        public Report()
        {
            InitializeComponent();
            AutoComplete();
            GetLocations();
            monthDict.Add("Januar", 1);
            monthDict.Add("Februar", 2);
            monthDict.Add("Mart", 3);
            monthDict.Add("April", 4);
            monthDict.Add("Maj", 5);
            monthDict.Add("Jun", 6);
            monthDict.Add("Jul", 7);
            monthDict.Add("Avgust", 8);
            monthDict.Add("Septembar", 9);
            monthDict.Add("Oktobar", 10);
            monthDict.Add("Novembar", 11);
            monthDict.Add("Decembar", 12);
            //FillRideHistoryData();
        }

        private List<Train> GetTrains()
        {
            return Database.Trains;
        }

        private void GetLocations()
        {
            Locations = new();
            Database.Stations.ForEach(station => Locations.Add(station.Name));
        }
        private void handleFilterClick(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(TrainSearchInput.Text))
            {
                System.Windows.MessageBox.Show(
                    "Mesec ne može biti prazan.",
                    "Greška", System.Windows.MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                ShowData();
            }
        }

        private void ShowData()
        {
            List<Train> trains = GetTrains();
            RideHistoryData = new List<RideHistoryEntry>();
            int ticketSum = 0;
            double incomeSum = 0;
            Random random = new();
            trains.ForEach(train =>
            {
                int tickets = 0;
                int income = 0;
                List<Departure> departures = new List<Departure>();
                int totalStations = train.Stations.Count;
                if (totalStations < 2) return;
                Tuple<double,double> tuple = Database.getTicketNumberAndIncomeForTrain(monthDict[TrainSearchInput.Text], TrainSearchInput2.Text, train);
                ticketSum += (int)tuple.Item1;
                incomeSum += tuple.Item2;
                RideHistoryData.Add(new RideHistoryEntry(train, ((int)tuple.Item1), tuple.Item2));

            });
            departuresGrid.ItemsSource = RideHistoryData;
            departuresGrid.Items.Refresh();

            soldTickets.Header = "Prodatih karata (" + ticketSum + " kom)";
            income.Header = "Zarada (" +  incomeSum + " rsd)";
            if (RideHistoryData.Any()) timetableComponent.Visibility = Visibility.Visible;
            else timetableComponent.Visibility = Visibility.Collapsed;
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
                        Tickets = rnd,
                        Income = rnd * 280.0
                    }
                );
            }

            departuresGrid.ItemsSource = RideHistoryData;
        }

        private void AutoComplete()
        {
            TrainSearchInput.ItemsSource = new List<String> { "Januar", "Februar", "Mart", "April", "Maj", "Jun", "Jul", "Avgust", "Septembar", "Oktobar", "Novembar", "Decembar" };
            TrainSearchInput2.ItemsSource = new List<String> { "2019", "2020", "2021", "2022", "2023"};
        }

        private class RideHistoryEntry
        {
            public Train Train { get; set; }
            public int Tickets { get; set; }
            public double Income { get; set; }

            public RideHistoryEntry(Train train, int tickets, double income)
            {
                Train = train;
                Tickets = tickets;
                Income = income;
            }
            public RideHistoryEntry()
            {

            }
        }
    }
}
