using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using chtotonaASP.Models;

namespace chtotonaASP.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class NewsController : Controller
    {
        private readonly AspnetContext _context;

        public NewsController(AspnetContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var news = await _context.News.ToListAsync();
            return View(news);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]

        public async Task<IActionResult> Create(News news)
        {
            if (ModelState.IsValid)
            {
                _context.Add(news);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(news);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var news = await _context.News.FindAsync(id);
            if (news == null)
            {
                return NotFound();
            }
            return View(news);
        }

        [HttpPost]

        public async Task<IActionResult> Edit(int id, News news)
        {
            if (id != news.NewId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _context.Update(news);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            return View(news);
        }

        [HttpPost, ActionName("Delete")]

        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var news = await _context.News.FindAsync(id);
            if (news == null)
            {
                return NotFound();
            }

            _context.News.Remove(news);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
