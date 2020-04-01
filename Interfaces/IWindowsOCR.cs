using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Interfaces
{
    public interface IWindowsOCR
    {
        /// <summary>
        /// OCRの実行言語
        /// </summary>
        string OcrLanguageTag { get; }

        /// <summary>
        /// OCRの実行が可能か
        /// </summary>
        bool CanExecute { get; }

        /// <summary>
        /// OCR結果の画像修正角度
        /// </summary>
        double OcrAngle { get; }

        /// <summary>
        /// OCR結果のWordsとBoundingRect情報を
        /// WinOcrResult構造体のリストで返す
        /// </summary>
        IList<WinOcrResult> OcrResults { get; }

        bool HasResults { get; }

        /// <summary>
        /// OCRの実行言語を設定する
        /// </summary>
        /// <param name="languageTag">
        /// LanguageTagを指定("ja" "en-us"等)
        /// </param>
        void SetOcrLanguage(string languageTag);

        /// <summary>
        /// OCRエンジンに指定可能な言語のTagリスト
        /// </summary>
        /// <returns></returns>
        IList<string> GetAvailableLanguageTags();

        /// <summary>
        /// OCR実行
        /// </summary>
        /// <param name="bitmapFrame"></param>
        /// <returns></returns>
        Task RecognizeAsync(BitmapFrame bitmapFrame);

        void ClearResults();
    }
}
