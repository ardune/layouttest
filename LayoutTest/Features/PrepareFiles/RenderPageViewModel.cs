using Caliburn.Micro;
using LayoutTest.Features.Shared;

namespace LayoutTest.Features.PrepareFiles
{
    public class RenderPageViewModel : PropertyChangedBase, IViewModel
    {
        private PageItem targetItem;

        public RenderPageViewModel()
        {
            if (Execute.InDesignMode)
            {
                SetDesignValues();
            }
        }

        public PageItem TargetItem
        {
            get { return targetItem; }
            set
            {
                if (value == targetItem) return;
                targetItem = value;
                NotifyOfPropertyChange();
            }
        }
        
        private void SetDesignValues()
        {
            TargetItem = new PageItem
            {
                PageNumber = 3
            };
        }
    }
}