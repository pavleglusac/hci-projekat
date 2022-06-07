using HCIProjekat.model;
using HCIProjekat.views.manager.dialogs;
using MaterialDesignThemes.Wpf;
using Microsoft.Maps.MapControl.WPF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
    /// Interaction logic for AddTrain.xaml
    /// </summary>
    public partial class AddTrain : Page
    {
        int pinNumber = 1;
        int numbersPressed = 0;
        int numberPressed = 0;
        Vector _mouseToMarker;
        private bool _dragPin;
        public Pushpin SelectedPushpin { get; set; }
        CancellationTokenSource timeout { get; set; }
        public bool IsDialogOpen = true;
        public Frame DialogContent = new Frame();
        public DialogHost parentDialog;
        public AddTrain(ref DialogHost dialog)
        {
            InitializeComponent();
            //Set focus on map
            MapWithEvents.Focus();
            parentDialog = dialog;

            MapWithEvents.MouseDoubleClick +=
                new MouseButtonEventHandler(MapWithEvents_MouseDoubleClick);
            MapWithEvents.MouseDown +=
                new MouseButtonEventHandler(MapWithEvents_MouseDown);
            // Fires when the mouse wheel is used to scroll the map
            MapWithEvents.MouseMove +=
                new MouseEventHandler(MapWithEvents_MouseMove);
            MapWithEvents.KeyDown += new KeyEventHandler(preventDefault);
        }

        void preventDefault(object sender, KeyEventArgs e)
        {

            e.Handled = true;
            if (e.Key == Key.Delete)
            {
                if (SelectedPushpin != null)
                {
                    Database.removeStation(SelectedPushpin.Location);
                    MapWithEvents.Children.Remove(SelectedPushpin);
                    if (Int32.TryParse(SelectedPushpin.Content.ToString(), out _))
                    {
                        updatePushpinsNumber(Int32.Parse(SelectedPushpin.Content.ToString()));
                        pinNumber--;
                    }
                    SelectedPushpin = null;
                }
            }
            if (e.Key == Key.Down)
            {
                if (SelectedPushpin != null && (Int32.TryParse(SelectedPushpin.Content.ToString(), out _)))
                {
                    if (Int32.Parse(SelectedPushpin.Content.ToString()) == 1)
                    {
                        return;
                    }
                    SelectedPushpin.Content = Int32.Parse(SelectedPushpin.Content.ToString()) - 1;
                    foreach (Pushpin pushpin in MapWithEvents.Children)
                    {
                        if (!Int32.TryParse(pushpin.Content.ToString(), out _))
                        {
                            continue;
                        }
                        if (Int32.Parse(pushpin.Content.ToString()) == Int32.Parse(SelectedPushpin.Content.ToString()) && !pushpin.Location.Equals(SelectedPushpin.Location))
                        {
                            pushpin.Content = Int32.Parse(pushpin.Content.ToString()) + 1;
                        }
                    }
                }
            }
            if (e.Key == Key.Up)
            {
                if (SelectedPushpin != null && (Int32.TryParse(SelectedPushpin.Content.ToString(), out _)))
                {
                    if (Int32.Parse(SelectedPushpin.Content.ToString()) == pinNumber - 1)
                    {
                        return;
                    }
                    SelectedPushpin.Content = Int32.Parse(SelectedPushpin.Content.ToString()) + 1;
                    foreach (Pushpin pushpin in MapWithEvents.Children)
                    {
                        if (!Int32.TryParse(pushpin.Content.ToString(), out _))
                        {
                            continue;
                        }
                        if (Int32.Parse(pushpin.Content.ToString()) == Int32.Parse(SelectedPushpin.Content.ToString()) && !pushpin.Location.Equals(SelectedPushpin.Location))
                        {
                            pushpin.Content = Int32.Parse(pushpin.Content.ToString()) - 1;
                        }
                    }
                }
            }
            if ((e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9))
            {
                numberPressed *= (numbersPressed == 0) ? 0 : Convert.ToInt32(Math.Pow(10, numbersPressed));
                numberPressed += (e.Key.ToString()[e.Key.ToString().Length - 1] - 48);
                numbersPressed++;
                System.Diagnostics.Debug.WriteLine(numberPressed);
                ClearTimeout(timeout);
                timeout = SetTimeout(() => {
                    this.Dispatcher.Invoke(() =>
                    {
                        int contentBefore;
                        System.Diagnostics.Debug.WriteLine("AAAAAAA" + numberPressed);
                        if (SelectedPushpin != null && (Int32.TryParse(SelectedPushpin.Content.ToString(), out _)))
                        {
                            if (numberPressed > pinNumber - 1 || numberPressed < 0)
                            {
                                numberPressed = 0;
                                numbersPressed = 0;
                                return;
                            }
                            contentBefore = Int32.Parse(SelectedPushpin.Content.ToString());
                            SelectedPushpin.Content = numberPressed;
                            foreach (Pushpin pushpin in MapWithEvents.Children)
                            {
                                if (!Int32.TryParse(pushpin.Content.ToString(), out _))
                                {
                                    continue;
                                }
                                if (Int32.Parse(pushpin.Content.ToString()) == Int32.Parse(SelectedPushpin.Content.ToString()) && !pushpin.Location.Equals(SelectedPushpin.Location))
                                {
                                    pushpin.Content = contentBefore;
                                }
                            }
                        }
                        numberPressed = 0;
                        numbersPressed = 0;
                    });
                }, 200);
            }
            if(e.Key == Key.R)
            {
                DialogContent.Content = new StationName(SelectedPushpin, ref TrainsDialogHost, SelectedPushpin.ToolTip.ToString());
                DialogContent.Height = 250;
                DialogContent.Width = 500;
                System.Diagnostics.Debug.WriteLine("AFD-SDSFSD");
                IsDialogOpen = true;
                TrainsDialogHost.DialogContent = DialogContent;
                TrainsDialogHost.CloseOnClickAway = true;
                TrainsDialogHost.ShowDialog(DialogContent);
            }
        }

        public CancellationTokenSource SetTimeout(Action action, int millis)
        {

            var cts = new CancellationTokenSource();
            var ct = cts.Token;
            _ = Task.Run(() => {
                Thread.Sleep(millis);
                if (!ct.IsCancellationRequested)
                    action();
            }, ct);

            return cts;
        }

        public void ClearTimeout(CancellationTokenSource cts)
        {
            if (cts != null)
                cts.Cancel();
        }

        void updatePushpinsNumber(int deletedNumber)
        {

            foreach (Pushpin pushpin in MapWithEvents.Children)
            {
                if (!Int32.TryParse(pushpin.Content.ToString(), out _))
                {
                    continue;
                }
                if (Int32.Parse(pushpin.Content.ToString()) > deletedNumber)
                {
                    pushpin.Content = Int32.Parse(pushpin.Content.ToString()) - 1;
                }
            }
        }
        private void MapWithEvents_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                if (_dragPin && SelectedPushpin != null)
                {
                    foreach (Station s in Database.Stations)
                    {
                        if (s.Location.Equals(SelectedPushpin.Location))
                        {
                            s.Location = MapWithEvents.ViewportPointToLocation(
                      Point.Add(e.GetPosition(MapWithEvents), _mouseToMarker));
                        }
                    }
                    SelectedPushpin.Location = MapWithEvents.ViewportPointToLocation(
                      Point.Add(e.GetPosition(MapWithEvents), _mouseToMarker));
                    e.Handled = true;
                }
            }
            MapWithEvents.Focus();
        }


        void pin_MouseDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            Location oldPushpinLocation = null;
            if (SelectedPushpin is not null)
            {
                if (SelectedPushpin.Background.ToString().Equals(new SolidColorBrush(Colors.Blue).ToString()))
                {
                    SelectedPushpin.Background = new SolidColorBrush(Colors.Green);
                }
                oldPushpinLocation = SelectedPushpin.Location;
            }
            SelectedPushpin = sender as Pushpin;
            List<String> trainNames = Database.getTrainsNamesWithStation(SelectedPushpin.Location);
            String showString = " ";
            foreach(String trainName in trainNames)
            {
                showString += trainName + ", ";
            }
            if (trainNames.Count > 0 && (oldPushpinLocation is null || !oldPushpinLocation.Equals((sender as Pushpin).Location)))
            {
                if (MessageBox.Show("You are about to edit a station that will affect these trains:" + showString.Remove(showString.Length - 2) + ".",
                    "Confirmation", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    //_dragPin = true;
                    if (SelectedPushpin.Background.ToString().Equals((new SolidColorBrush(Colors.Orange)).ToString()))
                    {
                        SelectedPushpin.Content = pinNumber++;
                    }
                    if (!SelectedPushpin.Background.ToString().Equals((new SolidColorBrush(Colors.Blue)).ToString()))
                    {
                        SelectedPushpin.Background = new SolidColorBrush(Colors.Blue);
                    }
                }
                else
                {
                    SelectedPushpin = null;
                }
            }
            else
            {
                SelectedPushpin = sender as Pushpin;
                _dragPin = true;
                _mouseToMarker = Point.Subtract(
                  MapWithEvents.LocationToViewportPoint(SelectedPushpin.Location),
                  e.GetPosition(MapWithEvents));
                if (e.RightButton == MouseButtonState.Pressed)
                {
                    System.Diagnostics.Debug.WriteLine("----" + SelectedPushpin.Background.ToString() + "=" + new SolidColorBrush(Colors.Orange).ToString());
                    if (SelectedPushpin.Background.ToString().Equals((new SolidColorBrush(Colors.Orange)).ToString()))
                    {
                        MapWithEvents.Children.Remove(SelectedPushpin);
                        if (!Int32.TryParse(SelectedPushpin.Content.ToString(), out _))
                        {
                            return;
                        }
                        updatePushpinsNumber(Int32.Parse(SelectedPushpin.Content.ToString()));
                        pinNumber--;
                        System.Diagnostics.Debug.WriteLine("obrisi");
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine("promeni bojuuuuuuuuuuuuuuuuu");
                        SelectedPushpin.Background = new SolidColorBrush(Colors.Orange);
                        if (!Int32.TryParse(SelectedPushpin.Content.ToString(), out _))
                        {
                            return;
                        }
                        updatePushpinsNumber(Int32.Parse(SelectedPushpin.Content.ToString()));
                        SelectedPushpin.Content = "";
                        pinNumber--;
                    }
                }
                else
                {
                    if (SelectedPushpin.Background.ToString().Equals((new SolidColorBrush(Colors.Orange)).ToString()))
                    {
                        SelectedPushpin.Content = pinNumber++;
                    }
                    if (!SelectedPushpin.Background.ToString().Equals((new SolidColorBrush(Colors.Blue)).ToString()))
                    {
                        SelectedPushpin.Background = new SolidColorBrush(Colors.Blue);
                    }

                }
            }
        }
        void pin_MouseUp(object sender, MouseButtonEventArgs e)
        {
            _dragPin = false;
        }
        private void MapWithEvents_MouseDown(object sender, MouseButtonEventArgs e)
        {
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
            pin.MouseDown += new MouseButtonEventHandler(pin_MouseDown);
            pin.MouseUp += new MouseButtonEventHandler(pin_MouseUp);
            pin.Background = new SolidColorBrush(Colors.Blue);
            // Adds the pushpin to the map.
            //MapWithEvents.Children.Add(pin);
            if (SelectedPushpin != null)
            {
                SelectedPushpin.Background = new SolidColorBrush(Colors.Green);
            }
            SelectedPushpin = pin;
            Task.Delay(100).ContinueWith(t =>
                Dispatcher.Invoke(() =>
            {
                DialogContent.Content = new StationName(ref pin,ref TrainsDialogHost, ref MapWithEvents, onModalClose);
                DialogContent.Height = 250;
                DialogContent.Width = 500;
                IsDialogOpen = true;
                TrainsDialogHost.DialogContent = DialogContent;
                TrainsDialogHost.CloseOnClickAway = true;
                TrainsDialogHost.ShowDialog(DialogContent);
            }            
            ));
        }

        public int onModalClose()
        {
            return pinNumber++;
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {

            List<Pushpin> pushpinsToAdd = new List<Pushpin>();
            bool addPushpin = true;
            foreach (Station station in Database.Stations)
            {
                foreach (Pushpin child in MapWithEvents.Children)
                {
                    if (child.Location.Equals(station.Location))
                    {
                        addPushpin = false;
                    }

                }
                if (addPushpin)
                {
                    Pushpin pin = new Pushpin();
                    pin.Location = station.Location;
                    pin.Background = new SolidColorBrush(Colors.Orange);
                    pin.Content = "";
                    pushpinsToAdd.Add(pin);
                    pin.ToolTip = station.Name;
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
                System.Diagnostics.Debug.WriteLine(child.Background.ToString() + "-" + new SolidColorBrush(Colors.Green).ToString() + "=" + child.Background.ToString().Equals(new SolidColorBrush(Colors.Green).ToString()));
                if (child.Background.ToString().Equals(new SolidColorBrush(Colors.Orange).ToString()))
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
            Dictionary<Station, int> trainsStations = new Dictionary<Station, int>();
            foreach (Pushpin child in MapWithEvents.Children)
            {
                if (!child.Background.ToString().Equals(new SolidColorBrush(Colors.Orange).ToString()))
                {
                    trainsStations.Add(Database.getOrAddStation(child.Location), Int32.Parse(child.Content.ToString()));
                }
                foreach (Station s in Database.Stations)
                {
                    if (!child.Location.Equals(s.Location) && !child.Background.ToString().Equals(new SolidColorBrush(Colors.Orange).ToString()))
                    {
                        pushpinsToAdd.Add(child);
                    }
                }
            }
            Train train = new Train(textBoxTrainName.Text, trainsStations, new List<Departure>(), 20);
            Database.Trains.Add(train);
            parentDialog.IsOpen = false;
        }


        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}