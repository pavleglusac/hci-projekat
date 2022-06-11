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

namespace HCIProjekat.views.manager.tutorial
{
    /// <summary>
    /// Interaction logic for TimetableTutorial.xaml
    /// </summary>
    public partial class TimetableTutorial : Page
    {
        Train train;
        public string TimeStartInput { get; set; }
        public string TimeEndInput { get; set; }

        public Station From { get; set; }
        public Station To { get; set; }
        public Boolean RemoveEnabled { get; set; }

        public static RoutedCommand UndoCommand = new RoutedCommand();
        public static RoutedCommand RedoCommand = new RoutedCommand();

        public static RoutedCommand SaveCommand = new RoutedCommand();

        public HistoryManager<Timetable> HistoryManager = new HistoryManager<Timetable>();

        Timetable Timetable { get; set; }

        public static RoutedCommand RemoveDeparture = new RoutedCommand();

        public TimetableTutorial()
        {
            InitializeComponent();
            UndoCommand.InputGestures.Add(new KeyGesture(Key.Z, ModifierKeys.Control));
            RedoCommand.InputGestures.Add(new KeyGesture(Key.Y, ModifierKeys.Control));

            SaveCommand.InputGestures.Add(new KeyGesture(Key.S, ModifierKeys.Control));
        }

        ToolTip LastToolTip;
        int StepNum = 1;


        public TimetableTutorial(Train train)
        {
            UndoCommand.InputGestures.Add(new KeyGesture(Key.Z, ModifierKeys.Control));
            RedoCommand.InputGestures.Add(new KeyGesture(Key.Y, ModifierKeys.Control));

            SaveCommand.InputGestures.Add(new KeyGesture(Key.S, ModifierKeys.Control));

            DataContext = this;
            InitializeComponent();
            this.Focus();
            this.train = train;
            Timetable = TutorDatabase.GetTimetableForTrainName(train.Name);
            From = train.GetFirstStation();
            To = train.GetLastStation();
            DepartureLocation.Text = From.Name;
            ArrivalLocation.Text = To.Name;
            HistoryManager.AddEntry(Timetable.Copy());
            timetablesGrid.ItemsSource = Timetable.Departures;
        }

        public void Step1(object sender, EventArgs e)
        {
            RemoveEnabled = false;
            SaveButton.IsEnabled = false;
            UndoButton.IsEnabled = false;
            RedoButton.IsEnabled = false;
            HelpButton.IsEnabled = false;

            var result = MessageBox.Show("Dobrodošli u tutorijal za upravljanje redovima vožnji voza! Kliknite OK da biste nastavili.", "Tutor", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.Yes);

            switch (result)
            {
                case MessageBoxResult.OK:
                    StepNum++;
                    Step2();
                    break;

            }
        }

        public void Step2()
        {
            var result = MessageBox.Show("Probajte da dodate vreme koje nije u konfliktu.", "Tutor", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.Yes);
        }

        public void Step3()
        {
            SaveButton.IsEnabled = false;
            UndoButton.IsEnabled = false;
            RedoButton.IsEnabled = false;
            var result = MessageBox.Show("Čestitamo! Uspešno ste dodali red vožnje!.", "Tutor", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.Yes);
            var result2 = MessageBox.Show("Pokušajte da dodate vreme koje je u konfliktu.", "Tutor", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.Yes);
        }

        public void Step4()
        {
            SaveButton.IsEnabled = false;
            UndoButton.IsEnabled = true;
            RedoButton.IsEnabled = false;
            var result = MessageBox.Show("Čestitamo! Uspešno ste napravili konflikt!.", "Tutor", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.Yes);
            var result2 = MessageBox.Show("Pokušajte da uradite korak unazad pritiskom na dugme Unazad ili" +
                " pritiskom tastera Ctrl + Z!", "Tutor", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.Yes);
        }

        public void Step5()
        {
            UndoButton.IsEnabled = false;
            var result = MessageBox.Show("Čestitamo! Pokušajte korak unapred pritiskom da dugme Unapred ili" +
                " kombinacijom tastera Ctrl + Y!", "Tutor", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.Yes);
        }

