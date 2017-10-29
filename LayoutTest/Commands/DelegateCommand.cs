using System;
using System.Windows.Input;

namespace LayoutTest.Commands
{
    public class DelegateCommand : DelegateCommand<object>
    {
        public DelegateCommand(Action action)
            : this(action,()=>true)
        {
        }

        public DelegateCommand(Action action, Func<bool> canExecute)
            : base(x=>action(), x => canExecute())
        {
        }
    }

    public class DelegateCommand<T> : ICommand, IRaiseCanExecuteChanged
    {
        public event EventHandler CanExecuteChanged;

        private readonly Action<T> action;
        private readonly Func<T,bool> canExecute;

        public DelegateCommand(Action<T> action)
            : this(action, x=>true)
        {
            
        }

        public DelegateCommand(Action<T> action, Func<T,bool> canExecute)
        {
            this.action = action;
            this.canExecute = canExecute;
        }


        public bool CanExecute(object parameter)
        {
            if (parameter is T matched)
            {
                return canExecute(matched);
            }

            return canExecute(default(T));
        }

        public void Execute(object parameter)
        {
            if (parameter is T matched)
            {
                action(matched);
                return;
            }

            action(default(T));
        }

        public void RaiseCanExecuteChanged()
        {
            //CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}