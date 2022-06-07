using HCIProjekat.model;
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
            usernameField.Focus();
            DataContext = new LoginInfo("", "");
            loginError.Visibility = Visibility.Visible;
        }

        private void SignIn()
        {
            // login logic
            hideError();
            string username = usernameField.Text;
            string password = passwordField.Password;
            System.Diagnostics.Debug.WriteLine(username);
            System.Diagnostics.Debug.WriteLine(password);

            ((TextBox)usernameField).Text = ((TextBox)usernameField).Text;

            //if successful -> transfer to navigation
            User? user = Database.GetUser(username, password);
            if (user != null)
            {
                Database.SetCurrentUser(user);
                if (user.Type == UserType.CUSTOMER)
                    ShowComponent(new CustomerNavigationLayout());
                else
                    ShowComponent(new ManagerNavigationLayout());
            }
            else
            {
                if (usernameField.Text.Length > 0)
                {
                    loginError.Text = "Pogrešne informacije.";
                    loginError.Visibility = Visibility.Visible;
                }
            }
        }

        private void handleLoginButton(object sender, RoutedEventArgs e)
        {
            SignIn();   
        }

        private void handleLoginKeypress(object sender, KeyEventArgs e)
        {
            hideError();
            if (e.Key == Key.Return)
            {
                SignIn();
            }
        }
        
        private void hideError()
        {
            loginError.Text = "";
            loginError.Visibility = Visibility.Hidden;
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
}