        public void Step6()
        {
            SaveButton.IsEnabled = false;
            UndoButton.IsEnabled = false;
            SaveButton.IsEnabled = true;
            var result = MessageBox.Show("Čestitamo! Pokušajte da sačuvate klikom na dugme Sačuvaj ili" +
                " kombinacijom tastera Ctrl + S!", "Tutor", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.Yes);

        }

        public void Step7()
        {
            SaveButton.IsEnabled = false;
            UndoButton.IsEnabled = false;
            SaveButton.IsEnabled = false;
            var result = MessageBox.Show("Čestitamo! Završili ste tutorijal. Ukoliko želite da se nekada pristite funkcionalnosti" +
                ", otvorite Pomoć sistema klikom na taster F1 ili klikom na dugme '?'. \n\nTakođe možete pokrenuti tutorijal koliko god puta želite.", "Tutor", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.Yes);
        }


        public void RemoveDeparture_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            int ind = Timetable.Departures.FindIndex(x => x.DepartureDateTime == (TimeOnly)e.Parameter);
            Timetable.Departures.RemoveAt(ind);
            HistoryManager.AddEntry(Timetable.Copy());
            timetablesGrid.ItemsSource = Timetable.Departures;
            timetablesGrid.Items.Refresh();
            UndoButton.IsEnabled = HistoryManager.CanUndo();
            RedoButton.IsEnabled = HistoryManager.CanRedo();
        }
        private void Undo_Click(object sender, RoutedEventArgs e)
        {
            if(StepNum == 4)
            {
                if (HistoryManager.CanUndo())
                {
                    Timetable = HistoryManager.Undo();
                    timetablesGrid.ItemsSource = Timetable.Departures;
                }
                UndoButton.IsEnabled = HistoryManager.CanUndo();
                RedoButton.IsEnabled = HistoryManager.CanRedo();
                StepNum++;
                Step5();
            }
        }

        private void Redo_Click(object sender, RoutedEventArgs e)
        {
            if (StepNum == 5)
            {
                if (HistoryManager.CanRedo())
                {
                    Timetable = HistoryManager.Redo();
                    timetablesGrid.ItemsSource = Timetable.Departures;
                }
                UndoButton.IsEnabled = HistoryManager.CanUndo();
                RedoButton.IsEnabled = HistoryManager.CanRedo();
                StepNum++;
                Step6();
            }
        }
        public void AddDeparture(object sender, EventArgs e)
        {
            try
            {
                TimeOnly dep = TimeOnly.Parse((string)TimeStartInput);
                TimeOnly arr = TimeOnly.Parse((string)TimeEndInput);
                if (arr <= dep)
                {
                    MessageBox.Show("Polazak mora biti pre dolaska.", "Nevalidni podaci", MessageBoxButton.OK, MessageBoxImage.Error);
                    if(StepNum == 3)
                    {
                        StepNum++;
                        Step4();
                    }
                    return;
                }
                Tuple<Boolean, Departure> conflictResult = DeparturesConflict(dep, arr);
                if (conflictResult.Item1)
                {
                    TimeOnly depStart = conflictResult.Item2.DepartureDateTime;
                    TimeOnly depEnd = conflictResult.Item2.ArrivalDateTime;
                    MessageBox.Show($"Konflikt sa postojećim polaskom! Polazak: {depStart} - {depEnd}", "Nevalidni podaci", MessageBoxButton.OK, MessageBoxImage.Error);
                    if (StepNum == 3)
                    {
                        StepNum++;
                        Step4();
                    }
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
                
                if(StepNum == 2)
                {
                    StepNum++;
                    Step3();
                }

            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"EXCEPTIOOOOOON {ex.Message}");
            }
        }

        public Tuple<Boolean, Departure> DeparturesConflict(TimeOnly start, TimeOnly end)
        {
            foreach (Departure departure in Timetable.Departures)
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
            if(StepNum == 6)
            {
                string message = "Da li ste sigurni da želite da sačuvate redove vožnji?";
                string caption = "Potvrda";

                MessageBoxButton buttons = MessageBoxButton.YesNo;
                MessageBoxImage icon = MessageBoxImage.Question;
                if (MessageBox.Show(message, caption, buttons, icon) == MessageBoxResult.Yes)
                {
                    train.Timetable = Timetable;
                }
                StepNum++;
                Step7();
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
