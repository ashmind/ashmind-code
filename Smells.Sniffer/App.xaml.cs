using System;
using System.Linq;
using System.Windows;

using Caliburn;

namespace AshMind.Code.Smells.Sniffer {
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application {
        public App() {
            CaliburnApplication.Start();
        }
    }
}
