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
                case "ItemTimetable":
                    CustomerFrame.Content = new Timetable();
                    break;
                case "ItemRoutes":
                    CustomerFrame.Content = new RouteMap();
                    break;
                case "ItemTicketHistory":
                    CustomerFrame.Content = new TicketHistory();
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
