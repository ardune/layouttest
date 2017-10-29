using System;
using System.Windows.Input;

namespace LayoutTest.Commands
{
    public class BypassExecuteCommand : ICommand
    {
        private readonly ICommand nestedCommand;
        private readonly Action executeAction;

        public BypassExecuteCommand(ICommand nestedCommand, Action executeAction)
        {
            this.nestedCommand = nestedCommand;
            this.executeAction = executeAction;
        }

        public bool CanExecute(object parameter)
        {
            return nestedCommand.CanExecute(parameter);
        }

        public void Execute(object parameter)
        {
            executeAction();
        }

        public event EventHandler CanExecuteChanged
        {
            add => nestedCommand.CanExecuteChanged += value;
            remove => nestedCommand.CanExecuteChanged -= value;
        }
    }
}