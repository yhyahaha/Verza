using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces
{
    public interface IControler
    {
        /// <summary>
        /// FilePickerを使ってFileを取得する
        /// </summary>
        IDelegateCommand PickUpFilesCommand { get; }

        /// <summary>
        /// 取得したファイル・リストをクリアする
        /// </summary>
        //IDelegateCommand ClearFileListCommand { get; }

        /// <summary>
        /// 選択されているファイルをファイル・リストから削除する
        /// </summary>
        //IDelegateCommand DeleteFileCommand { get; }

    }
}
