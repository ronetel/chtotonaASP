using Microsoft.AspNetCore.Mvc;
using chtotonaASP.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace chtotonaASP.Controllers
{
    public class CatalogController : Controller
    {
        private readonly AspnetContext _context;

        public CatalogController(AspnetContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Catalog(string search, string sort, string filter)
        {
            var query = _context.CatLists.AsQueryable();

            switch (filter)
            {
                case "gem":
                    query = query.Where(p => p.IdproductType == 1);
                    break;
                case "bp":
                    query = query.Where(p => p.IdproductType == 2);
                    break;
                case "skins":
                    query = query.Where(p => p.IdproductType == 3);
                    break;
            }

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(p => p.NameProduct.Contains(search) || p.DescProduct.Contains(search));
            }

            switch (sort)
            {
                case "price_asc":
                    query = query.OrderBy(p => p.Price);
                    break;
                case "price_desc":
                    query = query.OrderByDescending(p => p.Price);
                    break;
                case "name_asc":
                    query = query.OrderBy(p => p.NameProduct);
                    break;
                case "name_desc":
                    query = query.OrderByDescending(p => p.NameProduct);
                    break;
                default:
                    query = query.OrderBy(p => p.ProductId);
                    break;
            }

            var catlist = await query.ToListAsync();
            var viewModel = new CatListViewModel
            {
                CatList = catlist,
                Search = search,
                Sort = sort,
                Filter = filter
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> AddToCart(int productId, int quantity)
        {
            var userId = User.FindFirstValue("UserId");
            if (userId == null) return RedirectToAction("Login", "Account");

            var product = await _context.CatLists.FindAsync(productId);
            if (product == null)
            {
                return NotFound();
            }

            var cartItem = await _context.ProductsInOrders
                .FirstOrDefaultAsync(x => x.ProductId == productId && x.UserId == int.Parse(userId));

            if (cartItem == null)
            {
                cartItem = new ProductsInOrder
                {
                    UserId = int.Parse(userId),
                    ProductId = productId,
                    Quantity = quantity,
                };
                _context.ProductsInOrders.Add(cartItem);
            }
            else
            {
                cartItem.Quantity += quantity;
                cartItem.Product.Price = product.Price * cartItem.Quantity;
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("Cart", "Cart");
        }

        public async Task<IActionResult> Index()
        {
            var products = await _context.CatLists.ToListAsync();
            ViewData["ErrorMessage"] = TempData["ErrorMessage"];
            return View(products);
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewData["IdproductType"] = new SelectList(_context.ProductTypes, "ProductTypeId", "ProductType1");
            return View();
        }

        [HttpPost]
         
        public async Task<IActionResult> Create(CatList product)
        {
            if (ModelState.IsValid)
            {
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdproductType"] = new SelectList(_context.ProductTypes, "ProductTypeId", "ProductType1", product.IdproductType);
            return View(product);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.CatLists.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            ViewData["IdproductType"] = new SelectList(_context.ProductTypes, "ProductTypeId", "ProductType1", product.IdproductType);
            return View(product);
        }

        [HttpPost]
         
        public async Task<IActionResult> Edit(int id, CatList product)
        {
            if (id != product.ProductId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdproductType"] = new SelectList(_context.ProductTypes, "ProductTypeId", "ProductType1", product.IdproductType);
            return View(product);
        }

        [HttpPost, ActionName("Delete")]
         
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.CatLists
                .Include(p => p.ProductsInOrders)
                .Include(p => p.Reviews)
                .FirstOrDefaultAsync(p => p.ProductId == id);

            if (product == null)
            {
                return NotFound();
            }

            if (product.ProductsInOrders.Any() || product.Reviews.Any())
            {
                TempData["ErrorMessage"] = "Невозможно удалить товар, так как у него есть связанные записи.";
                return RedirectToAction(nameof(Index));
            }

            _context.CatLists.Remove(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
