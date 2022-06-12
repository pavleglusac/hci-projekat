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
using MaterialDesignThemes.Wpf;
using System.Threading;

namespace HCIProjekat.views.manager.pages
{
    public partial class Trains : Page
    {
        public List<Train> TrainsData = new List<Train>();

        public static RoutedCommand OpenSeats = new RoutedCommand();
        public static RoutedCommand OpenTimetables = new RoutedCommand();

        public static RoutedCommand OpenUpdate = new RoutedCommand();

        public static RoutedCommand OpenCreate = new RoutedCommand();

        public static RoutedCommand OpenDelete = new RoutedCommand();

        public Frame DialogContent = new Frame();

        public bool IsDialogOpen = true;

        public Trains()
        {
            InitializeComponent();
            TrainsData = Database.Trains.Where(x => !x.Deleted).ToList();
            trainsGrid.ItemsSource = TrainsData.Select(x => new GridEntry(x.Name, Database.CantBeDeleted(x))); ;
            AutoComplete();
        }

        private void handleFilterClick(object sender, EventArgs e)
        {
            TrainsData = Database.SearchTrainsByName(TrainSearchInput.Text).Where(x => !x.Deleted).ToList();
            trainsGrid.ItemsSource = TrainsData.Select(x => new GridEntry(x.Name, Database.CantBeDeleted(x))); ;
        }

        public int refreshItems()
        {
            TrainsData = Database.SearchTrainsByName(TrainSearchInput.Text);
            trainsGrid.ItemsSource = TrainsData.Select(x => new GridEntry(x.Name, Database.CantBeDeleted(x)));
            return 0;
        }

        private void AutoComplete()
        {
            TrainSearchInput.ItemsSource = TrainsData.Select(x => x.Name);
        }

        public void OpenSeatsExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            Train train = Database.GetTrainByName((string)e.Parameter);
            MainWindow mw = new MainWindow(new SeatCreator(train));
            mw.ShowDialog();
        }
        public void OpenUpdateExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            Train train = Database.GetTrainByName((string)e.Parameter);
            UpdateTrain updateTrain = new UpdateTrain(train);
            MainWindow mw = new MainWindow(updateTrain);
            updateTrain.thisWindow = mw;
            mw.ShowDialog();
            
        }

        public void OpenCreateExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            AddTrain addTrain = new AddTrain();
            MainWindow mw = new MainWindow(addTrain);
            addTrain.thisWindow = mw;
            addTrain.callOnClose = refreshItems;
            mw.ShowDialog();
        }

        public void OpenDeleteExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            var result = MessageBox.Show("Da li ste sigurni da želite da obrišete voz?", $"Brisanje voza: {(string)e.Parameter}", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if(result == MessageBoxResult.Yes)
            {
                var result2 = MessageBox.Show("Da li ste sasvim sigurni da želite da obrišete voz? Ova akcija je nepovratna!", $"Brisanje voza: {(string)e.Parameter}", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if(result2 == MessageBoxResult.Yes)
                {
                    Database.GetTrainByName((string)e.Parameter).Deleted = true;
                    MessageBox.Show("Voz je obrisan!", $"Brisanje voza: {(string)e.Parameter}", MessageBoxButton.OK, MessageBoxImage.Information);
                    TrainsData = Database.Trains.Where(x => !x.Deleted).ToList();
                    trainsGrid.ItemsSource = TrainsData.Select(x => new GridEntry(x.Name, Database.CantBeDeleted(x))); ;
                    AutoComplete();
                }
            }
        }

        public void Refresh(object sender, EventArgs args)
        {
            TrainsData.ForEach(x => System.Diagnostics.Debug.WriteLine($"{x.Name} "));
            trainsGrid.Items.Refresh();
            AutoComplete();
        }

        public void OpenTimetablesExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            Train train = Database.GetTrainByName((string)e.Parameter);
            MainWindow mw = new MainWindow(new TimetableAddition(train));
            mw.ShowDialog();
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
                HelpProvider.SetHelpKey((DependencyObject)focusedControl, "index");
            }
        }


        public void NameChangeExecuted(object sender, EventArgs e)
        {
            Train train = Database.GetTrainByName(((TextBlock)sender).Tag.ToString());
            NameChanger nameChanger = new NameChanger(train);
            nameChanger.ShowDialog();

            trainsGrid.Items.Refresh();
            this.Focus();
        }

        class GridEntry
        {
            public string Name { get; set; }
            public Boolean CanBeDeleted { get; set; }
            public GridEntry (string name, bool cantBeDeleted)
            {
                Name = name;
                CanBeDeleted = !cantBeDeleted;
                System.Diagnostics.Debug.Write($"CAN BE DELETED {cantBeDeleted}");
            }
        }
    }
}
