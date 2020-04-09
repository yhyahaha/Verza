using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Interfaces
{
    public interface IControler
    {
        // Properties
        string Message { get; }

        BitmapFrame ImageSource { get; }

        double ImageAngle { get; }

        BitmapSource ScrapingRectsImage { get; }

        BitmapSource BoundingRectsImage { get; }

        IList<string> AvailableLanguages { get; }

        string OcrLanguage { get; set; }

        double OcrParam { get; set; }

        List<double> OcrParamList { get; }

        string OcrTemplate { get; set; }

        List<string> ListOfOcrTemplates { get; }

        // Commands
        IDelegateCommand PickUpFilesCommand { get; }

        IDelegateCommand NextImageCommand { get; }

        IDelegateCommand PreviousImageCommand { get; }

        IDelegateCommand GoOcrCommand { get; }

        IDelegateCommand RotateRightCommond { get; }

        IDelegateCommand RotateLeftCommond { get; }

        IDelegateCommand ClearFileListCommand { get; }

        IDelegateCommand DeleteFileCommand { get; }

        // Public Method
        void ReOcrScrapingRectByPosition(int positionX, int positionY);
    }
}
