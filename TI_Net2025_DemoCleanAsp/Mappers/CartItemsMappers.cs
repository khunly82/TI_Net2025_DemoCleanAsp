using TI_Net2025_DemoCleanAsp.DL.Entities;
using TI_Net2025_DemoCleanAsp.Models.CartItem;

namespace TI_Net2025_DemoCleanAsp.Mappers
{
    public static class CartItemsMappers
    {
        public static CartItem ToCartItem(this CartItemSessionDto dto)
        {
            return new CartItem()
            {
                ProductId = dto.ProductId,
                Quantity = dto.Quantity,
            };
        }

        public static CartItemSessionDto ToCartItemSessionDto(this CartItem ci)
        {
            return new CartItemSessionDto()
            {
                ProductId = ci.ProductId,
                Quantity = ci.Quantity,
            };
        }
    }
}
