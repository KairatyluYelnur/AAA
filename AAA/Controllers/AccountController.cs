using AAA.Data;
using AAA.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AAA.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AccountController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string username, string password, bool remember)
        {
            var user = _context.User
                .FirstOrDefault(u => u.Username == username && u.Password == password);

            if (user == null)
            {
                ViewBag.Error = "Неверный логин или пароль.";
                return View();
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username)
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            var authProperties = new AuthenticationProperties
            {
                IsPersistent = remember, // сохраняем куки между сессиями
                ExpiresUtc = DateTimeOffset.UtcNow.AddDays(7) // можно настроить срок действия
            };

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, authProperties);

            return RedirectToAction("Index", "Employees");
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(string username, string password, string confirmPassword)
        {
            if (password != confirmPassword)
            {
                ViewBag.Error = "Пароли не совпадают.";
                return View();
            }

            if (_context.User.Any(u => u.Username == username))
            {
                ViewBag.Error = "Пользователь с таким именем уже существует.";
                return View();
            }

            var user = new User
            {
                Username = username,
                Password = password // Временно. Позже — хеширование
            };

            _context.User.Add(user);
            _context.SaveChanges();

            return RedirectToAction("Login");
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Login");
        }

        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}

