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
        }

        private void GetTickets()
        {
            Tickets = Database.GetCurrentUsersTickets();
        }
        private void ShowTickets()
        {
            ticketHistoryGrid.ItemsSource = Tickets;
        }
    }
}
