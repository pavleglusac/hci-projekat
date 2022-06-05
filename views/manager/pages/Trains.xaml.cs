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

namespace HCIProjekat.views.manager.pages
{
    /// <summary>
    /// Interaction logic for Trains.xaml
    /// </summary>
    public partial class Trains : Page
    {
        public List<Train> TrainsData = new List<Train>();

        public Trains()
        {
            InitializeComponent();
            trainsGrid.ItemsSource = TrainsData;
            TrainsData = Database.SearchTrainsByName(TrainSearchInput.Text);
            TrainSearchInput.ItemsSource = TrainsData.Select(x => {
                ComboBoxItem? cbi = new ComboBoxItem
                {
                    Content = x.Name
                }; return cbi;
            }).ToList();
        }

        private void handleFilterClick(object sender, EventArgs e)
        {
            TrainsData = Database.SearchTrainsByName(TrainSearchInput.Text);
            trainsGrid.ItemsSource = TrainsData;
        }

        private void AutoComplete(object sender, KeyEventArgs e)
        {
            TrainsData = Database.SearchTrainsByName(TrainSearchInput.Text);
            TrainSearchInput.ItemsSource = TrainsData.Select(x => {
                ComboBoxItem? cbi = new ComboBoxItem
                {
                    Content = x.Name
                }; return cbi; }).ToList();
        }
    }
}
