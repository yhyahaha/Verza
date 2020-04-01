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

namespace ViewModelControlers
{
    public partial class MainViewModel : IControler, INotifyPropertyChanged
    {
        
        public ObservableCollection<ImageFileWithOcrResults> Items { get; private set; }
            = new ObservableCollection<ImageFileWithOcrResults>();

        public MainViewModel()
        {
            this.workDirectory = "WORK";
        }

        public string Message
        {
            get { return message; }
            set
            {
                this.message = value;
                OnPropertyChanged();
            }
        }

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

        //public IDelegateCommand ClearFileListCommand => throw new NotImplementedException();

        //public IDelegateCommand DeleteFileCommand => throw new NotImplementedException();

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        
        // Fields
        private string workDirectory;
        private string message;
        private IDelegateCommand pickUpFilesCommand;
    }
}
