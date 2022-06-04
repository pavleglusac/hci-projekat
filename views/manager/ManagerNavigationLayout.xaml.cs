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
        pages.TrainAddition trainAddition;

        public ManagerNavigationLayout()
        {
            InitializeComponent();
            ManagerFrame.Content = new pages.SystemManagment();
            model.Database.loadData();
        }


        private void logoutButton_Click(object sender, RoutedEventArgs e)
        {
            ShowComponent(new auth.Login());
        }

        private void ShowComponent(object component)
        {
            NavigationService?.Navigate(component);
        }

        private void systemControlNavButton_Click(object sender, RoutedEventArgs e)
        {
            ManagerFrame.Content = new pages.SystemManagment();
        }

        private void reportNavButton_Click(object sender, RoutedEventArgs e)
        {
            ManagerFrame.Content = new pages.Report();

        }

        private void trainNavButton_Click(object sender, RoutedEventArgs e)
        {
            if(trainAddition == null)
            {
                model.Train train = model.Database.Trains[0];
                trainAddition = new pages.TrainAddition(train);
            }
            ManagerFrame.Content = trainAddition;
        }
    }
}
