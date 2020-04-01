using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModelControlers
{
    partial class MainViewModel
    {
        private void ExecutePickUpFilesCommand()
        {
            //// FileOpenPickerでTIFFファイルを取得
            //TiffImageLoader loader = new TiffImageLoader();
            //var list = loader.GetFiles();

            //if (list.Count == 0) return;

            //// 作業ディレクトリ
            //if (Directory.Exists(workDirectory))
            //{
            //    Directory.Delete(workDirectory, true);
            //}
            //Directory.CreateDirectory(workDirectory);

            //// TIFFファイルを分解して作業ディレクトリにコピー          
            //loader.CopyDividedFileToWorkDirectory(list, workDirectory);

            //OcrItems.Clear();
            //DirectoryInfo di = new DirectoryInfo(workDirectory);
            //var files = di.GetFiles();
            //foreach (var file in files)
            //{
            //    OcrItem item = new OcrItem(file.FullName);
            //    OcrItems.Add(item);
            //}

            //Message = $"{ OcrItems.Count } 件のファイルを取得しました。";

            //SetButtonsEnabled();
        }

        private bool CanExecutePickupCommand()
        {
            return (Items.Count == 0);
        }





    }
}
