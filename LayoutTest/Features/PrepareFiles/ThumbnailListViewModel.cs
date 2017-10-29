using System.Linq;
using Caliburn.Micro;
using LayoutTest.Features.Shared;

namespace LayoutTest.Features.PrepareFiles
{
    public class ThumbnailListViewModel : Screen, IViewModel
    {
        private PageItem selectedItem;

        public ThumbnailListViewModel()
        {
            if (Execute.InDesignMode)
            {
                SetDesignValues();
            }
        }

        private void SetDesignValues()
        {
            Thumbnails.AddRange(Enumerable.Range(95, 120).Select(x => new PageItem
            {
                PageNumber = x + 1,
                IsDeleted = (x + 1) % 2 == 0,
                Select1 = (x + 1) % 4 > 1,
                Select2 = (x + 1) % 5 > 2
            }));

            SelectedItem = Thumbnails[3];
        }

        public BindableCollection<PageItem> Thumbnails { get; set; } = new BindableCollection<PageItem>();

        public PageItem SelectedItem
        {
            get { return selectedItem; }
            set
            {
                if (Equals(value, selectedItem)) return;
                selectedItem = value;
                NotifyOfPropertyChange();
            }
        }
    }
}