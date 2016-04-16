using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;

namespace DrawBezier
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public IBezierModel BezierModel { get; set; }

        public App()
        {
            BezierModel = new BezierModel();
        }

    }
}
