using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces
{
    interface IOcrItem
    {
        /// <summary>
        /// ImageFileのパス
        /// </summary>
        string FilePath { get; set; }

        /// <summary>
        /// OCR結果としての画像回転角度
        /// </summary>
        double OcrAngle { get; set; }

        /// <summary>
        /// バウンディングボックス
        /// </summary>
        IList<ScrapingRect> ScrapingRects { get; set; }

        //public void ReadTemplate(string templateName)

        // BitmapSource GetScrapingRectsImage(int deviceDpi)
    }
}
