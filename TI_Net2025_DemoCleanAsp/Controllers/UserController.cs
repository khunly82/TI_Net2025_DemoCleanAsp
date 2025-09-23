using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TI_Net2025_DemoCleanAsp.BLL.Services;
using TI_Net2025_DemoCleanAsp.DL.Entities;
using TI_Net2025_DemoCleanAsp.Extensions;
using TI_Net2025_DemoCleanAsp.Mappers;
using TI_Net2025_DemoCleanAsp.Models.Users;

namespace TI_Net2025_DemoCleanAsp.Controllers
{
    public class UserController : Controller
    {
        private readonly UserService _userService;

        public UserController(UserService userService)
        {
            _userService = userService;
        }

        public IActionResult Register()
        {
            return View(new RegisterFormDto());
        }

        [HttpPost]
        public IActionResult Register([FromForm] RegisterFormDto form)
        {
            if(!ModelState.IsValid)
            {
                form.Password = "";
                form.ConfirmPassword = "";
                return View(form);
            }

            try
            {
                _userService.Register(form.ToUser());
                
                return RedirectToAction("Login","User");

            } catch(Exception ex)
            {
                form.Password = "";
                form.ConfirmPassword = "";
                return View(form);
            }
        }

        public IActionResult Login()
        {
            return View(new LoginFormDto());
        }

        [HttpPost]
        public IActionResult Login([FromForm] LoginFormDto form)
        {
            if(!ModelState.IsValid)
            {
                form.Password = "";
                return View(form);
            }

            try
            {
                User user = _userService.Login(form.Email, form.Password);

                ClaimsPrincipal claims = new ClaimsPrincipal(
                        new ClaimsIdentity([
                                new Claim(ClaimTypes.Sid,user.Id.ToString()),
                                new Claim(ClaimTypes.Email,user.Email),
                                new Claim(ClaimTypes.Role,user.Role.ToString()),
                            ], CookieAuthenticationDefaults.AuthenticationScheme)
                    );

                HttpContext.SignInAsync(claims);

                return RedirectToAction("Index", "Home");

            }catch (Exception ex)
            {
                ModelState.AddModelError("error",ex.Message);
                form.Password = "";
                return View(form);
            }
        }

        [Authorize]
        [HttpPost]
        public IActionResult Logout()
        {

            string email = User.GetEmail();

            Console.WriteLine(email);

            HttpContext.SignOutAsync();

            return RedirectToAction("Login", "User");
        }
    }
}
