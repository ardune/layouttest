using System.Windows.Input;

namespace LayoutTest.Features.Flyin
{
    public class SavedTab
    {
        public string Titled { get; }

        public ICommand OpenOpenCommand { get; }

        public SavedTab(string titled, ICommand openCommand)
        {
            Titled = titled;
            OpenOpenCommand = openCommand;
        }
    }
}