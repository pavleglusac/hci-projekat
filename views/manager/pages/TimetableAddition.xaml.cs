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
        public static RoutedCommand UndoCommand = new RoutedCommand();
        public static RoutedCommand RedoCommand = new RoutedCommand();

        public static RoutedCommand SaveCommand = new RoutedCommand();

        public TimetableAddition()
        {
            InitializeComponent();
            this.Focus();
            UndoCommand.InputGestures.Add(new KeyGesture(Key.Z, ModifierKeys.Control));
            RedoCommand.InputGestures.Add(new KeyGesture(Key.Y, ModifierKeys.Control));
            SaveCommand.InputGestures.Add(new KeyGesture(Key.S, ModifierKeys.Control));
        }

        public TimetableAddition(Train train)
        {
            InitializeComponent();
            UndoCommand.InputGestures.Add(new KeyGesture(Key.Z, ModifierKeys.Control));
            RedoCommand.InputGestures.Add(new KeyGesture(Key.Y, ModifierKeys.Control));
            SaveCommand.InputGestures.Add(new KeyGesture(Key.S, ModifierKeys.Control));
            this.Focus();
            DataContext = this;
            this.train = train;
            Timetable = Database.GetTimetableForTrainName(train.Name).Copy();
            From = train.GetFirstStation();
            To = train.GetLastStation();
            DepartureLocation.Text = From.Name;
            ArrivalLocation.Text = To.Name;
            HistoryManager.AddEntry(Timetable.Copy());
            timetablesGrid.ItemsSource = Timetable.Departures;
            SetHelpKey(null, null);
        }


        public void RemoveDeparture_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var result = MessageBox.Show("Da li ste sigurni? Ova akcija je nepovratna i obrisaće karte vezane za dati red vožnje pri čuvanju." +
                " Karte neće biti obrisane ukoliko nakon ovoga dodate red vožnje koji će obuhvatati obrisane.", "Potvrda", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if(result == MessageBoxResult.Yes)
            {
                int ind = Timetable.Departures.FindIndex(x => x.DepartureDateTime == (TimeOnly)e.Parameter);
                Timetable.Departures.RemoveAt(ind);
                HistoryManager.AddEntry(Timetable.Copy());
                timetablesGrid.ItemsSource = Timetable.Departures;
                timetablesGrid.Items.Refresh();
                UndoButton.IsEnabled = HistoryManager.CanUndo();
                RedoButton.IsEnabled = HistoryManager.CanRedo();
            }
        }

        private void Undo_Click(object sender, RoutedEventArgs e)
        {
            if (HistoryManager.CanUndo())
            {
                Timetable = HistoryManager.Undo().Copy();
                timetablesGrid.ItemsSource = Timetable.Departures;
            }
            UndoButton.IsEnabled = HistoryManager.CanUndo();
            RedoButton.IsEnabled = HistoryManager.CanRedo();
        }

        private void Redo_Click(object sender, RoutedEventArgs e)
        {
            if (HistoryManager.CanRedo())
            {
                Timetable = HistoryManager.Redo().Copy();
                timetablesGrid.ItemsSource = Timetable.Departures;
            }
            UndoButton.IsEnabled = HistoryManager.CanUndo();
            RedoButton.IsEnabled = HistoryManager.CanRedo();
        }

        public void AddDeparture(object sender, EventArgs e)
        {
            try
            {
                if (Validation.GetHasError(TimeStart) || Validation.GetHasError(TimeEnd))
                {
                    MessageBox.Show("Vremena nisu validna.", "Nevalidni podaci.", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                };

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
                    TimeOnly depStart = conflictResult.Item2.DepartureDateTime;
                    TimeOnly depEnd = conflictResult.Item2.ArrivalDateTime;
                    MessageBox.Show($"Konflikt sa postojećim polaskom! Polazak: {depStart} - {depEnd}", "Nevalidni podaci", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                Timetable.Departures.Add(
                    new Departure
                    {
                        To = To,
                        From = From,
                        DepartureDateTime = TimeOnly.Parse($"{dep.Hour}:{dep.Minute}:00"),
                        ArrivalDateTime = TimeOnly.Parse($"{arr.Hour}:{arr.Minute}:00"),
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
                TimeOnly depStart = departure.DepartureDateTime;
                TimeOnly depEnd = departure.ArrivalDateTime;
                System.Diagnostics.Debug.WriteLine($"DEPARTURE {depStart} {depEnd} |  {start}  {end} | {start.IsBetween(depStart, depEnd)} {end.IsBetween(depStart, depEnd)} ");
                if (start.IsBetween(depStart, depEnd) || end.IsBetween(depStart, depEnd)) return new Tuple<Boolean, Departure>(true, departure);
                if (start == depStart || start == depEnd || end == depStart || end == depEnd) return new Tuple<Boolean, Departure>(true, departure);
            }
            return new Tuple<Boolean, Departure>(false, null);
        }

        public void Save_Click(object sender, EventArgs e)
        {
            string message = "Da li ste sigurni da želite da sačuvate redove vožnji?";
            string caption = "Potvrda";

            MessageBoxButton buttons = MessageBoxButton.YesNo;
            MessageBoxImage icon = MessageBoxImage.Question;
            if (MessageBox.Show(message, caption, buttons, icon) == MessageBoxResult.Yes)
            {
                train.Timetable = Timetable;
                MessageBox.Show("Sačuvano!", "Potvrda", MessageBoxButton.OK, MessageBoxImage.Information);
                Database.RemoveDanglingTickets();
            }
        }

        public void Help_Click(object sender, EventArgs e)
        {
            var wnd = (MainWindow)Window.GetWindow(this);
            wnd.CommandBinding_Executed(sender, null);
        }

        public void SetHelpKey(object sender, EventArgs e)
        {
            IInputElement focusedControl = FocusManager.GetFocusedElement(Application.Current.Windows[0]);
            if (focusedControl is DependencyObject)
            {
                HelpProvider.SetHelpKey((DependencyObject)focusedControl, "timetables");
            }
        }

        private void UndoCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Undo_Click(sender, e);
        }

        private void RedoCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Redo_Click(sender, e);
        }

        private void SaveCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Save_Click(sender, e);
        }

    }

}
