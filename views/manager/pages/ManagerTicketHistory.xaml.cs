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

namespace HCIProjekat.views.manager.pages
{
    /// <summary>
    /// Interaction logic for ReservationHistory.xaml
    /// </summary>
    public partial class ManagerTicketHistory : Page
    {
        List<Ticket> Tickets = new List<Ticket>();
        List<Ticket> PastTickets = new List<Ticket>();
        Train Train { get; set; }
        Departure Departure { get; set; }
        DateOnly Date { get; set; }

        public ManagerTicketHistory(Train train, Departure departure, DateOnly date)
        {
            InitializeComponent();
            Train = train;
            Departure = departure;
            Date = date;
            GetTickets();
            ShowTickets();
        }

        private void GetTickets()
        {
            List<Ticket> AllTickets = Database.Tickets
                .Where(x => x.Train.Name == Train.Name)
                .Where(x => x.DepartureDate == Date)
                .Where(x => x.Departure.From == Departure.From)
                .Where(x => x.Departure.To == Departure.To)
                .Where(x => x.Departure.DepartureDateTime == Departure.DepartureDateTime)
                .Where(x => x.Departure.ArrivalDateTime == Departure.ArrivalDateTime)
                .ToList();

            PastTickets = AllTickets.FindAll(x => x.DepartureDate.ToDateTime(x.Departure.DepartureDateTime) < DateTime.Now);
            Tickets = AllTickets.FindAll(x => x.DepartureDate.ToDateTime(x.Departure.DepartureDateTime) >= DateTime.Now);


            if (Tickets.Any())
            {
                ActiveTicketsComponent.Visibility = Visibility.Visible;
            }
            else
            {
                ActiveTicketsComponent.Visibility = Visibility.Collapsed;
            }

            if (PastTickets.Any())
            {
                PastTicketsComponent.Visibility = Visibility.Visible;
            }
            else
            {
                PastTicketsComponent.Visibility = Visibility.Collapsed;
            }


        }


        private void ShowTickets()
        {
            ticketHistoryGrid.ItemsSource = PastTickets;
            reservationHistoryGrid.ItemsSource = Tickets;
        }
    }
}
