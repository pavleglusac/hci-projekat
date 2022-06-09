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

namespace HCIProjekat.views.manager.pages
{
    public partial class Trains : Page
    {
        public List<Train> TrainsData = new List<Train>();

        public static RoutedCommand OpenSeats = new RoutedCommand();
        public static RoutedCommand OpenTimetables = new RoutedCommand();

        public static RoutedCommand OpenUpdate = new RoutedCommand();

        public static RoutedCommand OpenCreate = new RoutedCommand();

        public Frame DialogContent = new Frame();

        public bool IsDialogOpen = true;

        public Trains()
        {
            InitializeComponent();
            TrainsData = Database.Trains;
            trainsGrid.ItemsSource = TrainsData;
            AutoComplete();
        }

        private void handleFilterClick(object sender, EventArgs e)
        {
            TrainsData = Database.SearchTrainsByName(TrainSearchInput.Text);
            trainsGrid.ItemsSource = TrainsData;
        }

        private void AutoComplete()
        {
            TrainSearchInput.ItemsSource = TrainsData.Select(x => {
                ComboBoxItem? cbi = new ComboBoxItem
                {
                    Content = x.Name
                }; return cbi;
            }).ToList();
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
            MainWindow mw = new MainWindow(new UpdateTrain(train));
            mw.ShowDialog();
            
        }

        public void OpenCreateExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            MainWindow mw = new MainWindow(new AddTrain());
            mw.ShowDialog();
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
    }
}
