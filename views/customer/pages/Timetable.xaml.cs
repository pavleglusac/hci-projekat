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
        List<Departure> Departures = new List<Departure>();
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
            Departures = new List<Departure>();
            for (int i = 0; i < 10; i++)
            {
                Departures.Add(new Departure("Soko " + (i%3).ToString(), DateTime.Parse($"2022-06-01T0{i%5}:0{(i * 23) % 10}"), 
                    DateTime.Parse($"2022-06-01T0{i%3+5}:0{(i* 27) % 10}"), 2, 30, 800+(i*20)));
            }
            departuresGrid.ItemsSource = Departures;
        }

        private void buyTicketButtonClick(object sender, RoutedEventArgs e)
        {
            Departure departure = (Departure)((Button)e.Source).DataContext;
            System.Diagnostics.Debug.WriteLine(departure.DepartureDateTime);
        }
    }
}
