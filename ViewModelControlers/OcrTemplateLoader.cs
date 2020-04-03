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

        public WriteableBitmap GetImageFromTemplate(string templatePath,int lineThickness)
        {
            WriteableBitmap bmp;

            using (StreamReader reader = new StreamReader(templatePath))
            {
                string header = reader.ReadLine();
                string[] token = header.Split(',');

                int pixelWidth = int.Parse(token[1]);
                int pixelHeight = int.Parse(token[2]);
                int documentDpi = int.Parse(token[3]);
                double adjustX = double.Parse(token[4]);
                double adjustY = double.Parse(token[5]);

                bmp = new WriteableBitmap(pixelWidth, pixelHeight, deviceDpi, deviceDpi, PixelFormats.Pbgra32,null);

                while (!reader.EndOfStream)
                {
                    string str = reader.ReadLine();
                    string[] info = str.Split(',');

                    int width = (int)double.Parse(info[4]);
                    int height = (int)double.Parse(info[5]);
                    byte[] pixels = GetRectPixels(width, height, lineThickness);

                    if(pixels != null)
                    {
                        Int32Rect rect = new Int32Rect(0, 0, width, height);
                        int stride = width * 4;
                        int codinateLeft = (int)(double.Parse(info[2]) + adjustX);
                        int codinateTop = (int)(double.Parse(info[3]) + adjustY);

                        bmp.WritePixels(rect, pixels, stride, codinateLeft, codinateTop);
                    }
                }
            }

            return bmp;
        }

        private byte[] GetRectPixels(int width, int height, int thickness)
        {
            // width 10 以上, height 10 以上, thickness 1 以上
            if (width < 10) return null;
            if (height < 10) return null;
            if (thickness < 1) return null;
            if (width < thickness * 2) return null;
            if (height < thickness * 2) return null;
            
            // 描画用byte[]
            int pixelsSize = width * height * 4;
            byte[] pixels = new byte[pixelsSize];

            // Left_Line
            for(int x=0; x < pixelsSize; x = x + (width * 4))
            {
                int factor = 4;
                for(int t=0; t < thickness; t++)
                {
                    pixels[x + (t * factor)] = 0;          // Blue
                    pixels[x + 1 + (t * factor)] = 0;      // Green
                    pixels[x + 2 + (t * factor)] = 255;    // Red
                    pixels[x + 3 + (t * factor)] = 255;    // Alpha
                }
            }

            // Top_Line
            for (int x = 0; x < width * 4; x = x + 4)
            {
                int factor = width * 4;
                for (int t = 0; t < thickness; t++)
                {
                    pixels[x + (t * factor)] = 0;          // Blue
                    pixels[x + 1 + (t * factor)] = 0;      // Green
                    pixels[x + 2 + (t * factor)] = 255;    // Red
                    pixels[x + 3 + (t * factor)] = 255;    // Alpha
                }
            }

            // Right_line
            for (int x = (width - 1) * 4 ; x < pixelsSize; x = x + (width * 4))
            {
                int factor = -4;
                for (int t = 0; t < thickness; t++)
                {
                    pixels[x + (t * factor)] = 0;          // Blue
                    pixels[x + 1 + (t * factor)] = 0;      // Green
                    pixels[x + 2 + (t * factor)] = 255;    // Red
                    pixels[x + 3 + (t * factor)] = 255;    // Alpha
                }
            }

            // BottomLine
            for (int x = (width * 4 * (height-1)); x < pixelsSize; x = x + 4)
            {
                int factor = width * -4;
                for (int t = 0; t < thickness; t++)
                {
                    pixels[x + (t * factor)] = 0;          // Blue
                    pixels[x + 1 + (t * factor)] = 0;      // Green
                    pixels[x + 2 + (t * factor)] = 255;    // Red
                    pixels[x + 3 + (t * factor)] = 255;    // Alpha
                }
            }

            return pixels;
        }

        private string errMsgDeviceDpiInvalid= "指定のDeviceDpiが無効です。";
    }
}
