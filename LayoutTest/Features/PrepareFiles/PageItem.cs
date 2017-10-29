using Caliburn.Micro;

namespace LayoutTest.Features.PrepareFiles
{
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