using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using chtotonaASP.Models;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace chtotonaASP.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class UsersController : Controller
    {
        private readonly AspnetContext _context;

        public UsersController(AspnetContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var users = await _context.Users.ToListAsync();
            ViewData["ErrorMessage"] = TempData["ErrorMessage"];
            return View(users);
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewData["RoleId"] = new SelectList(_context.Roles, "IdRole", "NameRole");
            return View();
        }

        [HttpPost]

        public async Task<IActionResult> Create(User user)
        {
            if (ModelState.IsValid)
            {
                user.PasswordHash = HashPassword(user.PasswordHash);
                _context.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["RoleId"] = new SelectList(_context.Roles, "IdRole", "NameRole", user.RoleId);
            return View(user);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            ViewData["RoleId"] = new SelectList(_context.Roles, "IdRole", "NameRole", user.RoleId);
            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, User user)
        {
            if (id != user.IdUser)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {

                var existingUser = await _context.Users.FindAsync(id);
                if (existingUser == null)
                {
                    return NotFound();
                }

                if (!string.IsNullOrEmpty(user.PasswordHash))
                {
                    user.PasswordHash = HashPassword(user.PasswordHash);
                }
                else
                {
                    user.PasswordHash = existingUser.PasswordHash;
                }

                _context.Entry(existingUser).CurrentValues.SetValues(user);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            ViewData["RoleId"] = new SelectList(_context.Roles, "IdRole", "NameRole", user.RoleId);
            return View(user);
        }

        [HttpPost, ActionName("Delete")]

        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var user = await _context.Users
                .Include(u => u.ProductsInOrders)
                .Include(u => u.Reviews)
                .FirstOrDefaultAsync(u => u.IdUser == id);

            if (user == null)
            {
                return NotFound();
            }

            if (user.ProductsInOrders.Any() || user.Reviews.Any())
            {
                TempData["ErrorMessage"] = "Невозможно удалить пользователя, так как у него есть связанные записи.";
                return RedirectToAction(nameof(Index));
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hashedBytes);
            }
        }
    }
}
