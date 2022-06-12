using HCIProjekat.model;
using MaterialDesignThemes.Wpf;
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

namespace HCIProjekat.views.manager.dialogs
{
    /// <summary>
    /// Interaction logic for StationName.xaml
    /// </summary>
    public partial class StationName : Page
    {
        public static String currentStationName;
        public Location currentLocation;
        public Pushpin pushpin;
        public Frame thisFrame;
        public DialogHost hostDialog;
        public int pinNum;
        public Boolean tutor;
        public Map map;
        Func<int> parentMethod;
        Func<int> parentMethod2;

        void enterKey(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {
                if (addButton.IsEnabled)
                {

                    if (!tutor)
                    {
                        Database.getOrAddStation(currentLocation);
                        Database.setName(currentLocation, textBoxTrainName.Text);
                        pushpin.ToolTip = Database.getOrAddStation(pushpin.Location).Name;
                    }
                    else
                    {
                        TutorDatabase.getOrAddStation(currentLocation);
                        TutorDatabase.setName(currentLocation, textBoxTrainName.Text);
                        pushpin.ToolTip = TutorDatabase.getOrAddStation(pushpin.Location).Name;
                    }
                    if (map != null)
                    {
                        map.Children.Add(pushpin);
                        pushpin.Content = parentMethod();
                    }
                    if (parentMethod2 != null)
                    {
                        parentMethod2();
                    }
                    hostDialog.IsOpen = false;
                }
            }
            if (e.Key == Key.R)
            {
                e.Handled = true;
            }
            if (e.Key == Key.Down)
            {
                e.Handled = true;
            }
            if (e.Key == Key.Up)
            {
                e.Handled = true;
            }
            if (e.Key > Key.NumPad0 && e.Key < Key.NumPad9)
            {
                e.Handled = true;
            }
        }
            public StationName(ref Pushpin pin, ref DialogHost dialog, ref Map MapWithEvents, Func<int> parentMethodSent, Func<int> parentMethod2Sent)
        {
            currentStationName = "";
            currentLocation = pin.Location;
            parentMethod = parentMethodSent;
            parentMethod2 = parentMethod2Sent;
            pushpin = pin;
            hostDialog = dialog;
            map = MapWithEvents;
            tutor = false;
            DataContext = new LoginInfo("", "");
            InitializeComponent();
            StationNameGrid.KeyDown += new KeyEventHandler(enterKey);
        }
        public StationName(ref Pushpin pin, ref DialogHost dialog, ref Map MapWithEvents, Func<int> parentMethodSent, Func<int> parentMethod2Sent,Boolean tutor)
        {
            currentStationName = "";
            currentLocation = pin.Location;
            parentMethod = parentMethodSent;
            parentMethod2 = parentMethod2Sent;
            pushpin = pin;
            hostDialog = dialog;
            map = MapWithEvents;
            this.tutor = true;
            DataContext = new LoginInfo("", "");
            InitializeComponent();
            StationNameGrid.KeyDown += new KeyEventHandler(enterKey);
        }
        public StationName(Pushpin pin, ref DialogHost dialog, string name)
        {
            currentStationName = name;
            map = null;
            pushpin = pin;
            currentLocation = pin.Location;
            hostDialog = dialog;
            DataContext = new LoginInfo(name, "");
            tutor = false;
            InitializeComponent();
            StationNameGrid.KeyDown += new KeyEventHandler(enterKey);
            textBoxTrainName.Text = name;
        }


        public StationName(Pushpin pin, ref DialogHost dialog, string name, Func<int> parentMethod2Sent)
        {
            currentStationName = name;
            map = null;
            pushpin = pin;
            currentLocation = pin.Location;
            hostDialog = dialog;
            DataContext = new LoginInfo(name, "");
            InitializeComponent();
            StationNameGrid.KeyDown += new KeyEventHandler(enterKey);
            tutor = false;
            parentMethod2 = parentMethod2Sent;
        }
        public StationName(Pushpin pin, ref DialogHost dialog, string name, Func<int> parentMethod2Sent, Boolean tutor)
        {

            currentStationName = name;
            map = null;
            pushpin = pin;
            currentLocation = pin.Location;
            hostDialog = dialog;
            DataContext = new LoginInfo("", "");
            InitializeComponent();
            StationNameGrid.KeyDown += new KeyEventHandler(enterKey);
            textBoxTrainName.Text = name;
            this.tutor = true;
            parentMethod2 = parentMethod2Sent;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!tutor)
            {
                Database.getOrAddStation(currentLocation);
                Database.setName(currentLocation, textBoxTrainName.Text);
                pushpin.ToolTip = Database.getOrAddStation(pushpin.Location).Name;
            }
            else
            {
                TutorDatabase.getOrAddStation(currentLocation);
                TutorDatabase.setName(currentLocation, textBoxTrainName.Text);
                pushpin.ToolTip = TutorDatabase.getOrAddStation(pushpin.Location).Name;
            }
            if (map != null)
            {
                map.Children.Add(pushpin);
                pushpin.Content = parentMethod();
            }
            if (parentMethod2 != null)
            {
                parentMethod2();
            }
            hostDialog.IsOpen = false;
        }
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            hostDialog.IsOpen = false;

        }


        private void handleLoginKeypress(object sender, KeyEventArgs e)
        {
            hideError();
        }

        private void hideError()
        {
            loginError.Text = "";
            loginError.Visibility = Visibility.Hidden;
        }


        public class LoginInfo
        {
            public string Username { get; set; }
            public string LoginError { get; set; }
            public LoginInfo(string username, string error)
            {
                Username = username;
                LoginError = error;
            }
        }



        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (addButton != null)
            {
                Boolean exists = false;
                foreach (Station station in Database.Stations)
                {
                    if (station.Name.Equals(textBoxTrainName.Text) && !station.Name.Equals(StationName.currentStationName))
                    {
                        exists = true;
                        break;
                    }
                }
                if (String.IsNullOrEmpty(textBoxTrainName.Text) || exists)
                {
                    addButton.IsEnabled = false;
                }
                else
                    addButton.IsEnabled = true;
            }
        }

    }
}
