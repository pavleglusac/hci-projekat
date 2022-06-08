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
            foreach(Ticket ticket in AllTickets)
            {
                System.Diagnostics.Debug.WriteLine($"{ticket.DepartureDate.ToDateTime(ticket.Departure.DepartureDateTime)} {DateTime.Now}");
                if(ticket.DepartureDate.ToDateTime(ticket.Departure.DepartureDateTime) < DateTime.Now)
                {
                    PastTickets.Add(ticket);
                }
                else
                {
                    Tickets.Add(ticket);
                }
            }
        }


        private void ShowTickets()
        {
            ticketHistoryGrid.ItemsSource = PastTickets;
            reservationHistoryGrid.ItemsSource = Tickets;
        }
    }
}
