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
        public Location currentLocation;
        public Pushpin pushpin;
        public Frame thisFrame;
        public DialogHost hostDialog;
        public int pinNum;
        public Map map;
        Func<int> parentMethod;
        Func<int> parentMethod2;
        public StationName(ref Pushpin pin, ref DialogHost dialog, ref Map MapWithEvents, Func<int> parentMethodSent, Func<int> parentMethod2Sent)
        {
            currentLocation = pin.Location;
            parentMethod = parentMethodSent;
            parentMethod2 = parentMethod2Sent;
            pushpin = pin;
            hostDialog = dialog;
            map = MapWithEvents;
            InitializeComponent();
        }
        public StationName(Pushpin pin, ref DialogHost dialog, string name)
        {
            map = null;
            pushpin = pin;
            currentLocation = pin.Location;
            hostDialog = dialog;
            InitializeComponent();
            textBoxTrainName.Text = name;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            Database.getOrAddStation(currentLocation);
            Database.setName(currentLocation, textBoxTrainName.Text);
            pushpin.ToolTip = Database.getOrAddStation(pushpin.Location).Name;
            if (map != null)
            {
                map.Children.Add(pushpin);
                pushpin.Content = parentMethod();
                parentMethod2();
            }
            hostDialog.IsOpen = false;
        }
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            hostDialog.IsOpen = false;

        }
    }
}
