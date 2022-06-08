using HCIProjekat.model;
using HCIProjekat.views.customer.dialogs;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Globalization;
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

namespace HCIProjekat.views.customer
{
    /// <summary>
    /// Interaction logic for Timetable.xaml
    /// </summary>
    public partial class Timetable : Page
    {
        List<string> Locations;
        List<TimetableEntry> DepartureEntries = new List<TimetableEntry>();

        public bool IsDialogOpen = true;
        public Frame DialogContent = new Frame();

        List<Train> Trains = new List<Train>();
        public Timetable()
        {
            InitializeComponent();
            Locations = new List<string>(){
                "Novi Sad",
                "Beograd",
                "Zrenjanin",
                "Subotica",
                "Sombor",
                "Kikinda",
                "Pančevo"
            };
            GetTrains();
            ShowData();
            startComboBox.ItemsSource = Locations;
            destinationComboBox.ItemsSource = Locations;
        }

        private void GetTrains()
        {
            Trains = Database.Trains;
        }

        private void ShowData()
        {
            DepartureEntries = new List<TimetableEntry>();
            Trains.ForEach(train =>
            {
                train.Timetable.Departures.ForEach(departure =>
                {
                    DepartureEntries.Add(new(train, departure, DateOnly.FromDateTime(DateTime.Now)));
                });
            });
            departuresGrid.ItemsSource = DepartureEntries;
        }

        private void handleFilterClick(object sender, RoutedEventArgs e)
        {
            DepartureEntries = new List<TimetableEntry>();
            List<Departure> Departures = new List<Departure>();
            Dictionary<Station, int> Stations = new Dictionary<Station, int>();
            Stations.Add(new Station("Novi Sad"), 0);
            Stations.Add(new Station("Zrenjanin"), 1);
            Stations.Add(new Station("Beograd"), 2);
            Stations.Add(new Station("Subotica"), 3);
            Train train = new Train("Soko X", Stations, 10);
            for (int i = 0; i < 10; i++)
            {
                Departures.Add(new Departure(TimeOnly.FromDateTime(DateTime.Parse($"2022-06-01T0{i%5}:0{(i * 23) % 10}")), 
                    TimeOnly.FromDateTime(DateTime.Parse($"2022-06-01T0{i%3+5}:0{(i* 27) % 10}")), Stations.Keys.ToList()[i%3], Stations.Keys.ToList()[(i+1)% 3]));
            }

            train.Timetable = new model.Timetable { Departures = Departures };
            train.Timetable.Departures.ForEach(x => DepartureEntries.Add(new TimetableEntry(train, x, DateOnly.FromDateTime(DateTime.Now))));
            departuresGrid.ItemsSource = DepartureEntries;
        }

        private void buyTicketButtonClick(object sender, RoutedEventArgs e)
        {
            TimetableEntry timetableEntry = (TimetableEntry)((Button)e.Source).DataContext;
            System.Diagnostics.Debug.WriteLine(timetableEntry.Departure.DepartureDateTime);

            
            DialogContent.Content = new SeatChooser(timetableEntry.Train, timetableEntry.Departure, timetableEntry.DepartureDate);
            DialogContent.Height = 600;
            DialogContent.Width = 800;
            IsDialogOpen = true;
            BuyTicketDialogHost.DialogContent = DialogContent;
            BuyTicketDialogHost.CloseOnClickAway = true;
            BuyTicketDialogHost.ShowDialog(DialogContent);
        }


        private class TimetableEntry
        {
            public Train Train { get; set; }
            public Departure Departure { get; set; }
            public DateOnly DepartureDate { get; set; }
            public TimetableEntry(Train train, Departure departure, DateOnly date)
            {
                Train = train;
                Departure = departure;
                DepartureDate = date;
            }
        }
    }
}
