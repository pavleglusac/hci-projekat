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

namespace HCIProjekat.views.manager.tutorial
{
    /// <summary>
    /// Interaction logic for AddTrain.xaml
    /// </summary>
    public partial class AddTrainTutorial : Page
    {
        int pinNumber = 1;
        int numbersPressed = 0;
        int numberPressed = 0;
        bool deleteEnabled;
        bool upArrowEnabled;
        bool downArrowEnabled;
        bool numpadEnabled;
        bool rEnabled;
        bool doubleClickEnabled;
        bool dragEnabled;
        bool rightEnabled;
        bool returnGreenEnabled;
        int stepNum = 1;
        Vector _mouseToMarker;
        private bool _dragPin;
        public Pushpin SelectedPushpin { get; set; }
        CancellationTokenSource timeout { get; set; }
        public bool IsDialogOpen = true;
        public Frame DialogContent = new Frame();
        public DialogHost parentDialog;
        public AddTrainTutorial()
        {
            InitializeComponent();
            //Set focus on map
            MapWithEvents.Focus();

            MapWithEvents.MouseDoubleClick +=
                new MouseButtonEventHandler(MapWithEvents_MouseDoubleClick);
            MapWithEvents.MouseDown +=
                new MouseButtonEventHandler(MapWithEvents_MouseDown);
            // Fires when the mouse wheel is used to scroll the map
            MapWithEvents.MouseMove +=
                new MouseEventHandler(MapWithEvents_MouseMove);
            MapWithEvents.KeyDown += new KeyEventHandler(preventDefault);
            showStationsCheckBox.IsEnabled = false;
            textBoxTrainName.IsEnabled = false;
            deleteEnabled = false;
            upArrowEnabled = false;
            downArrowEnabled = false;
            numpadEnabled = false;
            rEnabled = false;
            doubleClickEnabled = true;
            dragEnabled = false;
            Add_Button.IsEnabled = false;
            returnGreenEnabled = false;
            rightEnabled = false;
        }

        public void nextStep()
        {
            stepNum++;
            drawLines();
            callNextStep();
        }


        public void Step1(object sender, EventArgs e)
        {
            MessageBox.Show("Dobrodošli u tutorijal za upravljanje vozom! Kliknite OK da biste nastavili.", "Tutor", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.Yes);
            MessageBox.Show("Dvokliknite na mapu da dodate stanicu.", "Tutor", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.Yes);
            textBlock.Text = "Dvokliknite na mapu da dodate stanicu.";
        }


