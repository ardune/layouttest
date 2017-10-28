using System.Linq;
using Caliburn.Micro;
using LayoutTest.Features.Shared;

namespace LayoutTest.Features.PrepareFiles
{
    public class ThumbnailListViewModel : PropertyChangedBase, IViewModel
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
            Thumbnails.AddRange(Enumerable.Range(0, 10000).Select(x => new PageItem
            {
                PageNumber = x + 1,
                IsDeleted = (x+1)%2 == 0,
                Select1 = (x+1) %4 > 1,
                Select2 = (x+1) %5 > 2
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

    public class PageItem : PropertyChangedBase
    {
        private int pageNumber;
        private bool isDeleted;
        private bool select1;
        private bool select2;

        public int PageNumber
        {
            get { return pageNumber; }
            set
            {
                if (value == pageNumber) return;
                pageNumber = value;
                NotifyOfPropertyChange();
            }
        }

        public bool IsDeleted
        {
            get { return isDeleted; }
            set
            {
                if (value == isDeleted) return;
                isDeleted = value;
                NotifyOfPropertyChange();
            }
        }

        public bool Select1
        {
            get { return select1; }
            set
            {
                if (value == select1) return;
                select1 = value;
                NotifyOfPropertyChange();
            }
        }

        public bool Select2
        {
            get { return select2; }
            set
            {
                if (value == select2) return;
                select2 = value;
                NotifyOfPropertyChange();
            }
        }
    }
}