using Microsoft.AspNetCore.Mvc;
using TI_Net2025_DemoCleanAsp.BLL.Services;
using TI_Net2025_DemoCleanAsp.DL.Entities;
using TI_Net2025_DemoCleanAsp.Extensions;
using TI_Net2025_DemoCleanAsp.Models.CartItem;

namespace TI_Net2025_DemoCleanAsp.Controllers
{
    public class CartController : Controller
    {

        private readonly CartService _cartService;
        private readonly ProductService _productService;

        public CartController(CartService cartService, ProductService productService)
        {
            _cartService = cartService;
            _productService = productService;
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

        [HttpGet("api/cart")]
        public IActionResult GetCart()
        {
            var cart = HttpContext.Session.GetItem<List<CartItemSessionDto>>("cart") ?? [];

            foreach (var item in cart)
            {
                Product p = _productService.GetById(item.ProductId)!;
                item.ProductPrice = p.Price;
                item.ProductName = p.Name;
            }

            return Ok(cart);
        }

        [HttpDelete("api/cart/{id}")]
        public IActionResult DeleteItem([FromRoute]int id)
        {
            var cart = HttpContext.Session.GetItem<List<CartItemSessionDto>>("cart");
            if(cart != null)
            {
                cart = cart.Where(item => item.ProductId != id).ToList();
                HttpContext.Session.SetItem("cart", cart);
            }

            if(User.IsConnected())
            {
                _cartService.DeleteItem(id, User.GetId());
            }
            return Ok();
        }


    }
}
