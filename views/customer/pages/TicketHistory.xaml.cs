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

namespace HCIProjekat.views.customer
{
    /// <summary>
    /// Interaction logic for ReservationHistory.xaml
    /// </summary>
    public partial class TicketHistory : Page
    {
        List<Ticket> Tickets = new List<Ticket>();
        public TicketHistory()
        {
            InitializeComponent();
            GetTickets();
            ShowTickets();
            ShowReservations();
        }


        private void ShowReservations()
        {
            reservationHistoryGrid.ItemsSource = Tickets.FindAll(x => x.Status == TicketStatus.RESERVED);
            if (Tickets.Any(x => x.Status == TicketStatus.RESERVED))
                reservationsComponent.Visibility = Visibility.Visible;
            else
                reservationsComponent.Visibility = Visibility.Collapsed;
        }

        private void GetTickets()
        {
            Tickets = Database.GetCurrentUsersTickets();
        }
        private void ShowTickets()
        {
            ticketHistoryGrid.ItemsSource = Tickets.FindAll(x => x.Status == TicketStatus.BOUGHT);
        }

        private void CancelReservationButtonClick(object sender, RoutedEventArgs e)
        {
            Ticket reservation = (Ticket)((Button)e.Source).DataContext;
            MessageBoxResult messageBoxResult =
                MessageBox.Show("Da li ste sigurni da želite poništiti rezervaciju?",
                "Poništavanje rezervacije", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (messageBoxResult == MessageBoxResult.Yes)
                CancelReservation(reservation);
        }
        
        private void CancelReservation(Ticket reservation)
        {
            Database.DeleteReservation(reservation);
            GetTickets();
            ShowReservations();
        }
    }
}
