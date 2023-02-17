using PC_GAMING_BAZE.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PC_GAMING_BAZE.CustomCommands
{
    public class SetTimeItemClick : ICommand
    {
    
        private Action<object> _handler;
        private Predicate<object> _canExecute;

        public SetTimeItemClick(Action<object> handler, Predicate<object> canExecute)
        {
            _handler = handler;
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            Console.WriteLine(parameter.GetType());
            return _canExecute(parameter);
        }

        public void Execute(object parameter)
        {
            Console.WriteLine(parameter.GetType());
            _handler(parameter);
        }

        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

    }
}
