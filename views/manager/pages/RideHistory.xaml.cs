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

        public RideHistory()
        {
            InitializeComponent();
            AutoComplete();
            FillRideHistoryData();
            Locations = new List<string>(){
                "Novi Sad",
                "Beograd",
                "Zrenjanin",
                "Subotica",
                "Sombor",
                "Kikinda",
                "Pančevo"
            };
            startComboBox.ItemsSource = Locations;
            destinationComboBox.ItemsSource = Locations;
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
                Departures.Add(new Departure(DateTime.Parse($"2022-06-01T0{i % 5}:0{(i * 23) % 10}"),
                    DateTime.Parse($"2022-06-01T0{i % 3 + 5}:0{(i * 27) % 10}"), Stations.Keys.ToList()[i % 3], Stations.Keys.ToList()[(i + 1) % 3]));
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
