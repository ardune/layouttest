using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Caliburn.Micro;
using LayoutTest.Commands;
using LayoutTest.Extensions;
using LayoutTest.Features.Flyin;
using LayoutTest.Features.Shared;

namespace LayoutTest.Features.PrepareFiles
{
    public class PrepareFilesViewModel : ScreenBase, IViewModel
    {
        private readonly AppStateHolder appStateHolder;
        private List<IDisposable> subscriptions;

        public PrepareFilesViewModel(
            ThumbnailListViewModel thumbnailListViewModel, 
            RenderPageViewModel renderPageViewModel, 
            FlyinBarViewModel flyinBarViewModel,
            AppStateHolder appStateHolder)
        {
            this.appStateHolder = appStateHolder;
            ThumbnailListViewModel = thumbnailListViewModel;
            RenderPageViewModel = renderPageViewModel;
            FlyinBarViewModel = flyinBarViewModel;


            ThumbnailListViewModel.ActivateWith(this);
            ThumbnailListViewModel.DeactivateWith(this);
            FlyinBarViewModel.ActivateWith(this);
            FlyinBarViewModel.DeactivateWith(this);

            ShowDetailsCommand = new DelegateCommand<DetailsViewModel>(ShowingDetais);

            FlyinBarViewModel.AddTab<DetailsViewModel>("D - Details", ShowDetailsCommand);

            AddPageCommand = new AsyncDelegateCommand(AddPage);
            RemovePageCommand = new AsyncDelegateCommand(RemoveSelectedPage, () => CurrentState.PrimaryPageSelectionIndex.HasValue);
        }

        private AppState CurrentState => appStateHolder.LatestState;
        

        private void ShowingDetais(DetailsViewModel obj)
        {
            obj.DisplayName = "Foo";
        }

        protected override void OnActivate()
        {
            subscriptions = new List<IDisposable>
            {
                appStateHolder
                    .Project(x => x.PrimaryPageSelectionIndex)
                    .SubscribeWith(IndexChanged)
            };
            base.OnActivate();
        }

        private void IndexChanged(int? obj)
        {
            RemovePageCommand.RaiseCanExecuteChanged();
        }

        public DelegateCommand<DetailsViewModel> ShowDetailsCommand { get; }

        private async Task RemoveSelectedPage()
        {
            await appStateHolder.UpdateState(x =>
            {
                if (x.PrimaryPageSelectionIndex == null)
                {
                    return x;
                }
                var page = x.Pages[x.PrimaryPageSelectionIndex.Value];
                x.Pages = x.Pages.Update(page, new
                {
                    IsDeleted = !page.IsDeleted
                });
                return x;
            });
        }

        private async Task AddPage()
        {
            await appStateHolder.UpdateState(x =>
            {
                var newPageNumber = x.Pages.Max(y => (int?)y.PageNumber).GetValueOrDefault(0) + 1;

                x.Pages = x.Pages.Concat(Enumerable.Range(0, 1000).Select(i =>
                    new Page
                    {
                        PageNumber = newPageNumber + i
                    }
                )).ToArray();

                return x;
            });

        }

        public ThumbnailListViewModel ThumbnailListViewModel { get; }

        public RenderPageViewModel RenderPageViewModel { get; }

        public ICommand AddPageCommand { get; set; }

        public ICommand RemovePageCommand { get; set; }

        public FlyinBarViewModel FlyinBarViewModel { get; }
    }
}