using HCIProjekat.model;
using HCIProjekat.views.customer.dialogs;
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
        List<Ticket> PastTickets = new List<Ticket>();

        public TicketHistory()
        {
            InitializeComponent();
            GetTickets();
            ShowTickets();
        }

        private void GetTickets()
        {
            List<Ticket> AllTickets = Database.GetCurrentUsersTickets();

            PastTickets = AllTickets.FindAll(x => x.DepartureDate.ToDateTime(x.Departure.DepartureDateTime) < DateTime.Now);
            Tickets = AllTickets.FindAll(x => x.DepartureDate.ToDateTime(x.Departure.DepartureDateTime) >= DateTime.Now);

            if (Tickets.Any()) reservationsComponent.Visibility = Visibility.Visible;
            else reservationsComponent.Visibility = Visibility.Collapsed;
        }


        private void ShowTickets()
        {
            ticketHistoryGrid.ItemsSource = PastTickets;
            reservationHistoryGrid.ItemsSource = Tickets;
        }
    }
}
