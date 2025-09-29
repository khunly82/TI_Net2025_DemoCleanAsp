namespace TI_Net2025_DemoCleanAsp.Models.CartItem
{
    public class CartItemSessionDto
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }

        public string ProductName { get; set; } = null!;
        public int ProductPrice { get; set; }
    }
}
