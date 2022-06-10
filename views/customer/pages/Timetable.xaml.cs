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
        List<String> Locations = new();
        List<TimetableEntry> DepartureEntries = new();

        public List<string> DepartureLocations = new();
        public List<string> DestinationLocations = new();

        bool menja = false;

        string departureStation;
        string destinationStation;

        public Timetable()
        {
            InitializeComponent();
            DataContext = this;
            GetLocations();
            ShowLocations();
        }

        private List<Train> GetTrains()
        {
            return Database.FilterTrains(departureStation, destinationStation);
        }

        private void GetLocations()
        {
            Locations = new();
            Database.Stations.ForEach(station => Locations.Add(station.Name));
        }
        private void ShowLocations()
        {
            DepartureLocations.Clear();
            DepartureLocations.AddRange(Locations);
            DestinationLocations.Clear();
            DestinationLocations.AddRange(Locations);
            menja = true;
            departureStationComboBox.Items.Clear();
            destinationStationComboBox.Items.Clear();
            DepartureLocations.ForEach(x => departureStationComboBox.Items.Add(x));
            DestinationLocations.ForEach(x => destinationStationComboBox.Items.Add(x));
            departureStationComboBox.Items.Refresh();
            destinationStationComboBox.Items.Refresh();
            menja = false;
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

        private void ShowData(DateOnly departureDate)
        {
            System.Diagnostics.Debug.WriteLine(departureStation + " " + destinationStation + " " + departureDate);
            DepartureEntries = new List<TimetableEntry>();
            List<Train> trains = GetTrains();

            Random random = new();
            trains.ForEach(train =>
            {
                List<Departure> departures = new List<Departure>();
                int totalStations = train.Stations.Count;
                if (totalStations < 2) return;
                train.Timetable.Departures.ForEach(originalDeparture =>
                {
                    TimeOnly totalStart = originalDeparture.DepartureDateTime;
                    TimeOnly totalEnd = originalDeparture.ArrivalDateTime;
                    TimeSpan span = totalEnd - totalStart;

                    int spanInMinutes = span.Days * 24 * 60 + span.Hours * 60 + span.Minutes;
                    int timeSpanBetweenStations = spanInMinutes / (totalStations - 1);


                    Station from = Database.getStationByName(departureStation);
                    Station to = Database.getStationByName(destinationStation);

                    int spanFromFirstStation = train.Stations[from] - 1;
                    int spanBetweenStations = train.Stations[to] - train.Stations[from];

                    TimeOnly start = totalStart.Add(TimeSpan.FromMinutes(spanFromFirstStation * timeSpanBetweenStations));
                    TimeOnly end = start.Add(TimeSpan.FromMinutes(spanBetweenStations*timeSpanBetweenStations));
                    if (departureDate == DateOnly.FromDateTime(DateTime.Now) && start <= TimeOnly.FromDateTime(DateTime.Now)) return;
                    departures.Add(new Departure(start, end, from, to));
                });
                departures.ForEach(departure => DepartureEntries.Add(new(train, departure, departureDate)));
            });
            departuresGrid.ItemsSource = DepartureEntries;

            if (DepartureEntries.Any()) timetableComponent.Visibility = Visibility.Visible;
            else
            {
                timetableComponent.Visibility = Visibility.Collapsed;
                MessageBox.Show("Nema vožnji sa zadatim parametrima!");
            }
        }

        private void departureStationComboBox_SelectionChanged(object sender, RoutedEventArgs e)
        {
            if(!menja)
            {
                menja = true;

                string destinationStation = (string)destinationStationComboBox.SelectedItem;
                string departureStation = (string)departureStationComboBox.SelectedItem;
                System.Diagnostics.Debug.WriteLine($"{destinationStation} -> {departureStation}");
                departureStationComboBox.Items.Clear();
                destinationStationComboBox.Items.Clear();
                DepartureLocations.ForEach(x => departureStationComboBox.Items.Add(x));
                DestinationLocations.ForEach(x => destinationStationComboBox.Items.Add(x));
                menja = false;

            }
            //SetCorrectIndex();
        }

        private void destinationStationComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!menja)
            {
                menja = true;
                string destinationStation = (string)destinationStationComboBox.SelectedItem;
                string departureStation = (string)departureStationComboBox.SelectedItem;
                System.Diagnostics.Debug.WriteLine($"{destinationStation} -> {departureStation}");
                departureStationComboBox.Items.Clear();
                destinationStationComboBox.Items.Clear();
                DepartureLocations.ForEach(x => departureStationComboBox.Items.Add(x));
                DestinationLocations.ForEach(x => destinationStationComboBox.Items.Add(x));
                menja = false;
            }
            //SetCorrectIndex();
        }

        private void buyTicketButtonClick(object sender, RoutedEventArgs e)
        {
            TimetableEntry timetableEntry = (TimetableEntry)((Button)e.Source).DataContext;
            System.Diagnostics.Debug.WriteLine(timetableEntry.Departure.DepartureDateTime);

            MainWindow mw = new(new SeatChooser(
                timetableEntry.Train,
                timetableEntry.Departure,
                timetableEntry.DepartureDate,
                departureStation,
                destinationStation));
            mw.Height = 600;
            mw.Width = 800;
            mw.ShowDialog();
        }

        public void SwapPlaces(object sender, EventArgs e)
        {
            string departureStation = (string)departureStationComboBox.SelectedItem;
            string destinationStation = (string)destinationStationComboBox.SelectedItem;
            menja = true;

            System.Diagnostics.Debug.WriteLine($"{departureStation} {destinationStation}  *****");

            DepartureLocations = (Locations.Where(x => x != departureStation).ToList());
            DestinationLocations = (Locations.Where(x => x != destinationStation).ToList());


            departureStationComboBox.Items.Clear();
            destinationStationComboBox.Items.Clear();
            DepartureLocations.ForEach(x => departureStationComboBox.Items.Add(x));
            DestinationLocations.ForEach(x => destinationStationComboBox.Items.Add(x));


            int ind = 0, ind2 = 0;
            ind = DepartureLocations.IndexOf(destinationStation);
            ind2 = DestinationLocations.IndexOf(departureStation);



            departureStationComboBox.Items.Refresh();
            destinationStationComboBox.Items.Refresh();


            departureStationComboBox.SelectedIndex = ind;

            destinationStationComboBox.SelectedIndex = ind2;
            
            menja = false;
            //destinationStationComboBox_SelectionChanged(null, null);
        }

        public void SetCorrectIndex()
        {
            string departureStation = (string)departureStationComboBox.SelectedItem;
            string destinationStation = (string)destinationStationComboBox.SelectedItem;
            int ind = departureStationComboBox.Items.IndexOf(departureStation);
            departureStationComboBox.SelectedIndex = ind;
            departureStationComboBox.SelectedItem=departureStation;
            ind = destinationStationComboBox.Items.IndexOf(destinationStation);
            destinationStationComboBox.SelectedIndex = ind;
            destinationStationComboBox.SelectedItem = destinationStation;
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
