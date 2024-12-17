using Microsoft.AspNetCore.Mvc;
using chtotonaASP.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace chtotonaASP.Controllers
{
    public class CartController : Controller
    {
        private readonly AspnetContext _context;

        public CartController(AspnetContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Cart()
        {
            var userId = User.FindFirstValue("UserId");
            if (userId == null) return RedirectToAction("Login", "Account");

            var productsInOrder = await _context.ProductsInOrders
                .Include(p => p.Product)
                .Where(p => p.UserId == int.Parse(userId))
                .ToListAsync();

            var totalPrice = productsInOrder.Sum(item => item.Quantity * item.Product.Price);

            var viewModel = new PinorViewModel
            {
                ProductsInOrder = productsInOrder,
                TotalSum = (int)totalPrice
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> IncreaseQuantity(int pinorId)
        {
            var cartItem = await _context.ProductsInOrders
                .Include(p => p.Product)
                .FirstOrDefaultAsync(p => p.PinorId == pinorId);

            if (cartItem != null && cartItem.Product != null)
            {
                cartItem.Quantity += 1;
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Cart");
        }

        [HttpPost]
        public async Task<IActionResult> DecreaseQuantity(int pinorId)
        {
            var cartItem = await _context.ProductsInOrders
                .Include(p => p.Product)
                .FirstOrDefaultAsync(p => p.PinorId == pinorId);

            if (cartItem != null && cartItem.Product != null)
            {
                if (cartItem.Quantity > 1)
                {
                    cartItem.Quantity -= 1;
                    await _context.SaveChangesAsync();
                }
                else
                {
                    _context.ProductsInOrders.Remove(cartItem);
                    await _context.SaveChangesAsync();
                }
            }
            return RedirectToAction("Cart");
        }

        public async Task<IActionResult> Index()
        {
            var productsInOrders = await _context.ProductsInOrders.Include(p => p.Product).Include(p => p.User).ToListAsync();
            return View(productsInOrders);
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewData["ProductId"] = new SelectList(_context.CatLists, "ProductId", "NameProduct");
            ViewData["UserId"] = new SelectList(_context.Users, "IdUser", "FullName");
            return View();
        }

        [HttpPost]

        public async Task<IActionResult> Create(ProductsInOrder productInOrder)
        {
            if (ModelState.IsValid)
            {
                productInOrder.Product = await _context.CatLists.FindAsync(productInOrder.ProductId);
                if (productInOrder.Product != null)
                {
                    _context.Add(productInOrder);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            ViewData["ProductId"] = new SelectList(_context.CatLists, "ProductId", "NameProduct", productInOrder.ProductId);
            ViewData["UserId"] = new SelectList(_context.Users, "IdUser", "FullName", productInOrder.UserId);
            return View(productInOrder);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productInOrder = await _context.ProductsInOrders.FindAsync(id);
            if (productInOrder == null)
            {
                return NotFound();
            }
            ViewData["ProductId"] = new SelectList(_context.CatLists, "ProductId", "NameProduct", productInOrder.ProductId);
            ViewData["UserId"] = new SelectList(_context.Users, "IdUser", "FullName", productInOrder.UserId);
            return View(productInOrder);
        }

        [HttpPost]

        public async Task<IActionResult> Edit(int id, ProductsInOrder productInOrder)
        {
            if (id != productInOrder.PinorId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _context.Update(productInOrder);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProductId"] = new SelectList(_context.CatLists, "ProductId", "NameProduct", productInOrder.ProductId);
            ViewData["UserId"] = new SelectList(_context.Users, "IdUser", "FullName", productInOrder.UserId);
            return View(productInOrder);
        }

        [HttpPost, ActionName("Delete")]

        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var productInOrder = await _context.ProductsInOrders.FindAsync(id);
            if (productInOrder == null)
            {
                return NotFound();
            }

            _context.ProductsInOrders.Remove(productInOrder);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
