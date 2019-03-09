using System;
using System.Windows.Input;

namespace SupplyChain
{
    public class CommandHandler : ICommand
    {
        public event EventHandler CanExecuteChanged;

        private readonly Action _action;
        private readonly Action<object> _actionWithParameter;

        public CommandHandler(Action action)
        {
            _action = action;
        }

        public CommandHandler(Action<object> action)
        {
            _actionWithParameter = action;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            _action?.Invoke();
            _actionWithParameter?.Invoke(parameter);
        }
    }
}
