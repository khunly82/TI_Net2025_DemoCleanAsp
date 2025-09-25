namespace TI_Net2025_DemoCleanAsp.Models
{
    public class PageIndex<T> where T : class
    {
        public List<T> Items { get; set; } = null!;
        public PageIndexMeta Meta { get; set; } = null!;
    }

    public class PageIndexMeta
    {
        public int ItemsCount { get; set; }
        public int Page { get; set; }
        public int TotalItems { get; set; }
        public int TotalPages { get; set; }
    }
}
