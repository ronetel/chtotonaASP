using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using chtotonaASP.Models;
using System.Security.Claims;
using System.Text;
using System.Security.Cryptography;

namespace chtotonaASP.Controllers
{
    public class AccountController : Controller
    {
        private readonly AspnetContext _context;
        private readonly ILogger<AccountController> _logger;

        public AccountController(AspnetContext context, ILogger<AccountController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                string hash = HashPassword(model.Password);
                var user = _context.Users.FirstOrDefault(u => u.Email == model.Email && u.PasswordHash == hash);
                if (user != null )
                {
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, user.FullName),
                        new Claim(ClaimTypes.Role, user.RoleId == 1 ? "Customer":"Administrator"),
                        new Claim(ClaimTypes.Email, user.Email),
                        new Claim("UserId", user.IdUser.ToString())
                    };

                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                    if (User.IsInRole("Administrator")) return RedirectToAction("Index", "Users");
                    if (User.IsInRole("Customer")) return RedirectToAction("Cart", "Cart");
                }
                ModelState.AddModelError(string.Empty, "Неверный логин или пароль");
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (_context.Users.Any(u => u.Email == model.Email))
            {
                ViewBag.ErrorMessage = "Пользователь с таким логином существует";
                return View();
            }
            if (ModelState.IsValid)
            {
                var user = new User
                {
                    Email = model.Email,
                    FullName = model.FullName,
                    Phone = model.Phone,
                    RoleId = 1
                };
                user.PasswordHash = HashPassword(model.Password);

                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                return RedirectToAction("Login", "Account");
            }
            return View(model);
        }

        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hashedBytes);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account");
        }
    }
}
