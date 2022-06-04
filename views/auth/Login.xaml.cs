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
            DataContext = new LoginInfo("", "");
            loginError.Visibility = Visibility.Visible;
        }

        private void login()
        {
            // login logic
            hideError();
            string email = emailField.Text;
            string password = passwordField.Password;
            System.Diagnostics.Debug.WriteLine(email);
            System.Diagnostics.Debug.WriteLine(password);

            ((TextBox)emailField).Text = ((TextBox)emailField).Text;

            //if successful -> transfer to navigation
            if (email == "test" && password == "test")
            {
                ShowComponent(new CustomerNavigationLayout());
            }
            else if (email == "admin" && password == "test")
            {
                ShowComponent(new ManagerNavigationLayout());
            }
            else
            {
                if (emailField.Text.Length > 0)
                {
                    loginError.Text = "Pogrešne informacije.";
                    loginError.Visibility = Visibility.Visible;
                }
            }
        }

        private void handleLoginButton(object sender, RoutedEventArgs e)
        {
            login();   
        }

        private void handleLoginKeypress(object sender, KeyEventArgs e)
        {
            hideError();
            if (e.Key == Key.Return)
            {
                login();
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
        public string Email { get; set; }
        public string LoginError { get; set; }
        public LoginInfo(string email, string error)
        {
            Email = email;
            LoginError = error;
        }
    }
}
