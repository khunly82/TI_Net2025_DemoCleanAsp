using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TI_Net2025_DemoCleanAsp.BLL.Services;
using TI_Net2025_DemoCleanAsp.DL.Entities;
using TI_Net2025_DemoCleanAsp.Extensions;
using TI_Net2025_DemoCleanAsp.Mappers;
using TI_Net2025_DemoCleanAsp.Models.CartItem;
using TI_Net2025_DemoCleanAsp.Models.Users;

namespace TI_Net2025_DemoCleanAsp.Controllers
{
    public class UserController : Controller
    {
        private readonly UserService _userService;
        private readonly CartService _cartService;

        public UserController(UserService userService, CartService cartService)
        {
            _userService = userService;
            _cartService = cartService;
        }

        public IActionResult Register()
        {
            return View(new RegisterFormDto());
        }

        [HttpPost]
        public IActionResult Register([FromForm] RegisterFormDto form)
        {
            if (!ModelState.IsValid)
            {
                form.Password = "";
                form.ConfirmPassword = "";
                return View(form);
            }

            _userService.Register(form.ToUser());

            return RedirectToAction("Login", "User");
        }

        public IActionResult Login()
        {
            return View(new LoginFormDto());
        }

        [HttpPost]
        public IActionResult Login([FromForm] LoginFormDto form)
        {
            if (!ModelState.IsValid)
            {
                form.Password = "";
                return View(form);
            }

            User user = _userService.Login(form.Email, form.Password);

            ClaimsPrincipal claims = new ClaimsPrincipal(
                    new ClaimsIdentity([
                            new Claim(ClaimTypes.Sid,user.Id.ToString()),
                                new Claim(ClaimTypes.Email,user.Email),
                                new Claim(ClaimTypes.Role,user.Role.ToString()),
                        ], CookieAuthenticationDefaults.AuthenticationScheme)
                );

            HttpContext.SignInAsync(claims);

            var sessionCart = HttpContext.Session.GetItem<List<CartItemSessionDto>>("cart");

            Cart? cart = null;

            if (sessionCart != null && sessionCart.Count != 0)
            {
                List<CartItem> cartItems = [.. sessionCart.Select(ci => ci.ToCartItem())];
                cart = _cartService.MergeCarts(user.Id, cartItems);
            }
            else
            {
                cart = _cartService.GetWithCartLineByUserId(user.Id);
            }

            if(cart != null )
            {
                sessionCart = [.. cart.Items.Select(i => i.ToCartItemSessionDto())];
                HttpContext.Session.SetItem("cart", sessionCart);
            }

            return RedirectToAction("Index", "Home");

        }

        [Authorize]
        [HttpPost]
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync();

            HttpContext.Session.Remove("cart");

            return RedirectToAction("Login", "User");
        }
    }
}
