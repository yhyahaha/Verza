using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            // Items.ScrapingRects
            Items[imageIndex].ScrapingRects.Clear();
            string templatePath = Path.Combine("Templates", ocrTemplate);
            Items[imageIndex].ReadTemplate(templatePath);

            // OCR実行
            ocrEngine.ClearResults();
            await ocrEngine.RecognizeAsync(this.ImageSource).ConfigureAwait(true);

            // 結果の保存
            Items[imageIndex].OcrAngle = ocrEngine.OcrAngle;





            ShowImageWithScrapingRects();

            Message = ocrEngine.OcrResults.Count.ToString();
        }

        private bool CanExecuteGoOcrCommand() => this.imageSource != null && ocrEngine.CanExecute;

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
            ScrapingRectsImage = tempLoader.GetImageFromTemplate(templatePath);

            //BoundingRectsImage
            if (ocrEngine.OcrResults.Count > 0 && scrapingRectsImage != null)
            {
                int width = ImageSource.PixelWidth;
                int height = ImageSource.PixelHeight;
                BoundingRectsImage = ocrEngine.CreatBoundingRectImage(width,height,97);
            }

            SetButtonsEnabled();
            
        }

        private void ClearImages()
        {
            ImageSource = null;
            ScrapingRectsImage = null;
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
        }

        // Message
        string msgInitial = "Fileを指定してください。";

    }
}
