using HCIProjekat.model;
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
    /// Interaction logic for Registration.xaml
    /// </summary>
    public partial class Registration : Page
    {
        public Registration()
        {
            InitializeComponent();
            usernameField.Focus();
            DataContext = new RegistrationInfo("", "", "", "");
            loginError.Visibility = Visibility.Visible;
        }

        private void Register()
        {
            hideError();
            string username = usernameField.Text;
            string name = nameField.Text;
            string surname = surnameField.Text;
            string password = passwordField.Password;
            string confirmPassword = confirmPasswordField.Password;
            ((TextBox)usernameField).Text = ((TextBox)usernameField).Text;
            ((TextBox)nameField).Text = ((TextBox)nameField).Text;
            ((TextBox)surnameField).Text = ((TextBox)surnameField).Text;

            if (username.Length == 0 || name.Length == 0 || surname.Length == 0) return;
            if (password.Length < 4) showError("Lozinka mora biti duža od 4 karaktera.");
            if (password.Length == 0) showError("Niste uneli lozinku.");

            if (username != null && name != null && surname != null && password != null &&
                username.Length > 0 && name.Length > 0 && surname.Length > 0 && password.Length >= 4)
            {
                if (password == confirmPassword)
                {
                    if (Database.IsExistingUsername(username))
                        showError("Korisničko ime već postoji.");
                    else
                    {
                        Database.SaveUser(new(name, surname, username, password, UserType.CUSTOMER));
                        ShowComponent(new Login());
                        System.Windows.MessageBox.Show(
                            "Uspešno ste se registrovali.",
                            "Registracija uspešna", System.Windows.MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
                else
                    showError("Lozinke se ne poklapaju.");
            }
        }

        private void showError(string text)
        {
            loginError.Text = text;
            loginError.Visibility = Visibility.Visible;
        }

        private void hideError()
        {
            loginError.Text = "";
            loginError.Visibility = Visibility.Hidden;
        }

        private void hangleRegisterKeypress(object sender, KeyEventArgs e)
        {
            hideError();
            if (e.Key == Key.Return)
                Register();
        }

        private void handleRegisterButton(object sender, RoutedEventArgs e)
        {
            Register();
        }

        private void handleChooseLoginButton(object sender, RoutedEventArgs e)
        {
            ShowComponent(new Login());
        }

        private void ShowComponent(object component)
        {
            NavigationService?.Navigate(component);
        }
    }
    public class RegistrationInfo
    {
        public string Username { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string LoginError { get; set; }
        public RegistrationInfo(string username, string name, string surname, string error)
        {
            Username = username;
            Name = name;
            Surname = surname;
            LoginError = error;
        }
    }
}
