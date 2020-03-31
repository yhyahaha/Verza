using System.Windows.Input;

namespace Interfaces
{
    public interface IDelegateCommand : ICommand
    {
        void RaiseCanExecuteChanged();
    }
}
