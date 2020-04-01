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
            // FileOpenPickerでTIFFファイルを取得
            TiffImageLoader loader = new TiffImageLoader();
            var list = loader.GetFiles();

            if (list.Count == 0) return;

            // 作業ディレクトリ
            if (Directory.Exists(workDirectory))
            {
                Directory.Delete(workDirectory, true);
            }
            Directory.CreateDirectory(workDirectory);

            // TIFFファイルを分解して作業ディレクトリにコピー          
            loader.CopyDividedFileToWorkDirectory(list, workDirectory);

            Items.Clear();
            DirectoryInfo di = new DirectoryInfo(workDirectory);
            var files = di.GetFiles();
            foreach (var file in files)
            {
                ImageFileWithOcrResults item = new ImageFileWithOcrResults();
                item.FilePath = file.FullName;
                Items.Add(item);
            }

            Message = $"{ Items.Count } 件のファイルを取得しました。";

            //SetButtonsEnabled();
        }

        private bool CanExecutePickupCommand()
        {
            return (Items.Count == 0);
        }





    }
}
