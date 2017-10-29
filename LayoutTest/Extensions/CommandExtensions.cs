using System.Windows.Input;
using LayoutTest.Commands;

namespace LayoutTest.Extensions
{
    public static class CommandExtensions
    {
        public static void RaiseCanExecuteChanged(this ICommand target)
        {
            if (target is IRaiseCanExecuteChanged item)
            {
                item.RaiseCanExecuteChanged();
            }
        }
    }
}