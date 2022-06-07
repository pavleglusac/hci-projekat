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

        public string EndDate { get; set; }
        public string StartDate { get; set; }

        public TimetableAddition()
        {
            InitializeComponent();
        }

        public TimetableAddition(Train train)
        {
            InitializeComponent();
            DataContext = this;
            this.train = train;
        }

    }
    
}
