namespace TI_Net2025_DemoCleanAsp.DL.Entities
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; } = null!;
        public int Price { get; set; }
        public int CategoryId { get; set; }

        public Category? Category { get; set; }
    }
}
