﻿using System;
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

namespace HCIProjekat.views.customer
{
    /// <summary>
    /// Interaction logic for Timetable.xaml
    /// </summary>
    public partial class Timetable : Page
    {
        public Timetable()
        {
            InitializeComponent();
            DataContext = new TimetableContext();
        }
    }
    public class TimetableContext
    {
        public List<string> Locations { get; set; }

        public TimetableContext()
        {
            Locations = new List<string>()
            {
                "Novi Sad",
                "Beograd",
                "Zrenjanin",
                "Subotica",
                "Sombor",
                "Kikinda",
                "Pančevo"
            };
        }
    }
}
