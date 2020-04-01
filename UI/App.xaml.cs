using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Interfaces;
using ViewModelControlers;

namespace UI
{
    /// <summary>
    /// App.xaml の相互作用ロジック
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            IControler mainViewModel = new MainViewModel(); 
            
            this.MainWindow = new MainView(mainViewModel);
            this.MainWindow.Show();
        }
    }
}
