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
            else if (e.RightButton == MouseButtonState.Pressed)
            {
                SelectedPushpin.Background = new SolidColorBrush(Colors.Orange);
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
            SelectedPushpin.Background = new SolidColorBrush(Colors.Green);
            if (e.RightButton == MouseButtonState.Pressed)
            {
                SelectedPushpin.Background = new SolidColorBrush(Colors.Orange);
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

            // Determin the location to place the pushpin at on the map.

            //Get the mouse click coordinates
            Point mousePosition = e.GetPosition(this);
            //Convert the mouse coordinates to a locatoin on the map
            Location pinLocation = MapWithEvents.ViewportPointToLocation(mousePosition);

            // The pushpin to add to the map.
            Pushpin pin = new Pushpin();
            pin.Location = pinLocation;
            pin.MouseDown += new MouseButtonEventHandler(pin_MouseDown);
            pin.MouseUp += new MouseButtonEventHandler(pin_MouseUp);
            pin.Background = new SolidColorBrush(Colors.Green);
            // Adds the pushpin to the map.
            MapWithEvents.Children.Add(pin);
        }



        void ShowEvent(string eventName)
        {
            System.Diagnostics.Debug.WriteLine(eventName);
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
            Train train = new Train(textBoxTrainName.Text, trainsStations, new Timetable());
            Database.train.Add(train);
        }

        private void Button2_Click(object sender,RoutedEventArgs e)
        {
            List<Pushpin> pushpinsToAdd = new List<Pushpin>();
            Train trainToAdd = Database.train.First();
            removeAllPushpins();
            foreach(Station station in trainToAdd.stations)
            {
                Pushpin pin = new Pushpin();
                pin.Location = station.location;
                pin.Background = new SolidColorBrush(Colors.Green);
                pushpinsToAdd.Add(pin);
                pin.MouseDown += new MouseButtonEventHandler(pin_MouseDown);
                pin.MouseUp += new MouseButtonEventHandler(pin_MouseUp);
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