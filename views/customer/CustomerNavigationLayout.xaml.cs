﻿using System;
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

namespace HCIProjekat.views.customer
{
    /// <summary>
    /// Interaction logic for CustomerNavigationLayout.xaml
    /// </summary>
    public partial class CustomerNavigationLayout : Page
    {
        public CustomerNavigationLayout()
        {
            InitializeComponent();
            CustomerFrame.Content = new Timetable();
        }

        private void timetableNavButton_Click(object sender, RoutedEventArgs e)
        {
            CustomerFrame.Content = new Timetable();
        }

        private void linesNavButton_Click(object sender, RoutedEventArgs e)
        {
            CustomerFrame.Content = new RouteMap();
        }

        private void reservationHistoryNavButton_Click(object sender, RoutedEventArgs e)
        {
            CustomerFrame.Content = new ReservationHistory();
        }

        private void logoutButton_Click(object sender, RoutedEventArgs e)
        {
                ShowComponent(new auth.Login());
        }


        private void ShowComponent(object component)
        {
            NavigationService?.Navigate(component);
        }
    }
}
