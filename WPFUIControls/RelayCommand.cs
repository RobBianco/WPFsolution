using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace WPFUIControls
{
    public class RelayCommand(Action<object> execute, Func<object, bool>? canExecute = null)
        : ICommand
    {
        private readonly Action<object> execute = execute ?? throw new ArgumentNullException(nameof(execute));

        public event EventHandler? CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        public bool CanExecute(object? parameter)
        {
            return canExecute == null || canExecute(parameter);
        }

        public void Execute(object? parameter)
        {
            if (parameter != null) execute(parameter);
        }
    }
}
