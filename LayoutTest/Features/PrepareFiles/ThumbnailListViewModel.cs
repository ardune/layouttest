﻿using System.Collections.ObjectModel;
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

        private void SetDesignValues()
        {
            var all = Enumerable.Range(96, 120).Select(x => new PageItem
            {
                PageNumber = x + 1,
                IsDeleted = (x + 1) % 2 == 0,
                Select1 = (x + 1) % 4 > 1,
                Select2 = (x + 1) % 5 > 2
            });

            foreach (var pageItem in all)
            {
                Thumbnails.Add(pageItem);
            }
            SelectedItem = Thumbnails[2];
        }
    }
}