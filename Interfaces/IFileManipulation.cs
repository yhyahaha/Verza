namespace Interfaces
{
    interface IFileManipulation
    {   
        /// <summary>
        /// FilePickerを使ってFileを取得する
        /// </summary>
        IDelegateCommand ShowFilePickerCommand { get; }

        /// <summary>
        /// 取得したファイル・リストをクリアする
        /// </summary>
        IDelegateCommand ClearFileListCommand { get; }

        /// <summary>
        /// 選択されているファイルをファイル・リストから削除する
        /// </summary>
        IDelegateCommand DeleteFileCommand { get; }
    }
}
