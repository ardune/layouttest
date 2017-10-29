using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using LayoutTest.Features.Shared;
using LayoutTest.Features.Shared.State;
using LayoutTest.Features.Shell;
using SimpleInjector;

namespace LayoutTest.Configuration
{
    public static class SimpleInjectorConfiguration
    {
        public static Container Configure()
        {
            var container = new Container();
            var screens = container.GetTypesToRegister(typeof(IViewModel), new[] {Assembly.GetExecutingAssembly()});
            foreach (var screen in screens)
            {
                container.Register(screen);
            }

            container.Register<AppStateHolder>(Lifestyle.Singleton);
            container.Register<IWindowManager,WindowManager>(Lifestyle.Singleton);
            container.Register<IEventAggregator, EventAggregator>(Lifestyle.Singleton);
            container.Register<IViewModelLocator>(()=>new ContainerViewModelLocator(container));

            return container;
        }
    }
}
