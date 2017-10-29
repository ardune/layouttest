using System;
using System.ComponentModel;
using System.Linq.Expressions;

namespace LayoutTest.Extensions
{
    public static class BindingExtensions
    {
        public static void BindTo<T, TResult>(this T target, Expression<Func<T, TResult>> func, Action<TResult> callback)
            where T : INotifyPropertyChanged
        {
            if (!(func.Body is MemberExpression item))
            {
                throw new ArgumentException("expected func to represent a member expression");
            }
            var accessor = func.Compile();
            var member = item.Member;
            target.PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName != member.Name)
                {
                    return;
                }
                var newValue = accessor(target);
                callback(newValue);
            };
        }
    }
}