﻿using System;
using System.Collections.Generic;
using System.IO;
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

namespace HCIProjekat
{
    public partial class HelpViewer : Window
    {
        private JavaScriptControlHelper ch;
        public HelpViewer(string key, MainWindow originator)
        {
            InitializeComponent();
            string curDir = Directory.GetCurrentDirectory();
            System.Diagnostics.Debug.WriteLine($"HELP KEY {key}");
            string path = String.Format("{0}/help/{1}.html", curDir, key);
            if (!File.Exists(path))
            {
                key = "error";
            }
            Uri u = new Uri(String.Format("file:///{0}/help/{1}.html", curDir, key));
            ch = new JavaScriptControlHelper(originator);
            wbHelp.ObjectForScripting = ch;
            wbHelp.Navigate(u);

        }

        private void BrowseBack_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = ((wbHelp != null) && (wbHelp.CanGoBack));
        }

        private void BrowseBack_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            wbHelp.GoBack();
        }

        private void BrowseForward_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = ((wbHelp != null) && (wbHelp.CanGoForward));
        }

        private void BrowseForward_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            wbHelp.GoForward();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
        }

        private void wbHelp_Navigating(object sender, System.Windows.Navigation.NavigatingCancelEventArgs e)
        {
        }


    }
}
