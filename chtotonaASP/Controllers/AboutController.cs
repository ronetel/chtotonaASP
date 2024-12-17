using Microsoft.AspNetCore.Mvc;

namespace chtotonaASP.Controllers
{
    public class AboutController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
