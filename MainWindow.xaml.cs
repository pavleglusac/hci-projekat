using HCIProjekat.views.auth;
using HCIProjekat.views.manager;
﻿using HCIProjekat.model;
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

namespace HCIProjekat
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<Button> customerNavigation = new List<Button>();
        private List<Button> managerNavigation = new List<Button>();
        public MainWindow()
        {
            InitializeComponent();
            Database.loadData();
            MainFrame.Content = new Login();
        }

        public MainWindow(model.Train train)
        {
            InitializeComponent();
            MainFrame.Content = new ManagerNavigationLayout(train);
        }

        public MainWindow(Page page)
        {
            InitializeComponent();
            MainFrame.Content = page;
        }

        public void doThings(string param)
        {
        }

        public void CommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            IInputElement focusedControl = FocusManager.GetFocusedElement(Application.Current.Windows[0]);
            if (focusedControl is DependencyObject)
            {
                string str = HelpProvider.GetHelpKey((DependencyObject)focusedControl);
                HelpProvider.ShowHelp(str, this);
            }
        }
    }
}
