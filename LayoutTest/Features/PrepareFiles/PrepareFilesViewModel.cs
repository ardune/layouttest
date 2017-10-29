using System.Linq;
using System.Windows.Input;
using Caliburn.Micro;
using LayoutTest.Commands;
using LayoutTest.Extensions;
using LayoutTest.Features.Flyin;
using LayoutTest.Features.Shared;

namespace LayoutTest.Features.PrepareFiles
{
    public class PrepareFilesViewModel : Screen, IViewModel
    {
        public PrepareFilesViewModel(
            ThumbnailListViewModel thumbnailListViewModel, 
            RenderPageViewModel renderPageViewModel, 
            FlyinBarViewModel flyinBarViewModel)
        {
            ThumbnailListViewModel = thumbnailListViewModel;
            RenderPageViewModel = renderPageViewModel;
            FlyinBarViewModel = flyinBarViewModel;

            ShowDetailsCommand = new DelegateCommand<DetailsViewModel>(ShowingDetais);

            FlyinBarViewModel.AddTab<DetailsViewModel>("D - Details", ShowDetailsCommand);

            AddPageCommand = new DelegateCommand(AddPage);
            RemovePageCommand = new DelegateCommand(RemoveSelectedPage, () => ThumbnailListViewModel.SelectedItem != null);
            ThumbnailListViewModel.BindTo(x => x.SelectedItem, SelectionChanged);
        }

        private void ShowingDetais(DetailsViewModel obj)
        {
            obj.DisplayName = "Foo";
        }

        public DelegateCommand<DetailsViewModel> ShowDetailsCommand { get; }
        

        private void RemoveSelectedPage()
        {
            var item = ThumbnailListViewModel.SelectedItem;
            item.IsDeleted = !item.IsDeleted;
        }

        private void AddPage()
        {
            for (int i = 0; i < 1000; i++)
            {
                ThumbnailListViewModel.Thumbnails.Add(new PageItem
                {
                    PageNumber = (ThumbnailListViewModel.Thumbnails.LastOrDefault()?.PageNumber).GetValueOrDefault(0) + 1
                });
            }
        }

        public ThumbnailListViewModel ThumbnailListViewModel { get; }

        public RenderPageViewModel RenderPageViewModel { get; }

        public ICommand AddPageCommand { get; set; }

        public ICommand RemovePageCommand { get; set; }

        public FlyinBarViewModel FlyinBarViewModel { get; }

        private void SelectionChanged(PageItem obj)
        {
            RenderPageViewModel.TargetItem = obj;
            RemovePageCommand.RaiseCanExecuteChanged();
        }

        protected override void OnActivate()
        {
            RenderPageViewModel.TargetItem = ThumbnailListViewModel.SelectedItem;
            base.OnActivate();
        }
    }
}