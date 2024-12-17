using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using chtotonaASP.Models;

namespace chtotonaASP.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class ProductTypeController : Controller
    {
        private readonly AspnetContext _context;

        public ProductTypeController(AspnetContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var productTypes = await _context.ProductTypes.ToListAsync();
            return View(productTypes);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]

        public async Task<IActionResult> Create(ProductType productType)
        {
            if (ModelState.IsValid)
            {
                _context.Add(productType);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(productType);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productType = await _context.ProductTypes.FindAsync(id);
            if (productType == null)
            {
                return NotFound();
            }
            return View(productType);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, ProductType productType)
        {
            if (id != productType.ProductTypeId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _context.Update(productType);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            return View(productType);
        }

        [HttpPost, ActionName("Delete")]

        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var productType = await _context.ProductTypes.FindAsync(id);
            if (productType == null)
            {
                return NotFound();
            }

            _context.ProductTypes.Remove(productType);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
