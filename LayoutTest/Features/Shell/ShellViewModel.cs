using Caliburn.Micro;
using LayoutTest.Features.PrepareFiles;
using LayoutTest.Features.Shared;

namespace LayoutTest.Features.Shell
{
    public class ShellViewModel : Conductor<IScreen>, IViewModel
    {
        private readonly IViewModelLocator viewModelLocator;

        public ShellViewModel(IViewModelLocator viewModelLocator)
        {
            this.viewModelLocator = viewModelLocator;
        }

        protected override void OnActivate()
        {
            var newView = viewModelLocator.GetInstance<PrepareFilesViewModel>();
            ActivateItem(newView);
        }
    }
}
