using Dolphin_Book.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Dolphin_Book.Controllers
{
    public class PaymentController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
           return NotFound();
        }

        [HttpPost]
        public IActionResult Index(string totalPriceSTR)
        {
            double totalPrice = double.Parse(totalPriceSTR);
            PaymentVM model = new PaymentVM
            {
                Price = totalPrice.ToString("0.##"),
            };
            return View(model);
        }
    }
}
