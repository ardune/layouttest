using System;
using System.Reactive;

namespace LayoutTest.Extensions
{
    public static class ObservableExtensions
    {
        public static IDisposable SubscribeWith<T>(this IObservable<T> target, Action<T> handler)
        {
            return target.Subscribe(new AnonymousObserver<T>(handler));
        }
    }
}
