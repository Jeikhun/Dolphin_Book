using Dolphin_Book.Core.Entities;
using Dolphin_Book.Data.Contexts;
using Dolphin_Book.Service.Services.Interfaces;
using Dolphin_Book.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NuGet.ContentModel;

namespace Dolphin_Book.Controllers
{
    public class BasketController : Controller
    {
        private readonly DolphinDbContext _context;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly UserManager<User> _userManager;
        private readonly IBasketService _basketService;

        public BasketController(DolphinDbContext context, IHttpContextAccessor contextAccessor, UserManager<User> userManager, IBasketService basketService)
        {
            _context = context;
            _contextAccessor = contextAccessor;
            _userManager = userManager;
            _basketService = basketService;
        }
        [HttpPost]
        public async Task<IActionResult> AddBasket(int id, int? count, string? type, int stockCount)
        {
            await _basketService.AddBasket(id, count, type, stockCount);

            return Json(new { status = 200 });
        }


        public async Task<IActionResult> GetAll()
        {
            var result = await _basketService.GetAll();
             return Json(result);

        }

        public async Task<IActionResult> Remove(int id, string type )
        {
            await _basketService.Remove(id, type);
            return Redirect(Request.Headers["Referer"].ToString());
        }
        public async Task<IActionResult> RemoveAll()
        {
            await _basketService.RemoveAll();
            return RedirectToAction("index", "home");
        }

        [HttpGet]
        public async Task<IActionResult> Basket()
        {
            Setting? setting = await _context.Settings.FirstOrDefaultAsync(x=>!x.IsDeleted);

            return View(setting);
        }



        [HttpPost]
        public async Task<IActionResult> IncreaseCount(int id, string? type)
        {
            if (_contextAccessor.HttpContext.User.Identity.IsAuthenticated)
            {

                User? user = await _userManager.FindByNameAsync(_contextAccessor.HttpContext.User.Identity.Name);

                var basket = await _context.Baskets.FirstOrDefaultAsync(b => b.UserId == user.Id);
                if (basket is null)
                    return NotFound("Basket not Found");
                if(type.ToLower() == "book")
                {
                    var basketProduct = await _context.BasketItems.FirstOrDefaultAsync(bp =>!bp.IsDeleted && bp.BookId == id && bp.BasketId == basket.Id);
                    if (basketProduct is null) return NotFound("No such product is found in Basket");
                        var product = await _context.Books.FindAsync(basketProduct.BookId);
                        if (product is null)
                            return NotFound("Product is not found");
                        if (product.StockCount <= basketProduct.Count)
                            return NotFound("No more left in stock");
                        basketProduct.Count++;
                        _context.BasketItems.Update(basketProduct);
                }
                else
                {
                    var basketProduct = await _context.BasketItems.FirstOrDefaultAsync(bp =>!bp.IsDeleted && bp.ToyId == id && bp.BasketId == basket.Id);
                    if (basketProduct is null) return NotFound("No such product is found in Basket");
                        var product = await _context.Toys.FindAsync(basketProduct.ToyId);
                        if (product is null)
                            return NotFound("Product is not found");
                        if (product.StockCount <= basketProduct.Count)
                            return NotFound("No more left in stock");
                        basketProduct.Count++;
                        _context.BasketItems.Update(basketProduct);
                }
                await _context.SaveChangesAsync();

                return Ok();
            }
                return Ok();
            //    List<BasketVM> basket = JsonConvert.DeserializeObject<List<BasketVM>>(Request.Cookies["basket"]);

            //    var book = await _context.Books.FindAsync(id);
            //    if (book is null)
            //        return NotFound("Product is not found");

            //    var basketProduct = basket.Find(bp => bp.Id == book.Id);
            //    if (basketProduct is not null)
            //    {
            //        if (book.Count == basketProduct.Count)
            //            return NotFound("No more left at stock");

            //        basketProduct.Count++;
            //    }

            //    Response.Cookies.Append("basket", JsonConvert.SerializeObject(basket));
            //    return RedirectToAction(nameof(Index));
            //}
        }


        [HttpPost]
        public async Task<IActionResult> DecreaseCount(int id, string? type)
        {
            if (_contextAccessor.HttpContext.User.Identity.IsAuthenticated)
            {

                User user = await _userManager.FindByNameAsync(_contextAccessor.HttpContext.User.Identity.Name);

                var basket = await _context.Baskets.FirstOrDefaultAsync(b =>b.UserId == user.Id);
                if (basket is null)
                    return NotFound("Basket not Found");
                if (type.ToLower() == "book")
                {
                    var basketProduct = await _context.BasketItems.FirstOrDefaultAsync(bp =>!bp.IsDeleted && bp.BookId == id && bp.BasketId == basket.Id);
                    if (basketProduct is null) return NotFound("No such product is found in Basket");
                    var product = await _context.Books.FindAsync(basketProduct.BookId);
                    if (product is null)
                        return NotFound("Product is not found");
                    if (basketProduct.Count<2)
                        return NotFound("Daha az məhsul qeyd edə bilməzsiniz...");
                    basketProduct.Count--;
                    _context.BasketItems.Update(basketProduct);
                }
                else
                {
                    var basketProduct = await _context.BasketItems.FirstOrDefaultAsync(bp =>!bp.IsDeleted && bp.ToyId == id && bp.BasketId == basket.Id);
                    if (basketProduct is null) return NotFound("No such product is found in Basket");
                    var product = await _context.Toys.FindAsync(basketProduct.ToyId);
                    if (product is null)
                        return NotFound("Product is not found");
                    if (basketProduct.Count<2)
                        return NotFound("Daha az məhsul qeyd edə bilməzsiniz...");
                    basketProduct.Count--;
                    _context.BasketItems.Update(basketProduct);
                }
                await _context.SaveChangesAsync();

                return Ok();
            }
            //    List<BasketVM> basket = JsonConvert.DeserializeObject<List<BasketVM>>(Request.Cookies["basket"]);

            //    var book = await _context.Books.FindAsync(id);
            //    if (book is null)
            //        return NotFound("Product is not found");

            //    var basketProduct = basket.Find(bp => bp.Id == book.Id);
            //    if (basketProduct is not null)
            //    {
            //        if (basketProduct.Count == 1)
            //            basket.Remove(basketProduct);

            //        basketProduct.Count--;
            //    }

            //    Response.Cookies.Append("basket", JsonConvert.SerializeObject(basket));
            //    return RedirectToAction(nameof(Index));
            return Ok();
        }
    }
}
