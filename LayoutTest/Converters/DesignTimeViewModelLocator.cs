using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using Caliburn.Micro;
using LayoutTest.Configuration;
using SimpleInjector;

namespace LayoutTest.Converters
{
    public class DesignTimeViewModelLocator : IValueConverter
    {
        public static DesignTimeViewModelLocator Instance = new DesignTimeViewModelLocator();
        private static readonly Container TestContainer;

        static DesignTimeViewModelLocator()
        {
            if (!Execute.InDesignMode)
            {
                return;
            }

            AssemblySource.Instance.Clear();
            TestContainer = SimpleInjectorConfiguration.Configure();
            IoC.GetInstance = (t,c)=>TestContainer.GetInstance(t);
            IoC.GetAllInstances = type => throw new NotSupportedException();
            IoC.BuildUp = o => throw new NotSupportedException();
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Blend creates types from a runtime/dynamic assembly, so match on name/namespace
            if (value == null)
            {
                throw new Exception("Expected {Binding Source={Type}} to be present");
            }

            var fullType = GetTypeFromMockNames(value)
                           ?? GetTypeFromNamespaceName(value);

            if (fullType == null)
            {
                throw new Exception("Could not locate valid type from " + value + "\r\nMake sure it inhertis from IViewModel");
            }
            var instance = IoC.GetInstance(fullType, null);
            if (instance is IActivate activate)
            {
                activate.Activate();
            }
            return instance;
        }

        private static Type GetTypeFromNamespaceName(object value)
        {
            var name = value.GetType().Name.Split('.').Reverse().FirstOrDefault();
            return GetRegisteredTypeByName(name);
        }

        private static Type GetTypeFromMockNames(object value)
        {
            var name = value.GetType().Name.Split('_').Reverse().Take(3).Reverse().FirstOrDefault();
            return GetRegisteredTypeByName(name);
        }

        private static Type GetRegisteredTypeByName(string name)
        {
            var fullType = TestContainer.GetCurrentRegistrations().Select(x => x.ServiceType)
                .FirstOrDefault(x => x.Name == name);

            return fullType;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
