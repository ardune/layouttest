using Caliburn.Micro;
using SimpleInjector;

namespace LayoutTest.Features.Shell
{
    public class ContainerViewModelLocator : IViewModelLocator
    {
        private readonly Container container;

        public ContainerViewModelLocator(Container container)
        {
            this.container = container;
        }

        public T GetInstance<T>() where T : class, IScreen
        {
            return container.GetInstance<T>();
        }
    }
}