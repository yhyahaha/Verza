using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Interfaces;

namespace ViewModelClassLibrary
{
    internal class DelegateCommand : IDelegateCommand
    {
        // DeledateCommand コンストラクター
        public DelegateCommand(Action execute)
        {
            this.execute = x => execute();
            this.canExecute = x => true;
        }

        public DelegateCommand(Action execute, Func<bool> canExecute)
        {
            this.execute = x => execute();
            this.canExecute = x => canExecute();
        }

        public DelegateCommand(Action<object> execute)
        {
            this.execute = execute;
            this.canExecute = x => true;
        }

        public DelegateCommand(Action<object> execute, Func<object, bool> canExecute)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }

        // ICommand実装
        private readonly Action<object> execute;
        public void Execute(object value) => this.execute(value);

        private readonly Func<object, bool> canExecute;
        public bool CanExecute(object value) => this.canExecute(value);

        public event EventHandler CanExecuteChanged;

        // IDelegateCommandの実装
        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
