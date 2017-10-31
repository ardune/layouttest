using System.Collections.Generic;

namespace LayoutTest.Features.Shared.State
{
    public struct PageTagView
    {
        public Page Page { get; set; }
        public IEnumerable<Tag> Tags { get; set; }
    }
}