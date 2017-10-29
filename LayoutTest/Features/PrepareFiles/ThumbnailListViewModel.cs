using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Caliburn.Micro;
using LayoutTest.Commands;
using LayoutTest.Extensions;
using LayoutTest.Features.Shared;

namespace LayoutTest.Features.PrepareFiles
{
    public class ThumbnailListViewModel : ScreenBase, IViewModel
    {
        private readonly AppStateHolder appState;
        private List<PageItem> thumbnails;

        public ThumbnailListViewModel(AppStateHolder appState)
        {
            this.appState = appState;
        }

        protected override void OnActivate()
        {
            Subscriptions = new List<IDisposable>
            {
                appState.Project(x => x.Pages)
                    .Subscribe(NewPages),
                appState.Project(x => x.PrimaryPageSelectionIndex)
                    .SubscribeWith(OnPrimaryPageSelectionIndexChanged)
            };
            
            if (Execute.InDesignMode)
            {
                SetDesignValues();
            }
            base.OnActivate();
        }

        public List<PageItem> Thumbnails
        {
            get { return thumbnails; }
            set
            {
                thumbnails = value;
                NotifyOfPropertyChange();
            }
        }

        public ICommand SelectCommand => new AsyncDelegateCommand<PageItem>(Select);

        private async Task Select(PageItem arg)
        {
            var index = Thumbnails?.IndexOf(arg);
            await appState.UpdateState(x =>
            {
                Trace.WriteLine("Change Selection: " + index);
                x.PrimaryPageSelectionIndex = index < 0 ? null : index;
                return x;
            });
        }

        private void OnPrimaryPageSelectionIndexChanged(int? i)
        {
            Trace.WriteLine("OnPrimaryPageSelectionIndexChanged: " + i);

            var selected = appState.LatestState.PrimaryPageSelectionIndex;
            for (var index = 0; index < Thumbnails.Count; index++)
            {
                var thumbnail = Thumbnails[index];
                thumbnail.IsSelected = selected == index;
            }
        }

        private void NewPages(Page[] obj)
        {
            Trace.WriteLine("List changed: ");
            var selected = appState.LatestState.PrimaryPageSelectionIndex;
            var newPages = obj.Select((x,i) => new PageItem(x)
            {
                IsSelected = i == selected
            }).ToList();
            Thumbnails = newPages;
        }
        private void SetDesignValues()
        {
            var all = Enumerable.Range(96, 120).Select(x => new PageItem
            {
                IsSelected = x == 99,
                PageNumber = x + 1,
                IsDeleted = (x + 1) % 2 == 0,
                Select1 = (x + 1) % 4 > 1,
                Select2 = (x + 1) % 5 > 2
            });

            Thumbnails = all.ToList();
        }
    }
}