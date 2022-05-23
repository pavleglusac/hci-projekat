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

namespace HCIProjekat
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<Button> customerNavigation = new List<Button>();
        private List<Button> managerNavigation = new List<Button>();
        public MainWindow()
        {
            InitializeComponent();
            customerNavigation.Add(timetableNavButton);
            customerNavigation.Add(linesNavButton);
            customerNavigation.Add(reservationHistoryNavButton);

            managerNavigation.Add(systemControlNavButton);
            managerNavigation.Add(reportNavButton);

            ToggleNavigation(UserType.Customer);
        }

        private void ToggleNavigation(UserType type)
        {
            if (type == UserType.Customer)
            {
                customerNavigation.ForEach(b => b.Visibility = Visibility.Visible);
                managerNavigation.ForEach(b => b.Visibility = Visibility.Collapsed);
            } else
            {
                customerNavigation.ForEach(b => b.Visibility = Visibility.Collapsed);
                managerNavigation.ForEach(b => b.Visibility = Visibility.Visible);
            }
        }

        enum UserType
        {
            Customer,
            Manager
        }
    }
}
