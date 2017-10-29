﻿using System;
using System.Windows.Input;

namespace LayoutTest.Commands
{
    public class DelegateCommand : ICommand, IRaiseCanExecuteChanged
    {
        public event EventHandler CanExecuteChanged;

        private readonly Action action;
        private readonly Func<bool> canExecute;

        public DelegateCommand(Action action)
            : this(action,()=>true)
        {
        }

        public DelegateCommand(Action action,Func<bool> canExecute)
        {
            this.action = action;
            this.canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return canExecute();
        }

        public void Execute(object parameter)
        {
            action();
        }

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}