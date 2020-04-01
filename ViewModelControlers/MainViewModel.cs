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

namespace ViewModelControlers
{
    public partial class MainViewModel : IControler, INotifyPropertyChanged
    {
        
        public ObservableCollection<ImageFileWithOcrResults> Items { get; private set; }
            = new ObservableCollection<ImageFileWithOcrResults>();

        public MainViewModel()
        {
            this.workDirectory = "WORK";
            imageIndex = -1;

        }

        // Properties

        public string Message
        {
            get { return this.message; }
            set
            {
                this.message = value;
                OnPropertyChanged();
            }
        }

        public BitmapSource ImageSource
        {
            get { return this.imageSource; }
            set
            {
                this.imageSource = value;
                OnPropertyChanged();
            }
        }

        private double imageAngle;
        public double ImageAngle
        {
            get { return imageAngle; }
            private set
            {
                imageAngle = value;
                OnPropertyChanged();
            }
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
        private int imageIndex;
        private readonly string workDirectory;
        private string message;
        private BitmapSource imageSource;
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
