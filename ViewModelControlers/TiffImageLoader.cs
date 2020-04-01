using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ViewModelControlers
{
    class TiffImageLoader 
    {
        public IList<string> GetFilesWithPicker()
        {
            var list = new List<string>();

            using(var dialog = new CommonOpenFileDialog("ファイルを開く"))
            {
                dialog.Filters.Add(new CommonFileDialogFilter("TIFF", "*.tif;*.tiff"));
                dialog.RestoreDirectory = true;
                dialog.Multiselect = true;

                if(dialog.ShowDialog() == CommonFileDialogResult.Ok)
                {
                    foreach(var fileName in dialog.FileNames)
                    {
                        // FullNameを取得
                        list.Add(fileName);
                    }
                }
            }
            return list;
        }

        public void DivideMultiTiffImageAndCopyToWorkDirectory(IList<string> files, string directoryPath)
        {
            int index = 0;

            foreach (string file in files)
            {
                using (FileStream imageStream = new FileStream(file, FileMode.Open, FileAccess.Read))
                {
                    TiffBitmapDecoder tiffDecoder = new TiffBitmapDecoder(imageStream, BitmapCreateOptions.None, BitmapCacheOption.OnLoad);
                    int framesCount = tiffDecoder.Frames.Count;
                    for (int i = 0; i < framesCount; i++)
                    {
                        string fileName = index.ToString(CultureInfo.CurrentCulture) + ".tif";
                        SaveBitmapFrameAsTiffImage(tiffDecoder.Frames[i], TiffCompressOption.Default, directoryPath, fileName);
                        index++;
                    }
                }
            }
        }

        private void SaveBitmapFrameAsTiffImage(BitmapFrame bitmapFrame, TiffCompressOption compressOption, string saveDirectory, string fileName)
        {
            TiffBitmapEncoder encoder = new TiffBitmapEncoder();
            encoder.Frames.Add(bitmapFrame);
            encoder.Compression = compressOption;

            string saveFilePath = Path.Combine(saveDirectory, fileName);

            using (FileStream fileStream = new FileStream(saveFilePath, FileMode.Create, FileAccess.Write))
            {
                encoder.Save(fileStream);
            }
        }


        public void RotateTiffImage(string filePath, double rotation)
        {
            using (FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite))
            {
                var bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.StreamSource = fileStream;
                bitmap.EndInit();
                bitmap.Freeze();

                TransformedBitmap transformedBitmap = new TransformedBitmap(bitmap, new RotateTransform(rotation));
                transformedBitmap.Freeze();

                BitmapFrame transformed = BitmapFrame.Create(transformedBitmap);
                TiffBitmapEncoder encoder = new TiffBitmapEncoder();
                encoder.Frames.Add(transformed);
                encoder.Save(fileStream);
            }
        }


        public BitmapSource CreateBitmapSourceFromPath(string filePath, double decodeRate)
        {
            // DecodePixelHeight の既定値は 0 なので 正しい値が取れなくてもエラーにはならない
            int decodePixelHeight = (int)(GetFileHeightProperty(filePath) * decodeRate);
            
            using (FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite))
            {
                var bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.StreamSource = fileStream;
                bitmap.DecodePixelHeight = decodePixelHeight; // 規定値 0
                bitmap.EndInit();
                bitmap.Freeze();

                return bitmap;
            }
        }

        // ファイルのプロパティからイメージの高さを取得
        private int GetFileHeightProperty(string filePath)
        {
            Shell32.Shell shell = new Shell32.Shell();
            string res = "";
            int height = 0;

            try
            {
                Shell32.Folder objFolder = shell.NameSpace(System.IO.Path.GetDirectoryName(filePath));
                Shell32.FolderItem folderItem = objFolder.ParseName(System.IO.Path.GetFileName(filePath));

                for (int i = 0; i < 300; i++)
                {
                    if (objFolder.GetDetailsOf("", i) == "高さ")
                    {
                        res = objFolder.GetDetailsOf(folderItem, i);
                        break;
                    }
                }

                Regex regex = new Regex("[0-9]+");
                Match match = regex.Match(res);
                if (match.Success) height = int.Parse(match.Value);
            }
            catch
            {
                height = 0;
            }

            return height;
        }
    }
}
