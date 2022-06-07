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
        public DialogHost kruh;
        public int pinNum;
        public Map map;
        Func<int> MethodNamee;
        public StationName(ref Pushpin pin, ref DialogHost dialog, ref Map MapWithEvents, Func<int> MethodName)
        {
            currentLocation = pin.Location;
            pushpin = pin;
            kruh = dialog;
            map = MapWithEvents;
            MethodNamee = MethodName;
            InitializeComponent();
        }
        public StationName(Pushpin pin, ref DialogHost bruh, Func<int> MethodName)
        {
            currentLocation = pin.Location;
            pushpin = pin;
            kruh = bruh;
            map = null;
            MethodNamee = MethodName;
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Database.getOrAddStation(currentLocation);
            Database.setName(currentLocation, textBoxTrainName.Text);
            pushpin.ToolTip = Database.getOrAddStation(pushpin.Location).Name;
            kruh.IsOpen = false;
            pushpin.Content = MethodNamee();
            if (map != null)
                map.Children.Add(pushpin);
        }
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            kruh.IsOpen = false;

        }
    }
}
