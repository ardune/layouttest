namespace LayoutTest.Features.Shared.State
{
    public struct AppState
    {
        public Page[] Pages { get; set; }

        public Tag[] Tags { get; set; }

        public PageTag[] PageTags { get; set; }

        public PrepActivity PrepActivity { get; set; }
    }
}