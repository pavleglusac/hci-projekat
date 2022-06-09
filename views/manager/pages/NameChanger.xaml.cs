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
using System.Windows.Shapes;

namespace HCIProjekat.views.manager.pages
{
    /// <summary>
    /// Interaction logic for NameChanger.xaml
    /// </summary>
    public partial class NameChanger : Window
    {

        public Train Train { get; set; }
        public string Name { get; set; }
        public NameChanger()
        {
            InitializeComponent();
        }

        public NameChanger(Train train)
        {
            DataContext = this;
            InitializeComponent();
            Train = train;
            Name = train.Name;
            NameInput.Text = Name;
        }

        public void SaveButtonClicked(object sender, EventArgs e)
        {
            if(!Validation.GetHasError(NameInput))
            {
                Train.Name = NameInput.Text;
            }
        }

        private void NameInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            SaveButton.IsEnabled = !Validation.GetHasError(NameInput);
        }
    }
}
