using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using ViewModel;

namespace ViewModelControlers
{
    partial class MainViewModel
    {
        private void ExecutePickUpFilesCommand()
        {
            TiffImageLoader imageLoader = new TiffImageLoader();
            var list = imageLoader.GetFilesWithPicker();

            if (list.Count == 0) return;

            // Use work directory ,not to change original files
            if (Directory.Exists(workDirectory))
            {
                Directory.Delete(workDirectory, true);
            }
            Directory.CreateDirectory(workDirectory);

            imageLoader.DivideMultiTiffImageAndCopyToWorkDirectory(list, workDirectory);

            Items.Clear();

            DirectoryInfo di = new DirectoryInfo(workDirectory);
            var files = di.GetFiles();
            foreach (var file in files)
            {
                var item = new ImageFileWithOcrResults();
                item.FilePath = file.FullName;
                Items.Add(item);
            }

            Message = $"{ Items.Count } 件のファイルを取得しました。";

            SetButtonsEnabled();
        }

        private bool CanExecutePickupCommand()
        {
            return (Items.Count == 0);
        }

        private void ExecuteNextImageCommand()
        {
            if (imageIndex < (Items.Count - 1))
            {
                ClearImages();
                imageIndex++;
                ShowImageWithScrapingRects();
                ShowFileInfo();
            }
        }

        private bool CanExecuteNextImageCommand() => (0 < Items.Count && imageIndex < (Items.Count - 1));

        private void ExecutePreviousImageCommand()
        {
            if (0 < imageIndex)
            {
                ClearImages();
                imageIndex--;
                ShowImageWithScrapingRects();
                ShowFileInfo();
            }
        }

        private bool CanExecutePreviousImageCommand() => (0 < Items.Count && 0 < imageIndex);

        private async void ExecuteGoOcrCommand()
        {
            // OCR実行
            ocrEngine.ClearResults();
            await ocrEngine.RecognizeAsync(this.ImageSource).ConfigureAwait(true);

            // 結果の保存
            Items[imageIndex].OcrAngle = ocrEngine.OcrAngle;

            int counter = 0;
            var results = ocrEngine.OcrResults;
            foreach(var scrapingRect in Items[imageIndex].ScrapingRects)
            {
                int id = scrapingRect.Id;
                var result = results
                    .Where(res => scrapingRect.Left * OcrParam            <= res.RectLeft &&
                                  scrapingRect.Top * OcrParam             <= res.RectTop &&
                                  res.RectLeft + res.RectWidth * ocrParam <= (scrapingRect.Left + scrapingRect.Width) * ocrParam &&
                                  res.RectTop + res.RectHeight            <= (scrapingRect.Top + scrapingRect.Height) * ocrParam)
                    .OrderBy(x => x.RectLeft).Select(x => x.Words);

                if (result != null)
                {
                    string resultWord = "";                    
                    foreach (var word in result) resultWord += word.Trim();

                    if (resultWord.Length > 0)
                    {
                        var item = Items[imageIndex].ScrapingRects.Where(x => x.Id == id).First();
                        if (item != null) item.Value = resultWord;

                        counter++;
                    }
                }
            }

            ShowImageWithScrapingRects();

            Message = $"{counter}件読み取りました。";
        }

        private bool CanExecuteGoOcrCommand() => this.imageSource != null && ocrEngine.CanExecute;

        private void ExecuteClearOcrCommand()
        {
            foreach(var rect in Items[imageIndex].ScrapingRects)
            {
                rect.Value = "";
            }
            ClearImages();
            ShowImageWithScrapingRects();
            SetButtonsEnabled();
        }

        private bool CanExecuteClearOcrCommand() => this.imageSource != null && ocrEngine.HasResults;

        private void ExecuteRotateRightCommond()
        {
            RotateImage(90.0);
            ShowFileInfo();
        }
        private void ExecuteRotateLeftCommond()
        {
            RotateImage(-90.0);
            ShowFileInfo();
        }

        private void RotateImage(double angle)
        {
            ClearImages();

            string path = Items[imageIndex].FilePath;
            TiffImageLoader loader = new TiffImageLoader();
            loader.RotateTiffImage(path, angle);

            ShowImageWithScrapingRects();
        }

        private bool CanExecuteRotateCommand() => (ImageSource != null);

        private void ExecuteDeleteFileCommand()
        {
            this.ImageSource = null;
            FileInfo fileInfo = new FileInfo(Items[imageIndex].FilePath);
            fileInfo.Delete();
            Items.RemoveAt(imageIndex);

            if (!(imageIndex < Items.Count))
            {
                imageIndex = Items.Count - 1;
            }

            if (Items.Count > 0)
            {
                ShowImageWithScrapingRects();
            }
            else
            {
                ClearImages();
                imageIndex = -1;
            }

            ShowFileInfo();
            SetButtonsEnabled();
        }

        private bool CanExecuteDeleteFileCommand() => Items.Count != 0 && ImageSource != null;

        private void ExecuteFilesClearCommand()
        {
            Items.Clear();
            ClearImages();
            SetButtonsEnabled();
            ShowFileInfo();

            imageIndex = -1;
        }

