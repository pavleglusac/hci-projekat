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
    /// Interaction logic for RideHistory.xaml
    /// </summary>
    public partial class Tutor : Page
    {

        public Tutor()
        {
            InitializeComponent();
        }


        private void SeatTutorClick(object sender, EventArgs e)
        {
            MainWindow newWindow = new MainWindow(new SeatCreatorTutorial());
            newWindow.Show();
        }
        private void TrainTutorClick(object sender, EventArgs e)
        {
            MainWindow newWindow = new MainWindow(new AddTrainTutorial());
            newWindow.Show();
        }
        private void TimetableTutorClick(object sender, EventArgs e)
        {
            MainWindow newWindow = new MainWindow(new TimetableTutorial(TutorDatabase.GetTestTrain()));
            newWindow.Show();
        }
    }
}
