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

        public Frame DialogContent = new Frame();

        public bool IsDialogOpen = true;

        public Trains()
        {
            InitializeComponent();
            trainsGrid.ItemsSource = TrainsData;
            AutoComplete();
        }


        private void handleFilterClick(object sender, EventArgs e)
        {
            TrainsData = Database.SearchTrainsByName(TrainSearchInput.Text);
            trainsGrid.ItemsSource = TrainsData;
            TrainsData = Database.SearchTrainsByName(TrainSearchInput.Text);
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
            DialogContent.Content = new TrainAddition(train);
            DialogContent.Height = 640;
            DialogContent.Width = 800;
            IsDialogOpen = true;
            TrainsDialogHost.DialogContent = DialogContent;
            TrainsDialogHost.CloseOnClickAway = true;
            TrainsDialogHost.ShowDialog(DialogContent);
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
            DialogContent.Content = new TimetableAddition(train);
            DialogContent.Height = 640;
            DialogContent.Width = 800;
            IsDialogOpen = true;
            TrainsDialogHost.DialogContent = DialogContent;
            TrainsDialogHost.CloseOnClickAway = true;
            TrainsDialogHost.ShowDialog(DialogContent);
        }
    }
}
