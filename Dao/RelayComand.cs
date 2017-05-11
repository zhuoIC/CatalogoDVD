using System;
using System.Windows.Input;

namespace Dao
{
    class RelayComand:ICommand
    {
        private Action<object> _execute;
        private Func<object, bool> _canExecute;
        public RelayComand(Action<object> execute, Func<object,bool> canExecute) 
        {
            if (execute==null)
            {
                throw new ArgumentNullException("Falta el comando de ejecución");
            }
            else
            {
                _execute = execute;
                _canExecute = canExecute;
            }
        }
        public bool CanExecute(object parameter)
        {
            return _canExecute == null ? true:_canExecute(parameter);
        }

        public event EventHandler CanExecuteChanged 
        {
            add 
            {
                if (_canExecute!=null)
                {
                    CommandManager.RequerySuggested += value;
                }
            }
            remove 
            {
                if (_canExecute!=null)
                {
                    CommandManager.RequerySuggested+=value;
                }
            }
        }
        public void Execute(object parameter)
        {
            _execute(parameter);
        }
    }
}
