using HCIProjekat.model;
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
            startComboBox.ItemsSource = Locations;
            destinationComboBox.ItemsSource = Locations;
        }

        private void handleFilterClick(object sender, RoutedEventArgs e)
        {
            DepartureEntries = new List<TimetableEntry>();
            List<Departure> Departures = new List<Departure>();
            for (int i = 0; i < 10; i++)
            {
                Departures.Add(new Departure(DateTime.Parse($"2022-06-01T0{i%5}:0{(i * 23) % 10}"), 
                    DateTime.Parse($"2022-06-01T0{i%3+5}:0{(i* 27) % 10}")));
            }
            Train train = new Train("Soko X", Departures, 10);
            train.Timetable.ForEach(x => DepartureEntries.Add(new TimetableEntry(train, x)));
            departuresGrid.ItemsSource = DepartureEntries;
        }

        private void buyTicketButtonClick(object sender, RoutedEventArgs e)
        {
            TimetableEntry timetableEntry = (TimetableEntry)((Button)e.Source).DataContext;
            System.Diagnostics.Debug.WriteLine(timetableEntry.Departure.DepartureDateTime);
        }


        private class TimetableEntry
        {
            public Train Train { get; set; }
            public Departure Departure { get; set; }
            public TimetableEntry(Train train, Departure departure)
            {
                Train = train;
                Departure = departure;
            }
        }
    }
}