        public void callNextStep()
        {
            switch (stepNum)
            {
                case 2:
                    {
                        MessageBox.Show("Čestitamo! Dvokliknite na mapu ponovo da dodate drugu stanicu.", "Tutor", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.Yes);
                        textBlock.Text= "Dvokliknite na mapu ponovo da dodate drugu stanicu.";
                        break;
                    }
                case 3:
                    {
                        MessageBox.Show("Čestitamo! Prevucite neku stanicu kako bi joj promenili koordinate.", "Tutor", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.Yes);
                        doubleClickEnabled = false;
                        dragEnabled = true;
                        textBlock.Text = "Prevucite neku stanicu kako bi joj promenili koordinate.";
                        break;
                    }
                case 4:
                    {
                        MessageBox.Show("Čestitamo! Izaberite stanicu sa brojem 1 i pritisnite taster gore kako bi ona postala stanica broj 2.", "Tutor", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.Yes);
                        dragEnabled = false; 
                        upArrowEnabled = true;
                        textBlock.Text = "Izaberite stanicu sa brojem 1 i pritisnite taster gore kako bi ona postala stanica broj 2.";
                        break;
                    }
                case 5:
                    {
                        MessageBox.Show("Čestitamo! Izaberite stanicu sa brojem 2 i pritisnite taster dole kako bi ona postala stanica broj 1.", "Tutor", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.Yes);
                        upArrowEnabled = false;
                        downArrowEnabled = true;
                        textBlock.Text = "Izaberite stanicu sa brojem 2 i pritisnite taster dole kako bi ona postala stanica broj 1.";
                        break;
                    }
                case 6:
                    {
                        MessageBox.Show("Čestitamo! Izaberite bilo koju stanicu i pritiskom tastera na numpad-u joj promenite broj.", "Tutor", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.Yes);
                        downArrowEnabled = false;
                        numpadEnabled = true;
                        textBlock.Text = "Izaberite bilo koju stanicu i pritiskom tastera na numpad-u joj promenite broj.";
                        break; 
                    }
                case 7:
                    {
                        MessageBox.Show("Čestitamo! Izaberite bilo koju stanicu i pritiskom taster r joj promenite ime.", "Tutor", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.Yes);
                        numpadEnabled = false;
                        rEnabled = true;
                        textBlock.Text = "Izaberite bilo koju stanicu i pritiskom taster r joj promenite ime.";
                        break;
                    }
                case 8:
                    {
                        MessageBox.Show("Čestitamo! Klikom na checkbox prikažite već postojeće stanice.", "Tutor", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.Yes);
                        numpadEnabled = false;
                        showStationsCheckBox.IsEnabled = true;
                        textBlock.Text = "Klikom na checkbox prikažite već postojeće stanice.";
                        break;
                    }
                case 9:
                    {
                        MessageBox.Show("Čestitamo! Klikom na checkbox uklonite već postojeće stanice.", "Tutor", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.Yes);
                        textBlock.Text = "Klikom na checkbox uklonite već postojeće stanice."; 
                        break;
                    }
                case 10:
                    {
                        MessageBox.Show("Čestitamo! Dvoklikom na mapu dodajte još jednu stanicu", "Tutor", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.Yes);
                        showStationsCheckBox.IsEnabled = false;
                        doubleClickEnabled = true;
                        textBlock.Text = "Dvokliknite na mapu da dodate stanicu.";
                        break;
                    }
                case 11:
                    {
                        MessageBox.Show("Čestitamo! Izaberite jednu stanicu i pritiskom na taster delete je obrišite", "Tutor", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.Yes);
                        doubleClickEnabled = false;
                        deleteEnabled = true;
                        textBlock.Text = "Izaberite jednu stanicu i pritiskom na taster delete je obrišite.";
                        break;
                    }
                case 12:
                    {
                        MessageBox.Show("Čestitamo! Dvoklikom na mapu dodajte još jednu stanicu", "Tutor", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.Yes);
                        deleteEnabled = false;
                        rightEnabled = false; 
                        doubleClickEnabled = true;
                        textBlock.Text = "Dvokliknite na mapu da dodate stanicu.";
                        break;
                    }
                case 13:
                    {
                        MessageBox.Show("Čestitamo! Izaberite jednu stanicu i pritiskom na desni klik je uklonite sa rute", "Tutor", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.Yes);
                        rightEnabled = true;
                        doubleClickEnabled = false;
                        textBlock.Text = "Izaberite jednu stanicu i pritiskom na taster delete je obrišite.";
                        break;
                    }
                case 14:
                    {
                        MessageBox.Show("Čestitamo! Izaberite istu stanicu i pritiskom na levi klik je vratite na rutu", "Tutor", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.Yes);
                        rightEnabled = false;
                        returnGreenEnabled = true;
                        textBlock.Text = " Izaberite istu stanicu i pritiskom na levi klik je vratite na rutu.";
                        break;
                    }
                case 15:
                    {
                        MessageBox.Show("Čestitamo! Unesite ime vašeg voza pa pritiskom na dugme dodajte voz", "Tutor", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.Yes);
                        textBoxTrainName.IsEnabled = true;
                        returnGreenEnabled = false;
                        textBlock.Text = " Unesite ime vašeg voza!";
                        break;
                    }
                case 16:
                    {
                        MessageBox.Show("Čestitamo! Uspešno ste završili ovaj tutorial", "Tutor", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.Yes);

                        MainWindow parentWindow = Window.GetWindow(this) as MainWindow;
                        if (parentWindow != null)
                        {
                            parentWindow.Close();
                        }
                        break;
                    }
            }
        }

