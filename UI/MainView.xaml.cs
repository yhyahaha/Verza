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
using Interfaces;

namespace UI
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainView : Window
    {
        public MainView(IControler mainViewModel)
        {
            InitializeComponent();
            this.DataContext = mainViewModel;
        }

        private void buttonClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void s_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine($"I1:height {this.image.ActualHeight} width {this.image.ActualWidth} rate {this.image.ActualHeight / this.image.ActualWidth}");
            Console.WriteLine($"I2:height {this.i2.ActualHeight} width {this.i2.ActualWidth} rate {this.i2.ActualHeight / this.i2.ActualWidth}");
            Console.WriteLine($"I3:height {this.i3.ActualHeight} width {this.i3.ActualWidth} rate {this.i3.ActualHeight / this.i3.ActualWidth}");
        }
    }
}
