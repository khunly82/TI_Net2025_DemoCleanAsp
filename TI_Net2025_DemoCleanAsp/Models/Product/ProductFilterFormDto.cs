namespace TI_Net2025_DemoCleanAsp.Models.Product
{
    public class ProductFilterFormDto
    {
        public string? Name { get; set; }
        public int? MinPrice { get; set; }
        public int? MaxPrice { get; set; }
        public int? CategoryId { get; set; }
    }
}
