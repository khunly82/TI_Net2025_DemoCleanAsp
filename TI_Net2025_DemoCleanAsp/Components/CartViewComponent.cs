using Microsoft.AspNetCore.Mvc;
using TI_Net2025_DemoCleanAsp.Extensions;
using TI_Net2025_DemoCleanAsp.Models.CartItem;

namespace TI_Net2025_DemoCleanAsp.Components
{
    public class CartViewComponent: ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            int quantity = 0;

            if(!User.IsConnected())
            {
                var cart = HttpContext.Session.GetItem<List<CartItemSessionDto>>("cart") ?? [];

                quantity = cart.Sum(e => e.Quantity);
            }

            return View(quantity);
        }
    }
}
