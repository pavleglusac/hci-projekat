using HCIProjekat.views.manager.pages;
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

namespace HCIProjekat.views.manager
{
    /// <summary>
    /// Interaction logic for ManagerNavigationLayout.xaml
    /// </summary>
    public partial class ManagerNavigationLayout : Page
    {
        SeatCreator seatCreator;

        public ManagerNavigationLayout()
        {
            InitializeComponent();
            ManagerFrame.Content = new Trains();
        }

        public ManagerNavigationLayout(model.Train train)
        {
            InitializeComponent();
            ManagerFrame.Content = new SeatCreator();
        }

        private void ShowComponent(object component)
        {
            NavigationService?.Navigate(component);
        }
        private void ButtonOpenMenu_Click(object sender, RoutedEventArgs e)
        {
            ButtonCloseMenu.Visibility = Visibility.Visible;
            ButtonOpenMenu.Visibility = Visibility.Collapsed;
        }

        private void ButtonCloseMenu_Click(object sender, RoutedEventArgs e)
        {
            ButtonCloseMenu.Visibility = Visibility.Collapsed;
            ButtonOpenMenu.Visibility = Visibility.Visible;
        }

        private void ListViewMenu_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (((ListViewItem)((ListView)sender).SelectedItem).Name)
            {
                case "ItemTrains":
                    ManagerFrame.Content = new Trains();
                    break;
                case "ItemReports":
                    ManagerFrame.Content = new Report();
                    break;
                case "ItemRideHistory":
                    ManagerFrame.Content = new RideHistory();
                    break;
                case "ItemLogout":
                    model.Database.ClearCurrentUser();
                    ShowComponent(new auth.Login());
                    break;
                default:
                    break;
            }
        }
    }
}
