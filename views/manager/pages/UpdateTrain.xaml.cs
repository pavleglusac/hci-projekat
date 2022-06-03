using HCIProjekat.model;
using Microsoft.Maps.MapControl.WPF;
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
    /// Interaction logic for SystemManagment.xaml
    /// </summary>
    public partial class UpdateTrain : Page
    {
        Vector _mouseToMarker;
        private bool _dragPin;
        public Pushpin SelectedPushpin{get; set;}
        public UpdateTrain()
        {
            InitializeComponent();
            //Set focus on map
            MapWithEvents.Focus();

            MapWithEvents.MouseDoubleClick +=
                new MouseButtonEventHandler(MapWithEvents_MouseDoubleClick);
            // Fires when the mouse wheel is used to scroll the map
            MapWithEvents.MouseMove +=
                new MouseEventHandler(MapWithEvents_MouseMove);
            this.KeyDown += new KeyEventHandler(MainWindow_KeyDown);
        }

        void MainWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
            {
                if (SelectedPushpin != null)
                {
                    Database.removeStation(SelectedPushpin.Location);
                    MapWithEvents.Children.Remove(SelectedPushpin);
                    SelectedPushpin = null;
                }
            }
        }
        private void MapWithEvents_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                if (_dragPin && SelectedPushpin != null)
                {
                    foreach(Station s in Database.stations)
                    {
                        if (s.location.Equals(SelectedPushpin.Location))
                        {
                            s.location = MapWithEvents.ViewportPointToLocation(
                      Point.Add(e.GetPosition(MapWithEvents), _mouseToMarker));
                        }
                    }
                    SelectedPushpin.Location = MapWithEvents.ViewportPointToLocation(
                      Point.Add(e.GetPosition(MapWithEvents), _mouseToMarker));
                    e.Handled = true;
                }
            }
        }


        void pin_MouseDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            SelectedPushpin = sender as Pushpin;
            _dragPin = true;
            _mouseToMarker = Point.Subtract(
              MapWithEvents.LocationToViewportPoint(SelectedPushpin.Location),
              e.GetPosition(MapWithEvents));
            if (e.RightButton == MouseButtonState.Pressed)
            {
                if (!SelectedPushpin.Background.ToString().Equals((new SolidColorBrush(Colors.Green)).ToString())){
                    MapWithEvents.Children.Remove(SelectedPushpin);
                    System.Diagnostics.Debug.WriteLine("obrisi");
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("promeni bojuuuuuuuuuuuuuuuuu");
                    SelectedPushpin.Background = new SolidColorBrush(Colors.Orange);
                }
            }
            else
            {
                SelectedPushpin.Background = new SolidColorBrush(Colors.Green);
            }
        }
        void pin_MouseUp(object sender, MouseButtonEventArgs e)
        {
            _dragPin = false;
        }


        private void MapWithEvents_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            // Disables the default mouse double-click action.
            e.Handled = true;
            if (e.LeftButton != MouseButtonState.Pressed)
            {
                return;
            }

            // Determin the location to place the pushpin at on the map.

            //Get the mouse click coordinates
            Point mousePosition = e.GetPosition(this);
            //Convert the mouse coordinates to a locatoin on the map
            Location pinLocation = MapWithEvents.ViewportPointToLocation(mousePosition);

            // The pushpin to add to the map.
            Pushpin pin = new Pushpin();
            pin.Location = pinLocation;
            pin.DataContext = "AAAA";
            pin.Content = "1";
            pin.MouseDown += new MouseButtonEventHandler(pin_MouseDown);
            pin.MouseUp += new MouseButtonEventHandler(pin_MouseUp);
            pin.Background = new SolidColorBrush(Colors.Green);
            // Adds the pushpin to the map.
            MapWithEvents.Children.Add(pin);
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {

            List<Pushpin> pushpinsToAdd = new List<Pushpin>();
            bool addPushpin = true;
            foreach (Station station in Database.stations)
            {
                    foreach (Pushpin child in MapWithEvents.Children)
                    {
                        if (child.Location.Equals(station.location))
                        {
                            addPushpin = false;
                        }

                    }
                    if (addPushpin)
                    {
                        Pushpin pin = new Pushpin();
                        pin.Location = station.location;
                        pin.Background = new SolidColorBrush(Colors.Orange);
                        pushpinsToAdd.Add(pin);
                        pin.MouseDown += new MouseButtonEventHandler(pin_MouseDown);
                        pin.MouseUp += new MouseButtonEventHandler(pin_MouseUp);
                    }
                    addPushpin = true;
            }

            foreach (Pushpin pushpin in pushpinsToAdd)
            {
                MapWithEvents.Children.Add(pushpin);
                pushpin.MouseDown += new MouseButtonEventHandler(pin_MouseDown);
                pushpin.MouseUp += new MouseButtonEventHandler(pin_MouseUp);
            }
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            List<Pushpin> pushpinsToRemove = new List<Pushpin>();
            foreach (Pushpin child in MapWithEvents.Children)
            {
                System.Diagnostics.Debug.WriteLine(child.Background.ToString() + "-"+ new SolidColorBrush(Colors.Green).ToString() + "="+ child.Background.ToString().Equals(new SolidColorBrush(Colors.Green).ToString()));
                if (!child.Background.ToString().Equals(new SolidColorBrush(Colors.Green).ToString()))
                    {
                        pushpinsToRemove.Add(child);
                    }

            }
            foreach (Pushpin pushpin in pushpinsToRemove)
            {
                MapWithEvents.Children.Remove(pushpin);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            List<Pushpin> pushpinsToAdd = new List<Pushpin>();
            List<Station> trainsStations = new List<Station>();
            foreach (Pushpin child in MapWithEvents.Children)
            {
                if(child.Background.ToString().Equals(new SolidColorBrush(Colors.Green).ToString()))
                {
                    trainsStations.Add(Database.getOrAddStation(child.Location));
                }
                foreach (Station s in Database.stations)
                {
                    if (!child.Location.Equals(s.location) && child.Background.ToString().Equals(new SolidColorBrush(Colors.Green).ToString()))
                    {
                        pushpinsToAdd.Add(child);
                    }
                }
            }
            foreach (Train train in Database.train)
            {
                if (train.name == textBoxTrainName.Text)
                {
                    train.updateStations(trainsStations);
                }
            }
        }

        private void Button2_Click(object sender,RoutedEventArgs e)
        {
            reloadStations();
        }

        private void reloadStations()
        {

            List<Pushpin> pushpinsToAdd = new List<Pushpin>();
            Train trainToAdd;
            foreach (Train train in Database.train)
            {
                if (train.name == textBoxTrainName.Text)
                {
                    removeAllPushpins();
                    foreach (Station station in train.stations)
                    {
                        Pushpin pin = new Pushpin();
                        pin.Location = station.location;
                        pin.Background = new SolidColorBrush(Colors.Green);
                        pushpinsToAdd.Add(pin);
                        pin.MouseDown += new MouseButtonEventHandler(pin_MouseDown);
                        pin.MouseUp += new MouseButtonEventHandler(pin_MouseUp);
                    }
                    break;
                }
            }
            foreach (Pushpin pushpin in pushpinsToAdd)
            {
                MapWithEvents.Children.Add(pushpin);
                pushpin.MouseDown += new MouseButtonEventHandler(pin_MouseDown);
                pushpin.MouseUp += new MouseButtonEventHandler(pin_MouseUp);
            }
        }

        private void removeAllPushpins()
        {

            List<Pushpin> pushpinsToRemove = new List<Pushpin>();
            foreach (Pushpin child in MapWithEvents.Children)
            {
                pushpinsToRemove.Add(child);
            }
            foreach (Pushpin pushpin in pushpinsToRemove)
            {
                MapWithEvents.Children.Remove(pushpin);
            }
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}