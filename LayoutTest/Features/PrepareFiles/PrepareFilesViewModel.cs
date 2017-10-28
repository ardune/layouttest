using Caliburn.Micro;
using LayoutTest.Features.Shared;

namespace LayoutTest.Features.PrepareFiles
{
    public class PrepareFilesViewModel : Screen, IViewModel
    {
        public ThumbnailListViewModel ThumbnailListViewModel { get; }

        public PrepareFilesViewModel(ThumbnailListViewModel thumbnailListViewModel)
        {
            ThumbnailListViewModel = thumbnailListViewModel;
        }

        protected override void OnActivate()
        {
            DisplayName = "asd";

            base.OnActivate();
        }
    }
}