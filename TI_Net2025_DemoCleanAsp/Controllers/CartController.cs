using Microsoft.AspNetCore.Mvc;
using TI_Net2025_DemoCleanAsp.BLL.Services;
using TI_Net2025_DemoCleanAsp.Extensions;
using TI_Net2025_DemoCleanAsp.Models.CartItem;

namespace TI_Net2025_DemoCleanAsp.Controllers
{
    public class CartController : Controller
    {

        private readonly CartService _cartService;

        public CartController(CartService cartService)
        {
            _cartService = cartService;
        }

        public IActionResult AddItem([FromQuery] int productId)
        {
            var cartSession = HttpContext.Session.GetItem<List<CartItemSessionDto>>("cart") ?? [];

            CartItemSessionDto? item = cartSession.FirstOrDefault(p => p.ProductId == productId);

            if (item == null)
            {
                cartSession.Add(new CartItemSessionDto() { ProductId = productId, Quantity = 1 });
            }
            else
            {
                item.Quantity++;
            }

            HttpContext.Session.SetItem("cart", cartSession);

            if (User.IsConnected())
            {
                _cartService.AddCartItem(User.GetId(),productId);
            }

            return RedirectToAction("Index", "Product");
        }
    }
}
