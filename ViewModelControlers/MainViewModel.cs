﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModel;
using Interfaces;


namespace ViewModelControlers
{
    public class MainViewModel : IControler
    {
        public ObservableCollection<ImageFileWithOcrResults> Items { get; private set; }
            = new ObservableCollection<ImageFileWithOcrResults>();

        public IDelegateCommand ShowFilePickerCommand => throw new NotImplementedException();

        public IDelegateCommand ClearFileListCommand => throw new NotImplementedException();

        public IDelegateCommand DeleteFileCommand => throw new NotImplementedException();
    }
}
