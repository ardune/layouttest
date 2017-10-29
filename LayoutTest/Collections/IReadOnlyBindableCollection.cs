using System.Collections.Generic;
using System.Collections.Specialized;
using Caliburn.Micro;

namespace LayoutTest.Collections
{
    public interface IReadOnlyBindableCollection<T> : INotifyCollectionChanged, INotifyPropertyChangedEx, IReadOnlyCollection<T>
    {
    }
}