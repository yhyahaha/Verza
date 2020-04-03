using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Interfaces;
using System.Windows.Media;
using System.Windows;
using System.Windows.Media.Imaging;

// UWPのOCR、SoftWareBitmap
using UwpLanguage = Windows.Globalization.Language;
using UwpOcrEngine = Windows.Media.Ocr.OcrEngine;
using UwpOcrResult = Windows.Media.Ocr.OcrResult;
using UwpSoftwareBitmap = Windows.Graphics.Imaging.SoftwareBitmap;


namespace UwpOcrForWpfLibrary
{
    public class WindowsOCR : IWindowsOCR
    {
        UwpOcrEngine engine;
        UwpLanguage ocrLanguage;

        public WindowsOCR()
        {
            // Engine を既定の言語で初期化する。
            engine = UwpOcrEngine.TryCreateFromUserProfileLanguages();
            if (engine == null) throw new InvalidOperationException(errMsgFailedToConstract);

            // Propertyの初期化
            ocrLanguage = new UwpLanguage(engine.RecognizerLanguage.LanguageTag);
            ocrAngle = 0.0;
            ocrResults = new List<WinOcrResult>();
        }

        public string OcrLanguage
        {
            get
            {
                if (ocrLanguage == null) return string.Empty;
                OcrLanguages langus = new OcrLanguages();
                return langus.GetLanguage(ocrLanguage.LanguageTag);
            }
        }

        private double ocrAngle;
        public double OcrAngle
        {
            get { return ocrAngle; }
        }

        private IList<WinOcrResult> ocrResults;
        public IList<WinOcrResult> OcrResults
        {
            get { return ocrResults; }
        }

        public bool HasResults => ocrResults.Any();

        public bool CanExecute => engine != null && this.ocrLanguage != null;

        public void SetOcrLanguage(string language)
        {
            OcrLanguages langus = new OcrLanguages();
            string tag = langus.GetTag(language);
            
            ocrLanguage = new UwpLanguage(tag);
            engine = UwpOcrEngine.TryCreateFromLanguage(ocrLanguage);

            if (engine == null)
            {
                ocrLanguage = null;
                throw new InvalidOperationException(errMsgFailedToSetLanguage);
            }
        }

        public IList<string> GetAvailableLanguages()
        {
            OcrLanguages langus = new OcrLanguages();
            
            IList<string> languages = new List<string>();
            var list = UwpOcrEngine.AvailableRecognizerLanguages;
            if(list.Count != 0)
            {
                foreach(var item in list)
                {
                    var lang = langus.GetLanguage(item.LanguageTag);
                    languages.Add(lang); 
                }
            }
            return languages;
        }

        public async Task RecognizeAsync(BitmapFrame bitmapFrame)
        {
            if (!CanExecute) return;
            ClearResults();
            
            // SoftwareBitmapを取得
            UwpSoftwareBitmap bitmap
                = await UwpSoftwareBitmapHelper.ConvertFrom(bitmapFrame).ConfigureAwait(true);
            if (bitmap == null) return;

            // OCR実行
            UwpOcrResult ocrResult = await engine.RecognizeAsync(bitmap);
            bitmap.Dispose();

            // Angle
            ocrAngle = ocrResult.TextAngle ?? 0.0;

            // Line
            foreach (var ocrLine in ocrResult.Lines)
            {
                WinOcrResult result = new WinOcrResult();

                string words = "";
                double left = ocrLine.Words[0].BoundingRect.Left;
                double right = ocrLine.Words[0].BoundingRect.Right;
                double top = ocrLine.Words[0].BoundingRect.Top;
                double bottom = ocrLine.Words[0].BoundingRect.Bottom;
                
                foreach (var word in ocrLine.Words)
                {
                    words += word.Text;

                    if (word.BoundingRect.Left < left) left = word.BoundingRect.Left;
                    if (right < word.BoundingRect.Right) right = word.BoundingRect.Right;
                    if (word.BoundingRect.Top < top) top = word.BoundingRect.Top;
                    if (bottom < word.BoundingRect.Bottom) bottom = word.BoundingRect.Bottom;
                }

                result.Words = words;
                result.RectLeft = left;
                result.RectTop = top;
                result.RectWidth = right - left;
                result.RectHeight = bottom - top;

                this.ocrResults.Add(result);
            }   
        }

        public BitmapSource CreatBoundingRectImage(int imageWidth, int imageHeight, int deviceDpi)
        {
            var bitmap = new WriteableBitmap(imageWidth, imageHeight, deviceDpi, deviceDpi, PixelFormats.Pbgra32, null);

            if (!this.ocrResults.Any()) return bitmap;

            foreach(var result in this.ocrResults)
            {
                int left = (int)result.RectLeft;
                int top = (int)result.RectTop;
                int width = (int)result.RectWidth;
                int height = (int)result.RectHeight;
                int stride = (int)result.RectWidth * 4;

                var pixels = GetRectPixels(width, height);
                Int32Rect rect = new Int32Rect(0, 0, width, height);                
                bitmap.WritePixels(rect, pixels, stride, left, top);
            }

            bitmap.Freeze();

            return bitmap;
        }


        private byte[] GetRectPixels(int width, int height)
        {
            int pixelSize = width * height * 4;
            byte[] pixels = new byte[pixelSize];

            for(int x=0; x < pixelSize; x = x + 4)
            {
                pixels[x] = 0;      // Blue
                pixels[x+1] = 255;  // Green
                pixels[x+2] = 0;    // Red
                pixels[x+3] = 60;  // Alpha
            }

            return pixels;
        }


        public void ClearResults()
        {
            ocrAngle = 0.0;
            ocrResults.Clear();
        }


        string errMsgFailedToConstract = "TryCreateFromUserProfileLanguagesメソッドでの初期化に失敗しました。";
        string errMsgFailedToSetLanguage = "OcrEngineの言語設定で利用できない言語が指定されました。";
    }
}
