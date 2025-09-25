namespace TI_Net2025_DemoCleanAsp.Models.Product
{
    public class ProductFilterFormDto
    {
        public string? Name { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public int? CategoryId { get; set; }
    }
}
