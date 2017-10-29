using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace LayoutTest.Commands
{
    public class AsyncDelegateCommand : AsyncDelegateCommand<object>, IAsyncCommand
    {
        public AsyncDelegateCommand(Func<Task> executeMethod)
            : base(o => executeMethod())
        {
        }

        public AsyncDelegateCommand(Func<Task> executeMethod, Func<bool> canExecuteMethod)
            : base(o => executeMethod(), o => canExecuteMethod())
        {
        }
    }

    public class AsyncDelegateCommand<T> : IAsyncCommand<T>, ICommand
    {
        private readonly Func<T, Task> executeMethod;
        private readonly DelegateCommand<T> underlyingCommand;
        private bool isExecuting;

        public AsyncDelegateCommand(Func<T, Task> executeMethod)
            : this(executeMethod, _ => true)
        {
        }

        public AsyncDelegateCommand(Func<T, Task> executeMethod, Func<T, bool> canExecuteMethod)
        {
            this.executeMethod = executeMethod;
            underlyingCommand = new DelegateCommand<T>(x => { }, canExecuteMethod);
        }

        public async Task ExecuteAsync(T obj)
        {
            try
            {
                isExecuting = true;
                RaiseCanExecuteChanged();
                await executeMethod(obj);
            }
            finally
            {
                isExecuting = false;
                RaiseCanExecuteChanged();
            }
        }

        public ICommand Command { get { return this; } }

        public bool CanExecute(object parameter)
        {
            return !isExecuting && underlyingCommand.CanExecute((T)parameter);
        }

        public event EventHandler CanExecuteChanged
        {
            add { underlyingCommand.CanExecuteChanged += value; }
            remove { underlyingCommand.CanExecuteChanged -= value; }
        }

        public async void Execute(object parameter)
        {
            await ExecuteAsync((T)parameter);
        }

        public void RaiseCanExecuteChanged()
        {
            underlyingCommand.RaiseCanExecuteChanged();
        }
    }
}