using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using chtotonaASP.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace chtotonaASP.Controllers
{
    public class ReviewsController : Controller
    {
        private readonly AspnetContext _context;

        public ReviewsController(AspnetContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Reviews()
        {
            var reviews = await _context.Reviews
                .Include(r => r.User)
                .ToListAsync();

            var viewModel = new ReviewsViewModel
            {
                Review = reviews
            };
            return View(viewModel);
        }

        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Index()
        {
            var reviews = await _context.Reviews
                .Include(r => r.Product)
                .Include(r => r.User)
                .ToListAsync();
            return View(reviews);
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewData["ProductId"] = new SelectList(_context.CatLists, "ProductId", "NameProduct");
            ViewData["UserId"] = new SelectList(_context.Users, "IdUser", "FullName");
            return View();
        }

        [HttpPost]

        public async Task<IActionResult> Create(Review review)
        {
            if (ModelState.IsValid)
            {
                review.Created = DateTime.Now;
                _context.Reviews.Add(review);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewData["ProductId"] = new SelectList(_context.CatLists, "ProductId", "NameProduct", review.ProductId);
            ViewData["UserId"] = new SelectList(_context.Users, "IdUser", "FullName", review.UserId);
            return View(review);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var review = await _context.Reviews.FindAsync(id);
            if (review == null)
            {
                return NotFound();
            }
            ViewData["ProductId"] = new SelectList(_context.CatLists, "ProductId", "NameProduct", review.ProductId);
            ViewData["UserId"] = new SelectList(_context.Users, "IdUser", "FullName", review.UserId);
            return View(review);
        }

        [HttpPost]

        public async Task<IActionResult> Edit(int id, Review review)
        {
            if (id != review.ReviewId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {

                var existingReview = await _context.Reviews.FindAsync(id);
                if (existingReview == null)
                {
                    return NotFound();
                }

                existingReview.ProductId = review.ProductId;
                existingReview.UserId = review.UserId;
                existingReview.Rating = review.Rating;
                existingReview.Comment = review.Comment;

                _context.Update(existingReview);
                await _context.SaveChangesAsync();

                return RedirectToAction("Index");
            }
            ViewData["ProductId"] = new SelectList(_context.CatLists, "ProductId", "NameProduct", review.ProductId);
            ViewData["UserId"] = new SelectList(_context.Users, "IdUser", "FullName", review.UserId);
            return View(review);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var review = await _context.Reviews.FindAsync(id);
            if (review == null)
            {
                return NotFound();
            }

            _context.Reviews.Remove(review);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
