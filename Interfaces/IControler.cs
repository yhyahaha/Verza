using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces
{
    public interface IControler
    {
        IDelegateCommand PickUpFilesCommand { get; }

        IDelegateCommand NextImageCommand { get; }

        IDelegateCommand PreviousImageCommand { get; }

        IDelegateCommand GoOcrCommand { get; }

        IDelegateCommand RotateRightCommond { get; }

        IDelegateCommand RotateLeftCommond { get; }

        IDelegateCommand ClearFileListCommand { get; }

        IDelegateCommand DeleteFileCommand { get; }
    }
}
