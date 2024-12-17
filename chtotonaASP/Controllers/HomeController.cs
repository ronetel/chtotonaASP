using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using chtotonaASP.Models;
using Microsoft.Extensions.Logging;

namespace YourNamespace.Controllers
{
    
    public class HomeController : Controller
    {
        private readonly AspnetContext _context;

        public HomeController(AspnetContext context) { _context = context;}
        public IActionResult Index()
        {
            var viewModel = new HomeViewModel
            {
                CatList = _context.CatLists.ToList(),
                News = _context.News.ToList()
            };

            return View(viewModel);
        }
    }


}