        void preventDefault(object sender, KeyEventArgs e)
        {

            e.Handled = true;
            if (e.Key == Key.Delete && deleteEnabled)
            {
                if (SelectedPushpin != null && SelectedPushpin.Content != null && SelectedPushpin.ToolTip != null)
                {

                    if (MessageBox.Show("Da li ste sigurni da želite da obrišete stanicu pod nazivom " + SelectedPushpin.ToolTip + "?",
                        "Potvrda", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    {
                        TutorDatabase.removeStation(SelectedPushpin.Location);
                        MapWithEvents.Children.Remove(SelectedPushpin);
                        if (Int32.TryParse(SelectedPushpin.Content.ToString(), out _))
                        {
                            updatePushpinsNumber(Int32.Parse(SelectedPushpin.Content.ToString()));
                            pinNumber--;
                        }
                        SelectedPushpin = null;
                    }
                }
                else
                {
                    MessageBox.Show("Morate izabrati stanicu koju ćete obrisati.");
                }
                drawLines();
                nextStep();
            }
            if (e.Key == Key.Down && downArrowEnabled)
            {
                if (SelectedPushpin != null && SelectedPushpin.Content != null && (Int32.TryParse(SelectedPushpin.Content.ToString(), out _)))
                {
                    if (Int32.Parse(SelectedPushpin.Content.ToString()) == 1)
                    {
                        return;
                    }
                    SelectedPushpin.Content = Int32.Parse(SelectedPushpin.Content.ToString()) - 1;
                    foreach (var child in MapWithEvents.Children)
                    {
                        if (child is not Pushpin)
                        {
                            continue;
                        }
                        Pushpin pushpin = (Pushpin)child;
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
                else
                {
                    MessageBox.Show("Morate izabrati stanicu kojoj ćete promeniti broj.");
                }
                nextStep();
                drawLines();
            }
            if (e.Key == Key.Up && upArrowEnabled)
            {
                if (SelectedPushpin != null && SelectedPushpin.Content != null && (Int32.TryParse(SelectedPushpin.Content.ToString(), out _)))
                {
                    if (Int32.Parse(SelectedPushpin.Content.ToString()) == pinNumber - 1)
                    {
                        return;
                    }
                    SelectedPushpin.Content = Int32.Parse(SelectedPushpin.Content.ToString()) + 1;
                    foreach (var child in MapWithEvents.Children)
                    {
                        if (child is not Pushpin)
                        {
                            continue;
                        }
                        Pushpin pushpin = (Pushpin)child;
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
                else
                {
                    MessageBox.Show("Morate izabrati stanicu kojoj ćete promeniti broj.");
                }
                nextStep();
                drawLines();
            }
            if ((e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9) && numpadEnabled)
            {
                if (SelectedPushpin != null && SelectedPushpin.Content != null)
                {
                    numberPressed *= (numbersPressed == 0) ? 0 : Convert.ToInt32(Math.Pow(10, numbersPressed));
                    numberPressed += (e.Key.ToString()[e.Key.ToString().Length - 1] - 48);
                    numbersPressed++;
                    System.Diagnostics.Debug.WriteLine(numberPressed);
                    ClearTimeout(timeout);
                    timeout = SetTimeout(() =>
                    {
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
                                foreach (var child in MapWithEvents.Children)
                                {
                                    if (child is not Pushpin)
                                    {
                                        continue;
                                    }
                                    Pushpin pushpin = (Pushpin)child;
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
                            nextStep();
                        });
                    }, 200);
                }
                else
                {
                    MessageBox.Show("Morate izabrati stanicu kojoj ćete promeniti broj.");
                }
            }
            if(e.Key == Key.R && rEnabled)
            {
                if (SelectedPushpin != null && SelectedPushpin.Background.ToString().Equals(new SolidColorBrush(Colors.Blue).ToString()) && SelectedPushpin.ToolTip != null)
                {
                    DialogContent.Content = new StationName(SelectedPushpin, ref TrainsDialogHost, SelectedPushpin.ToolTip.ToString(), onModalClose2, true);
                    DialogContent.Height = 250;
                    DialogContent.Width = 500;
                    System.Diagnostics.Debug.WriteLine("AFD-SDSFSD");
                    IsDialogOpen = true;
                    TrainsDialogHost.DialogContent = DialogContent;
                    TrainsDialogHost.CloseOnClickAway = true;
                    TrainsDialogHost.ShowDialog(DialogContent);
                }
                else
                {
                    MessageBox.Show("Morate izabrati stanicu kojoj ćete promeniti ime.");
                }
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

            foreach (var child in MapWithEvents.Children)
            {
                if (child is not Pushpin)
                {
                    continue;
                }
                Pushpin pushpin = (Pushpin)child;
                if (!Int32.TryParse(pushpin.Content.ToString(), out _))
                {
                    continue;
                }
                if (Int32.Parse(pushpin.Content.ToString()) > deletedNumber)
                {
                    pushpin.Content = Int32.Parse(pushpin.Content.ToString()) - 1;
                }
            }
            drawLines();
        }
        private void MapWithEvents_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                if (_dragPin && SelectedPushpin != null && dragEnabled)
                {
                    foreach (Station s in TutorDatabase.Stations)
                    {
                        if (s.Location.Equals(SelectedPushpin.Location))
                        {
                            s.Location = MapWithEvents.ViewportPointToLocation(
                      Point.Add(e.GetPosition(MapWithEvents), _mouseToMarker));
                        }
                    }
                    SelectedPushpin.Location = MapWithEvents.ViewportPointToLocation(
                      Point.Add(e.GetPosition(MapWithEvents), _mouseToMarker));
                    drawLines();
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
            List<String> trainNames = TutorDatabase.getTrainsNamesWithStation(SelectedPushpin.Location);
            String showString = " ";
            foreach(String trainName in trainNames)
            {
                showString += trainName + ", ";
            }
            if (trainNames.Count > 0 && (oldPushpinLocation is null || !oldPushpinLocation.Equals((sender as Pushpin).Location)))
            {
                if (returnGreenEnabled)
                {
                    if (MessageBox.Show("Upravo će te da uredite stanicu koja će uticati na ove vozove:" + showString.Remove(showString.Length - 2) + ".",
                    "Potvrda", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
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
            }
            else
            {
                SelectedPushpin = sender as Pushpin;
                _dragPin = true;
                _mouseToMarker = Point.Subtract(
                  MapWithEvents.LocationToViewportPoint(SelectedPushpin.Location),
                  e.GetPosition(MapWithEvents));
                if (e.RightButton == MouseButtonState.Pressed && rightEnabled)
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
                        nextStep();
                    }
                }
                else if((e.RightButton == MouseButtonState.Pressed && rightEnabled) || 
                    (e.LeftButton == MouseButtonState.Pressed && (!SelectedPushpin.Background.ToString().Equals((new SolidColorBrush(Colors.Orange)).ToString()) || returnGreenEnabled)))
                {
                    if (SelectedPushpin.Background.ToString().Equals((new SolidColorBrush(Colors.Orange)).ToString()))
                    {

                        if (returnGreenEnabled)
                        {
                            nextStep();
                        }
                        SelectedPushpin.Content = pinNumber++;
                    }
                    if (!SelectedPushpin.Background.ToString().Equals((new SolidColorBrush(Colors.Blue)).ToString()))
                    {
                        SelectedPushpin.Background = new SolidColorBrush(Colors.Blue);
                    }
                }
            }
            drawLines();
        }
        void pin_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (_dragPin && stepNum == 3)
            {
                nextStep();
            }
            _dragPin = false;
        }
        private void MapWithEvents_MouseDown(object sender, MouseButtonEventArgs e)
        {
        }

            
        private void MapWithEvents_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            // Disables the default mouse double-click action.
            e.Handled = true;
            if (e.LeftButton != MouseButtonState.Pressed || !doubleClickEnabled)
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
                DialogContent.Content = new StationName(ref pin,ref TrainsDialogHost, ref MapWithEvents, onModalClose, onModalClose2,true);
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

        public int onModalClose2()
        {
            drawLines();
            nextStep();
            return 0;
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {

            List<Pushpin> pushpinsToAdd = new List<Pushpin>();
            bool addPushpin = true;
            foreach (Station station in TutorDatabase.Stations)
            {
                foreach (var child in MapWithEvents.Children)
                {
                    if (child is not Pushpin)
                    {
                        continue;
                    }
                    Pushpin pushpin = (Pushpin)child;
                    if (pushpin.Location.Equals(station.Location))
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
            nextStep();
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            List<Pushpin> pushpinsToRemove = new List<Pushpin>();
            foreach (var child in MapWithEvents.Children)
            {
                if (child is not Pushpin)
                {
                    continue;
                }
                Pushpin pushpin = (Pushpin)child;
                System.Diagnostics.Debug.WriteLine(pushpin.Background.ToString() + "-" + new SolidColorBrush(Colors.Green).ToString() + "=" + pushpin.Background.ToString().Equals(new SolidColorBrush(Colors.Green).ToString()));
                if (pushpin.Background.ToString().Equals(new SolidColorBrush(Colors.Orange).ToString()))
                {
                    pushpinsToRemove.Add(pushpin);
                }

            }
            foreach (Pushpin pushpin in pushpinsToRemove)
            {
                MapWithEvents.Children.Remove(pushpin);
            }
            nextStep();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            List<Pushpin> pushpinsToAdd = new List<Pushpin>();
            Dictionary<Station, int> trainsStations = new Dictionary<Station, int>();
            foreach (var child in MapWithEvents.Children)
            {
                if (child is not Pushpin)
                {
                    continue;
                }
                Pushpin pushpin = (Pushpin)child;
                if (!pushpin.Background.ToString().Equals(new SolidColorBrush(Colors.Orange).ToString()))
                {
                    trainsStations.Add(TutorDatabase.getOrAddStation(pushpin.Location), Int32.Parse(pushpin.Content.ToString()));
                }
                foreach (Station s in TutorDatabase.Stations)
                {
                    if (!pushpin.Location.Equals(s.Location) && !pushpin.Background.ToString().Equals(new SolidColorBrush(Colors.Orange).ToString()))
                    {
                        pushpinsToAdd.Add(pushpin);
                    }
                }
            }
            Train train = new Train(textBoxTrainName.Text, trainsStations, new List<Departure>(), 20);
            TutorDatabase.Trains.Add(train);
            nextStep();
        }

        private void drawLines(List<Pushpin> pushpinsToAdd)
        {
            var line = new MapPolyline();
            line.Locations = new LocationCollection();
            Location[] sortedLocations = new Location[pinNumber + 1];
            line.Stroke = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Gray);
            line.StrokeThickness = 3;
            foreach (Pushpin child in pushpinsToAdd)
            {
                if (!child.Background.ToString().Equals(new SolidColorBrush(Colors.Orange).ToString()))
                {
                    sortedLocations[Int32.Parse(child.Content.ToString())] = child.Location;
                }
            }
            for (int i = 1; i < pinNumber; i++)
            {
                line.Locations.Add(sortedLocations[i]);
            }
            MapWithEvents.Children.Add(line);
        }
        private void drawLines()
        {
            removeAllLines();
            var line = new MapPolyline();
            line.Locations = new LocationCollection();
            Location[] sortedLocations = new Location[pinNumber + 1];
            List<Pushpin> pushpins = new List<Pushpin>();
            line.Stroke = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Gray);
            line.StrokeThickness = 3;
            foreach (var child in MapWithEvents.Children)
            {
                if (child is not Pushpin)
                {
                    continue;
                }
                Pushpin pushpin = (Pushpin)child;
                pushpins.Add(pushpin);
                if (!pushpin.Background.ToString().Equals(new SolidColorBrush(Colors.Orange).ToString()))
                {
                    if (pushpin.Content == null || String.IsNullOrEmpty(pushpin.Content.ToString()))
                    {
                        continue;
                    }
                    sortedLocations[Int32.Parse(pushpin.Content.ToString())] = pushpin.Location;
                }
            }
            for (int i = 1; i < pinNumber; i++)
            {
                line.Locations.Add(sortedLocations[i]);
            }
            MapWithEvents.Children.Add(line);
            foreach (Pushpin pushpin in pushpins)
            {
                MapWithEvents.Children.Remove(pushpin);
                MapWithEvents.Children.Add(pushpin);
            }
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




        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            Add_Button.IsEnabled = true;
            textBlock.Text = " Dodajte voz!";
        }
    }
}