using System;
using System.Collections.Generic;
using Caliburn.Micro;

namespace LayoutTest.Features.Shared
{
    public abstract class ScreenBase : Screen
    {
        protected List<IDisposable> Subscriptions = new List<IDisposable>();

        protected override void OnDeactivate(bool close)
        {
            if (Subscriptions != null)
            {
                foreach (var subscription in Subscriptions)
                {
                    subscription.Dispose();
                }
                Subscriptions = null;
            }
            base.OnDeactivate(close);
        }
    }
}