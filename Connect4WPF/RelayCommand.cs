using System;

namespace Connect4WPF
{
    using System.Windows.Input;

    internal class RelayCommand : ICommand
    {
        private Action<object> execute;
        private Func<object, bool> canExecute;

        public event EventHandler CanExecuteChanged;

        public RelayCommand(Action<object> execute, Func<object, bool> canExecute = null)
        {
            if (execute == null) throw new ArgumentNullException("Execute can not be null");
            this.execute = execute;
            this.canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            if (canExecute == null) return true;
            else return canExecute(parameter);
        }

        public void Execute(object parameter)
        {
            execute(parameter);
        }
    }

}
