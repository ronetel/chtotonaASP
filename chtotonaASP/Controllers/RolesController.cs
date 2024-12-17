using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using chtotonaASP.Models;

namespace chtotonaASP.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class RolesController : Controller
    {
        private readonly AspnetContext _context;

        public RolesController(AspnetContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var roles = await _context.Roles.ToListAsync();
            ViewData["ErrorMessage"] = TempData["ErrorMessage"];
            return View(roles);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]

        public async Task<IActionResult> Create(Role role)
        {
            if (ModelState.IsValid)
            {
                _context.Add(role);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(role);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var role = await _context.Roles.FindAsync(id);
            if (role == null)
            {
                return NotFound();
            }
            return View(role);
        }

        [HttpPost]

        public async Task<IActionResult> Edit(int id, Role role)
        {
            if (id != role.IdRole)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _context.Update(role);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(role);
        }

        [HttpPost, ActionName("Delete")]

        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var role = await _context.Roles
                .Include(r => r.Users)
                .FirstOrDefaultAsync(r => r.IdRole == id);

            if (role == null)
            {
                return NotFound();
            }

            if (role.Users.Any())
            {
                TempData["ErrorMessage"] = "Невозможно удалить роль, так как у неё есть связанные пользователи.";
                return RedirectToAction(nameof(Index));
            }

            _context.Roles.Remove(role);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
