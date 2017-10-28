using System;
using System.Windows;
using Caliburn.Micro;
using LayoutTest.Features.Shell;
using SimpleInjector;

namespace LayoutTest.Configuration
{
    public class Bootstrapper : BootstrapperBase
    {
        private Container container;

        public Bootstrapper()
        {
            Initialize();
        }

        protected override void Configure()
        {
            ViewModelBinder.ApplyConventionsByDefault = false;
            container = SimpleInjectorConfiguration.Configure();
        }

        protected override object GetInstance(Type service, string key)
        {
            if (!string.IsNullOrWhiteSpace(key))
            {
                throw new NotSupportedException();
            }
            return container.GetInstance(service);
        }

        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            DisplayRootViewFor<ShellViewModel>();
        }
    }
}
