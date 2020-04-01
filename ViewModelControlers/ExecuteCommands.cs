using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
                ClearImage();
                imageIndex++;
                ShowImageWithScrapingRects();
            }
        }

        private bool CanExecuteNextImageCommand() => (0 < Items.Count && imageIndex < (Items.Count - 1));

        private void ExecutePreviousImageCommand()
        {
            if (0 < imageIndex)
            {
                ClearImage();
                imageIndex--;
                ShowImageWithScrapingRects();
            }
        }

        private bool CanExecutePreviousImageCommand() => (0 < Items.Count && 0 < imageIndex);

        private void ExecuteRotateRightCommond()
        {
            RotateImage(90.0);
        }
        private void ExecuteRotateLeftCommond()
        {
            RotateImage(-90.0);
        }

        private void RotateImage(double angle)
        {
            ClearImage();

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
                ImageSource = null;
                imageIndex = -1;
                Message = "";
            }

            SetButtonsEnabled();
        }

        private bool CanExecuteDeleteFileCommand() => Items.Count != 0 && ImageSource != null;

        private void ExecuteFilesClearCommand()
        {
            Items.Clear();
            ImageSource = null;
            imageIndex = -1;
            SetButtonsEnabled();

            Message = msgReset;
        }

        private void ShowImageWithScrapingRects()
        {
            double ocrParam = 0.6;
            
            // ImageSource
            TiffImageLoader imageLoader = new TiffImageLoader();
            this.ImageSource = imageLoader.CreateBitmapSourceFromPath(Items[imageIndex].FilePath, ocrParam);

            //// OCR結果をもとにUIの画像を回転
            this.ImageAngle = Items[imageIndex].OcrAngle * -1;

            //// ScrapingRectsImage
            //if (!OcrItems[imageIndex].ScrapingRects.Any())
            //{
            //    OcrItems[imageIndex].ReadTemplate("社内スタッフ撮照");
            //}

            //int deviceDpi = 97;
            //this.ScrapingRects = OcrItems[imageIndex].GetScrapingRectsImage(deviceDpi);

            // BoundingRect
            //if (engine.OcrResults.Count > 0)
            //{
            //    BitmapFrame bitmapFrame = BitmapFrame.Create(ImageSource);

            //    // UIの OverlayViewBox に BoundingRect を描画した WritableBitmap を提供
            //    int width = bitmapFrame.PixelWidth;
            //    int height = bitmapFrame.PixelHeight;
            //    double dpiX = bitmapFrame.DpiX;
            //    double dpiY = bitmapFrame.DpiY;

            //    WriteableBitmap resultRects =
            //        new WriteableBitmap(width, height, dpiX, dpiY, PixelFormats.Pbgra32, null);

            //    DrawBoundingBoxs(ref resultRects);
            //    resultRects.Freeze();

            //    BoundingRects = resultRects;
            //}

            SetButtonsEnabled();
            Message = $"{imageIndex + 1} / { Items.Count}";
        }
        private void ClearImage()
        {
            ImageSource = null;
            //BoundingRects = null;
            //engine.ClearResults();
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
        string errMsgNotAvailableLanguage = "指定の言語は利用できません";
        string msgReset = "クリアしました。";

    }
}
