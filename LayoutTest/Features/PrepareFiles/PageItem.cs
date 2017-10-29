using System.Windows.Input;
using Caliburn.Micro;
using LayoutTest.Features.Shared;
using LayoutTest.Features.Shared.State;

namespace LayoutTest.Features.PrepareFiles
{
    public class PageItem : PropertyChangedBase
    {
        private bool isSelected;

        public PageItem()
        {
        }
        public PageItem(Page x)
        {
            IsDeleted = x.IsDeleted;
            PageNumber = x.PageNumber;
        }

        public int PageNumber { get; set; }

        public bool IsDeleted { get; set; }

        public bool Select1 { get; set; }

        public bool Select2 { get; set; }

        public bool IsSelected
        {
            get { return isSelected; }
            set
            {
                if (value == isSelected) return;
                isSelected = value;
                NotifyOfPropertyChange();
            }
        }
    }
}