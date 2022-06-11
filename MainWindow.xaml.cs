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
using System.ComponentModel;
using HCIProjekat.views.manager.pages;

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
            TutorDatabase.loadData();
            MainFrame.Content = new Login();
        }

        public MainWindow(model.Train train)
        {
            InitializeComponent();
            this.Activate();
            MainFrame.Content = new ManagerNavigationLayout(train);
        }

        public MainWindow(Page page)
        {
            InitializeComponent();
            MainFrame.Content = page;
            WindowState = WindowState.Normal;
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
        protected override void OnClosing(CancelEventArgs e)
        {
            var content = MainFrame.Content;
            if(content is SeatCreator)
            {
                var res = MessageBox.Show("Da li ste sigurni da želite da zatvorite prozor? Možete izgubiti podatke ukoliko niste sačuvali.", "Potvrda", MessageBoxButton.YesNo, MessageBoxImage.Exclamation);
                if(res == MessageBoxResult.Yes)
                {
                    base.OnClosing(e);
                }
                else
                {
                    e.Cancel = true;
                    return;
                }
            }

            if(content is TimetableAddition)
            {
                var res = MessageBox.Show("Da li ste sigurni da želite da zatvorite prozor? Možete izgubiti podatke ukoliko niste sačuvali.", "Potvrda", MessageBoxButton.YesNo, MessageBoxImage.Exclamation);
                if (res == MessageBoxResult.Yes)
                {
                    base.OnClosing(e);
                }
                else
                {
                    e.Cancel = true;
                    return;
                }
            }

        }

    }
}
