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

        private void scrapingImage_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount >= 2)
            {
                var point = e.GetPosition(this.scrapingImage);
                var imageWidth = (this.scrapingImage.Source as BitmapSource).PixelWidth;
                var imageHeight = (this.scrapingImage.Source as BitmapSource).PixelHeight;

                int X = (int)(point.X / this.scrapingImage.ActualWidth * imageWidth);
                int Y = (int)(point.Y / this.scrapingImage.ActualHeight * imageHeight);

                (this.DataContext as IControler).ReOcrScrapingRectByPosition(X,Y);
            }
        }

        private void checkBoxScrapingRects_Checked(object sender, RoutedEventArgs e)
        {
            this.scrapingImage.Visibility = Visibility.Visible;
            this.ocrResultImage.Visibility = Visibility.Visible;
        }

        private void checkBoxScrapingRects_Unchecked(object sender, RoutedEventArgs e)
        {
            this.scrapingImage.Visibility = Visibility.Collapsed;
            this.ocrResultImage.Visibility = Visibility.Collapsed;
        }

        private void checkBoxBoundingRects_Checked(object sender, RoutedEventArgs e)
        {
            this.boundingImage.Visibility = Visibility.Visible;
        }

        private void checkBoxBoundingRects_Unchecked(object sender, RoutedEventArgs e)
        {
            this.boundingImage.Visibility = Visibility.Collapsed;
        }
    }
}
