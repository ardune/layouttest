using Caliburn.Micro;

namespace LayoutTest.Features.Shell
{
    public interface IViewModelLocator
    {
        T GetInstance<T>() where T : class, IScreen;
    }
}