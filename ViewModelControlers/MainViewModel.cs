using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModel;
using Interfaces;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Media.Imaging;
using System.IO;

namespace ViewModelControlers
{
    public partial class MainViewModel : IControler, INotifyPropertyChanged
    {
        
        public ObservableCollection<ImageFileWithOcrResults> Items { get; private set; }
            = new ObservableCollection<ImageFileWithOcrResults>();

        public MainViewModel()
        {
            this.deviceDpi = 97;
            this.workDirectory = "WORK";
            imageIndex = -1;
            SetComboBoxOcrParam();
            SetComboBoxOcrTemplats();
        }

        // Initializer

        private void SetComboBoxOcrParam()
        {
            ocrParam = 0.6;
            ocrParamList = new List<double>()
            { 1.0, 0.95, 0.90, 0.85, 0.80, 0.75, 0.70, 0.65, 0.60, 0.55, 0.50, 0.45, 0.40 };
        }

        private void SetComboBoxOcrTemplats()
        {
            listOfOcrTemplates = new List<string>();

            DirectoryInfo di = new DirectoryInfo("Templates");
            var templateList = di.GetFiles();

            foreach(var template in templateList)
            {
                listOfOcrTemplates.Add(template.Name);
            }
        }

        // Properties

        public string Message
        {
            get { return this.message; }
            private set
            {
                this.message = value;
                OnPropertyChanged();
            }
        }

        public BitmapSource ImageSource
        {
            get { return this.imageSource; }
            private set
            {
                this.imageSource = value;
                OnPropertyChanged();
            }
        }
        
        public double ImageAngle
        {
            get { return this.imageAngle; }
            private set
            {
                if (value == this.imageAngle) return;
                this.imageAngle = value;
                OnPropertyChanged();
            }
        }

        public BitmapSource ScrapingRectsImage
        {
            get { return this.scrapingRectsImage; }
            set
            {
                this.scrapingRectsImage = value;
                OnPropertyChanged();
            }
        }

        public double OcrParam
        {
            get { return this.ocrParam; }
            set
            {
                if (value == this.ocrParam) return;
                this.ocrParam = value;
                OnPropertyChanged();
            }
        }

        public List<double> OcrParamList
        {
            get { return ocrParamList; }
        }

        public string OcrTemplate
        {
            get { return this.ocrTemplate; }
            set
            {
                if (value == this.ocrTemplate) return;
                this.ocrTemplate = value;
                OnPropertyChanged();
            }
        }

        public List<string> ListOfOcrTemplates
        {
            get { return this.listOfOcrTemplates; }
        }


        // DelegateCommands

        public IDelegateCommand PickUpFilesCommand
        {
            get
            {
                if (pickUpFilesCommand == null)
                {
                    pickUpFilesCommand = new DelegateCommand(ExecutePickUpFilesCommand, CanExecutePickupCommand);
                }
                return pickUpFilesCommand;
            }
        }
        
        public IDelegateCommand NextImageCommand
        {
            get
            {
                if (nextImageCommand == null)
                {
                    nextImageCommand = new DelegateCommand(ExecuteNextImageCommand, CanExecuteNextImageCommand);
                }
                return nextImageCommand;
            }
        }

        public IDelegateCommand PreviousImageCommand
        {
            get
            {
                if (previousImageCommand == null)
                {
                    previousImageCommand = new DelegateCommand(ExecutePreviousImageCommand, CanExecutePreviousImageCommand);
                }
                return previousImageCommand;
            }
        }

        public IDelegateCommand GoOcrCommand
        {
            get
            {
                if (goOcrCommand == null)
                {
                    //goOcrCommand = new DelegateCommand(ExecuteGoOcrCommand, CanExecuteGoOcrCommand);
                }
                return goOcrCommand;
            }
        }

        public IDelegateCommand RotateRightCommond
        {
            get
            {
                if (rotateRightCommond == null)
                {
                    rotateRightCommond = new DelegateCommand(ExecuteRotateRightCommond, CanExecuteRotateCommand);
                }
                return rotateRightCommond;
            }
        }

        public IDelegateCommand RotateLeftCommond
        {
            get
            {
                if (rotateLeftCommond == null)
                {
                    rotateLeftCommond = new DelegateCommand(ExecuteRotateLeftCommond, CanExecuteRotateCommand);
                }
                return rotateLeftCommond;
            }
        }

        public IDelegateCommand ClearFileListCommand
        {
            get
            {
                if (clearFileListCommand == null)
                {
                    clearFileListCommand = new DelegateCommand(ExecuteFilesClearCommand);
                }
                return clearFileListCommand;
            }
        }

        public IDelegateCommand DeleteFileCommand
        {
            get
            {
                if (deleteFileCommand == null)
                {
                    deleteFileCommand = new DelegateCommand(ExecuteDeleteFileCommand, CanExecuteDeleteFileCommand);
                }
                return deleteFileCommand;
            }
        }

        // INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        // Fields
        private readonly int deviceDpi;
        private int imageIndex;
        private readonly string workDirectory;
        private string message;
        private BitmapSource imageSource;
        private double imageAngle;
        private BitmapSource scrapingRectsImage;
        private double ocrParam;
        private List<double> ocrParamList;
        private string ocrTemplate;
        private List<string> listOfOcrTemplates;
        private IDelegateCommand pickUpFilesCommand;
        private IDelegateCommand nextImageCommand;
        private IDelegateCommand previousImageCommand;
        private IDelegateCommand goOcrCommand;
        private IDelegateCommand rotateRightCommond;
        private IDelegateCommand rotateLeftCommond;
        private IDelegateCommand clearFileListCommand;
        private IDelegateCommand deleteFileCommand;
    }
}