        private void ShowImageWithScrapingRects()
        {
            // ImageSource
            TiffImageLoader imageLoader = new TiffImageLoader();
            this.ImageSource = imageLoader.CreateBitmapSourceFromPath(Items[imageIndex].FilePath, this.ocrParam);

            // OCR結果をもとにUIの画像を回転
            this.ImageAngle = Items[imageIndex].OcrAngle * -1;

            // ScrapingRectsImage
            var tempLoader = new OcrTemplateLoader(this.deviceDpi);
            var templatePath = Path.Combine("Templates", this.ocrTemplate);
            int lineThickness = 3;
            ScrapingRectsImage = tempLoader.GetImageFromTemplate(templatePath,lineThickness);

            // ImageFileWithOcrResult ScrapingRects の準備
            if (!Items[imageIndex].ScrapingRects.Any())
            {
                Items[imageIndex].ReadScrapingRectsFromTemplate(templatePath);
            }

            // ResultRects
            var scrapingRects = Items[imageIndex].ScrapingRects;
            if (scrapingRects.Any() && ocrEngine.OcrResults.Count > 0 && scrapingRectsImage != null)
            {
                int width = (int)(this.ImageSource.PixelWidth / this.ocrParam);
                int height = (int)(this.ImageSource.PixelHeight / this.ocrParam);
                tempLoader.CreateOcrResultImage(width, height, 97);

                foreach(var rect in scrapingRects)
                {
                    if (string.IsNullOrEmpty(rect.Value)) continue;
                    
                    var rectLeft = (int)rect.Left;
                    var rectTop = (int)rect.Top;
                    var rectWidth = (int)rect.Width;
                    var rectHeight = (int)rect.Height;

                    tempLoader.DrawResultRectOnOcrResultImage(rectLeft, rectTop, rectWidth, rectHeight);
                }
                this.OcrResultImage = tempLoader.GetOcrResultImage();
            }

            // BoundingRectsImage
            if (ocrEngine.OcrResults.Count > 0 && scrapingRectsImage != null)
            {
                int width = ImageSource.PixelWidth;
                int height = ImageSource.PixelHeight;
                BoundingRectsImage = ocrEngine.CreatBoundingRectImage(width, height, 97);
            }

            SetButtonsEnabled();
        }

        private void ClearImages()
        {
            ImageSource = null;
            ScrapingRectsImage = null;
            OcrResultImage = null;
            BoundingRectsImage = null;
            ocrEngine.ClearResults();
        }

        private void ShowFileInfo()
        {
            if(Items.Count == 0)
            {
                Message = msgInitial;
            }
            else
            {
                string filePath = Items[imageIndex].FilePath;
                TiffImageLoader loader = new TiffImageLoader();
                string imageSize = loader.GetFileProperty(filePath, "大きさ");
                string horizontalResolution = loader.GetFileProperty(filePath, "水平方向の解像度");
                string verticalResolution = loader.GetFileProperty(filePath, "垂直方向の解像度");
                string fileType = loader.GetFileProperty(filePath, "項目の種類");
                string fileSize = loader.GetFileProperty(filePath, "サイズ");

                Message = $"{imageIndex + 1} / { Items.Count}" + Environment.NewLine +
                          $"大きさ : {imageSize}" + Environment.NewLine +
                          $"解像度(H/V) : {horizontalResolution} × {verticalResolution}" + Environment.NewLine +
                          $"種類 : {fileType}" + Environment.NewLine +
                          $"サイズ : {fileSize }";
            }
        }

        private void SetButtonsEnabled()
        {
            PickUpFilesCommand?.RaiseCanExecuteChanged();
            NextImageCommand?.RaiseCanExecuteChanged();
            PreviousImageCommand?.RaiseCanExecuteChanged();
            RotateLeftCommond?.RaiseCanExecuteChanged();
            RotateRightCommond?.RaiseCanExecuteChanged();
            DeleteFileCommand?.RaiseCanExecuteChanged();
            goOcrCommand?.RaiseCanExecuteChanged();
            clearOcrCommand?.RaiseCanExecuteChanged();
        }

        public async void ReOcrScrapingRectByPosition(int positionX, int positionY)
        {
            var target = Items[imageIndex].ScrapingRects
                .Where(x => x.Left <= positionX && positionX <= x.Left + x.Width &&
                            x.Top <= positionY && positionY <= x.Top + x.Height).FirstOrDefault();

            if (target == null) return;

            int X = (int)(target.Left * OcrParam);
            int Y = (int)(target.Top * OcrParam);
            int width = (int)(target.Width * OcrParam);
            int height = (int)(target.Height * OcrParam);
            Int32Rect rect = new Int32Rect(X, Y, width, height);

            BitmapSource bmpSource = this.ImageSource;
            CroppedBitmap cropped = new CroppedBitmap(bmpSource, rect);

            BitmapFrame frame = BitmapFrame.Create(cropped);

            ocrEngine.ClearResults();
            await ocrEngine.RecognizeAsync(frame).ConfigureAwait(true);

            if (ocrEngine.HasResults)
            {
                Message = ocrEngine.OcrResults[0].Words;
            }

            



            // recognazeAsync


            // 


               


        }

        // Message
        string msgInitial = "Fileを指定してください。";

    }
}
