using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dolphin_Book.Controllers
{
    
    public class OrderController : Controller
    {
        public IActionResult OrderHistory()
        {
            return View();
        }
    }
}
