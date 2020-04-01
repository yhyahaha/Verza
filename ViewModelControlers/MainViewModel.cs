using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModel;


namespace ViewModelControlers
{
    public class MainViewModel
    {
        public ObservableCollection<ImageFileWithOcrResults> Items { get; private set; }
            = new ObservableCollection<ImageFileWithOcrResults>();




    }
}
