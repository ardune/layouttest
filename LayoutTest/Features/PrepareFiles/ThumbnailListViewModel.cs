using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Caliburn.Micro;
using LayoutTest.Commands;
using LayoutTest.Extensions;
using LayoutTest.Features.Shared;
using LayoutTest.Features.Shared.State;

namespace LayoutTest.Features.PrepareFiles
{
    public class ThumbnailListViewModel : ScreenBase, IViewModel
    {
        private readonly AppStateHolder appState;
        private List<PageItem> thumbnails;

        public ThumbnailListViewModel(AppStateHolder appState)
        {;
            this.appState = appState;
        }

        protected override void OnActivate()
        {
            Subscriptions = new List<IDisposable>
            {
                appState.Project(x => x.Pages)
                    .Subscribe(NewPages),
                appState.Project(x => x.PrepActivity.PrimaryPageSelectionIndex)
                    .SubscribeWith(OnPrimaryPageSelectionIndexChanged),
                appState.CurrentState.Select(state => state.Pages.Select(page => new PageTagView
                    {
                        Page = page,
                        Tags = state.PageTags
                            .Where(x => x.PageId == page.Id)
                            .SelectMany(pageTag => state.Tags.Where(tag=>tag.Id == pageTag.TagId))
                            .ToArray()
                    }))
                    .DistinctUntilChanged()
                    .Subscribe(PagesTest)
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
                var activity = x.PrepActivity;
                activity.PrimaryPageSelectionIndex = index < 0 ? null : index;
                x.PrepActivity = activity;
                return x;
            });
        }

        private void OnPrimaryPageSelectionIndexChanged(int? i)
        {
        }

        private void PagesTest(IEnumerable<PageTagView> obj)
        {
        }

        private void NewPages(Page[] obj)
        {
        }
        private void SetDesignValues()
        {
        }
    }
}