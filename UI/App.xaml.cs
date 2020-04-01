using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Interfaces;
using ViewModelControlers;
using UwpOcrForWpfLibrary;

namespace UI
{
    /// <summary>
    /// App.xaml の相互作用ロジック
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            IWindowsOCR engine = new WindowsOCR();
            
            IControler mainViewModel = new MainViewModel(engine); 
            
            this.MainWindow = new MainView(mainViewModel);
            this.MainWindow.Show();
        }
    }
}
