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
            GenerateTickets();
        }

        private void GenerateTickets()
        {
            List<Departure> Departures = new List<Departure>();
            Dictionary<Station, int> Stations = new Dictionary<Station, int>();
            Stations.Add(new Station("Novi Sad"), 0);
            Stations.Add(new Station("Zrenjanin"), 1);
            Stations.Add(new Station("Beograd"), 2);
            Stations.Add(new Station("Subotica"), 3);
            Train train = new Train("Soko X", Stations, 10);
            for (int i = 0; i < 10; i++)
            {
                Departures.Add(new Departure(DateTime.Parse($"2022-06-01T0{i % 5}:0{(i * 23) % 10}"),
                    DateTime.Parse($"2022-06-01T0{i % 3 + 5}:0{(i * 27) % 10}"), Stations.Keys.ToList()[i % 3], Stations.Keys.ToList()[(i + 1) % 3]));
            }
            train.Timetable = Departures;
            for (int i = 0; i < 13; i++)
            {
                Tickets.Add(new Ticket(train, train.Timetable[i % 10]));
            }
            ticketHistoryGrid.ItemsSource = Tickets;
        }
    }
}
