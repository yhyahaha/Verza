using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.IO;

namespace ViewModelControlers
{
    class OcrTemplateLoader
    {
        int deviceDpi;

        /// <summary>
        /// デバイスのDpiを指定して初期化
        /// </summary>
        /// <param name="deviceDpi"></param>
        public OcrTemplateLoader(int deviceDpi)
        {
            if (deviceDpi < 1)
            {
                throw new InvalidOperationException(errMsgDeviceDpiInvalid);
            }
            this.deviceDpi = deviceDpi;
        }

        public RenderTargetBitmap GetImageFromTemplate(string templatePath)
        {
            RenderTargetBitmap bmp;

            using (StreamReader reader = new StreamReader(templatePath))
            {
                string header = reader.ReadLine();
                string[] token = header.Split(',');

                int pixelWidth = int.Parse(token[1]);
                int pixelHeight = int.Parse(token[2]);
                int documentDpi = int.Parse(token[3]);
                double adjustX = double.Parse(token[4]);
                double adjustY = double.Parse(token[5]);

                bmp = new RenderTargetBitmap(pixelWidth, pixelHeight, deviceDpi, deviceDpi, PixelFormats.Pbgra32);

                DrawingVisual drawingVisual = new DrawingVisual();
                DrawingContext drawingContext = drawingVisual.RenderOpen();
                Pen pen = new Pen(Brushes.Red, 5.0);

                while (!reader.EndOfStream)
                {
                    string str = reader.ReadLine();
                    string[] info = str.Split(',');

                    double codinateLeft = (double.Parse(info[2]) + adjustX) ;
                    double codinateTop = (double.Parse(info[3]) + adjustY) ;
                    double width = double.Parse(info[4]);
                    double height = double.Parse(info[5]);
                    Rect rect = new Rect(codinateLeft, codinateTop, width, height);

                    drawingContext.DrawRectangle(null, pen, rect);
                }
                drawingContext.Close();

                bmp.Render(drawingVisual);
            }

            return bmp;
        }

        private string errMsgDeviceDpiInvalid= "指定のDeviceDpiが無効です。";
    }
}
