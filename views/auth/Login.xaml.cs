using HCIProjekat.views.customer;
using HCIProjekat.views.manager;
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

namespace HCIProjekat.views.auth
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Page
    {
        public Login()
        {
            InitializeComponent();
            emailField.Focus();

        }

        private void handleLogin(object sender, RoutedEventArgs e)
        {
            // login logic
            string email = emailField.Text;
            string password = passwordField.Password;
            System.Diagnostics.Debug.WriteLine(email);
            System.Diagnostics.Debug.WriteLine(password);


            //if successful -> transfer to navigation
            if (email == "test" && password == "test")
            {
                ShowComponent(new CustomerNavigationLayout());
            }
            if (email == "admin" && password == "test")
            {
                ShowComponent(new ManagerNavigationLayout());
            }

        }

        private void handleChooseRegisterButton(object sender, RoutedEventArgs e)
        {
            ShowComponent(new Registration());
        }

        private void ShowComponent(object component)
        {
            NavigationService?.Navigate(component);
        }
    }
}
