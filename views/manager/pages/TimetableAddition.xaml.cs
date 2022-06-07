using HCIProjekat.model;
using System;
using System.Collections.Generic;
using System.Globalization;
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
    /// Interaction logic for TimetableAddition.xaml
    /// </summary>
    public partial class TimetableAddition : Page
    {
        Train train;
        public string TimeStartInput { get; set; }
        public string TimeEndInput { get; set; }

        public Station From { get; set; }
        public Station To { get; set; }

        public HistoryManager<Timetable> HistoryManager = new HistoryManager<Timetable>();

        Timetable Timetable { get; set; }

        public static RoutedCommand RemoveDeparture = new RoutedCommand();

        public TimetableAddition()
        {
            InitializeComponent();
        }

        public TimetableAddition(Train train)
        {
            InitializeComponent();
            DataContext = this;
            this.train = train;
            Timetable = Database.GetTimetableForTrainName(train.Name);
            From = train.GetFirstStation();
            To = train.GetLastStation();
            DepartureLocation.Text = From.Name;
            ArrivalLocation.Text = To.Name;
            HistoryManager.AddEntry(Timetable.Copy());
            timetablesGrid.ItemsSource = Timetable.Departures;
        }

        public void RemoveDeparture_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            int ind = Timetable.Departures.FindIndex(x => x.DepartureDateTime == (DateTime)e.Parameter);
            Timetable.Departures.RemoveAt(ind);
            HistoryManager.AddEntry(Timetable.Copy());
            timetablesGrid.ItemsSource = Timetable.Departures;
            timetablesGrid.Items.Refresh();
            UndoButton.IsEnabled = HistoryManager.CanUndo();
            RedoButton.IsEnabled = HistoryManager.CanRedo();
        }

        private void Undo_Click(object sender, RoutedEventArgs e)
        {
            if (HistoryManager.CanUndo())
            {
                Timetable = HistoryManager.Undo();
                timetablesGrid.ItemsSource = Timetable.Departures;
            }
            UndoButton.IsEnabled = HistoryManager.CanUndo();
            RedoButton.IsEnabled = HistoryManager.CanRedo();
        }

        private void Redo_Click(object sender, RoutedEventArgs e)
        {
            if (HistoryManager.CanRedo())
            {
                Timetable = HistoryManager.Redo();
                timetablesGrid.ItemsSource = Timetable.Departures;
            }
            UndoButton.IsEnabled = HistoryManager.CanUndo();
            RedoButton.IsEnabled = HistoryManager.CanRedo();
        }

        public void AddDeparture(object sender, EventArgs e)
        {
            try
            {
                TimeOnly dep = TimeOnly.Parse((string)TimeStartInput);
                TimeOnly arr = TimeOnly.Parse((string)TimeEndInput);
                if(arr <= dep)
                {
                    MessageBox.Show("Polazak mora biti pre dolaska.", "Nevalidni podaci", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                Tuple<Boolean, Departure> conflictResult = DeparturesConflict(dep, arr);
                if(conflictResult.Item1)
                {
                    TimeOnly depStart = TimeOnly.FromDateTime(conflictResult.Item2.DepartureDateTime);
                    TimeOnly depEnd = TimeOnly.FromDateTime(conflictResult.Item2.ArrivalDateTime);
                    MessageBox.Show($"Konflikt sa postojećim polaskom! Polazak: {depStart} - {depEnd}", "Nevalidni podaci", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                Timetable.Departures.Add(
                    new Departure
                    {
                        To = To,
                        From = From,
                        DepartureDateTime = DateTime.Parse($"06/08/2022 {dep.Hour}:{dep.Minute}:00"),
                        ArrivalDateTime = DateTime.Parse($"06/08/2022 {arr.Hour}:{arr.Minute}:00"),
                    }
                );

                HistoryManager.AddEntry(Timetable.Copy());
                timetablesGrid.ItemsSource = Timetable.Departures;
                timetablesGrid.Items.Refresh();
                UndoButton.IsEnabled = HistoryManager.CanUndo();
                RedoButton.IsEnabled = HistoryManager.CanRedo();

            }
            catch(Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"EXCEPTIOOOOOON {ex.Message}");
            }
        }

        public Tuple<Boolean, Departure> DeparturesConflict(TimeOnly start, TimeOnly end)
        {
            foreach(Departure departure in Timetable.Departures)
            {
                TimeOnly depStart = TimeOnly.FromDateTime(departure.DepartureDateTime);
                TimeOnly depEnd = TimeOnly.FromDateTime(departure.ArrivalDateTime);
                System.Diagnostics.Debug.WriteLine($"DEPARTURE {depStart} {depEnd} |  {start}  {end} | {start.IsBetween(depStart, depEnd)} {end.IsBetween(depStart, depEnd)} ");
                if (start.IsBetween(depStart, depEnd) || end.IsBetween(depStart, depEnd)) return new Tuple<Boolean, Departure>(true, departure);
                if (start == depStart || start == depEnd || end == depStart || end == depEnd) return new Tuple<Boolean, Departure>(true, departure);
            }
            return new Tuple<Boolean, Departure>(false, null);
        }
    }

}
