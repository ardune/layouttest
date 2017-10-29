using System.Windows.Input;
using Caliburn.Micro;
using LayoutTest.Collections;
using LayoutTest.Commands;
using LayoutTest.Extensions;
using LayoutTest.Features.Shared;
using LayoutTest.Features.Shell;
using Action = System.Action;

namespace LayoutTest.Features.Flyin
{
    public class FlyinBarViewModel : Conductor<IScreen>, IViewModel
    {
        private readonly IViewModelLocator viewModelLocator;
        private readonly ReadOnlyBindableCollection<SavedTab> tabs = new ReadOnlyBindableCollection<SavedTab>();

        public FlyinBarViewModel(IViewModelLocator viewModelLocator)
        {
            this.viewModelLocator = viewModelLocator;
            if (Execute.InDesignMode)
            {
                LoadDesignValues();
            }
        }

        public IReadOnlyBindableCollection<SavedTab> Tabs => tabs;

        public void AddTab<T>(string titled, ICommand openCommand)
            where T : Screen
        {
            var tab = new SavedTab(titled, CreateCommand<T>(openCommand));
            tabs.Add(tab);
        }

        private ICommand CreateCommand<T>(ICommand nestedCommand) 
            where T : Screen
        {
            return new BypassExecuteCommand(nestedCommand, () =>
            {
                var found = viewModelLocator.GetInstance<T>();
                ActivateItem(found);
                nestedCommand.Execute(found);
            });
        }
        
        private void LoadDesignValues()
        {
            for (int i = 0; i < 10; i++)
            {
                AddTab<Screen>($"Tab {i}", null);
            }
        }
    }
}
