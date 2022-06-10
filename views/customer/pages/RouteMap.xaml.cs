using HCIProjekat.model;
using Microsoft.Maps.MapControl.WPF;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
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
    /// Interaction logic for RouteMap.xaml
    /// </summary>
    public partial class RouteMap : Page
    {
        public List<Train> Trains = new();
        public List<Train> SelectedTrains = new();

        int pinNumber = 1;

        public RouteMap()
        {
            InitializeComponent();
            Trains = Database.Trains;
            trainsListBox.ItemsSource = Trains;
        }

        private void listBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectedTrains = new();
            var list = trainsListBox.SelectedItems.Cast<Train>();
            foreach (var train in list)
            {
                SelectedTrains.Add(train);
            }
            reloadStations();
        }
        private void reloadStations()
        {
            removeAllLines();
            removeAllPushpins();
            List<Color> backgrounds = new() 
            { 
                Colors.Green,
                Colors.Salmon,
                Colors.Blue, 
                Colors.Crimson, 
                Colors.Purple,
                Colors.Red,
            };
            int i = 0;
            foreach (Train train in SelectedTrains)
            {
                List<Pushpin> pushpinsToAdd = new List<Pushpin>();
                foreach (Station station in train.Stations.Keys)
                {
                    Pushpin pin = new Pushpin();
                    pin.Location = station.Location;
                    pin.Background = new SolidColorBrush(backgrounds[i % backgrounds.Count]);
                    pin.Content = train.Stations[station];
                    pushpinsToAdd.Add(pin);
                    pinNumber++;
                    pin.ToolTip = station.Name;
                }
                i++;
                pinNumber++;
                if (pushpinsToAdd.Count > 0)
                {

                    drawLines(pushpinsToAdd);
                    foreach (Pushpin pushpin in pushpinsToAdd)
                    {
                        MapWithEvents.Children.Add(pushpin);
                    }
                    pinNumber = 0;
                }
            }
        }
        private void drawLines(List<Pushpin> pushpinsToAdd)
        {
            var line = new MapPolyline();
            line.Locations = new LocationCollection();
            Location[] sortedLocations = new Location[pinNumber + 1];
            line.Stroke = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Gray);
            line.StrokeThickness = 3;
            foreach (Pushpin pushpin in pushpinsToAdd)
            {
                if (!pushpin.Background.ToString().Equals(new SolidColorBrush(Colors.Orange).ToString()))
                {
                    sortedLocations[Int32.Parse(pushpin.Content.ToString())] = pushpin.Location;
                }
            }
            for (int i = 1; i < pinNumber; i++)
            {
                line.Locations.Add(sortedLocations[i]);
            }
            MapWithEvents.Children.Add(line);
        }
        private void removeAllLines()
        {

            List<MapPolyline> linesToRemove = new List<MapPolyline>();
            foreach (var child in MapWithEvents.Children)
            {
                if (child is not Pushpin)
                {
                    linesToRemove.Add((MapPolyline)child);
                    continue;
                }
            }
            foreach (MapPolyline mapPolyline in linesToRemove)
            {
                MapWithEvents.Children.Remove(mapPolyline);
            }
        }


        private void removeAllPushpins()
        {

            List<Pushpin> pushpinsToRemove = new List<Pushpin>();
            List<MapPolyline> linesToRemove = new List<MapPolyline>();
            foreach (var child in MapWithEvents.Children)
            {
                if (child is not Pushpin)
                {
                    linesToRemove.Add((MapPolyline)child);
                    continue;
                }
                pushpinsToRemove.Add((Pushpin)child);
            }
            foreach (Pushpin pushpin in pushpinsToRemove)
            {
                MapWithEvents.Children.Remove(pushpin);
            }
            foreach (MapPolyline mapPolyline in linesToRemove)
            {
                MapWithEvents.Children.Remove(mapPolyline);
            }
            pinNumber = 0;
        }
    }
}
