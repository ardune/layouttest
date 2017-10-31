using System.Collections.Generic;
using System.Windows.Input;
using Caliburn.Micro;
using LayoutTest.Features.Shared;
using LayoutTest.Features.Shared.State;

namespace LayoutTest.Features.PrepareFiles
{
    public class PageItem
    {
        public int PageNumber { get; set; }

        public bool IsDeleted { get; set; }

        public IEnumerable<string> Tags { get; set; }
    }
}