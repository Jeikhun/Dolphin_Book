using Microsoft.AspNetCore.Mvc;

namespace Dolphin_Book.Controllers
{
    public class Payment : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
