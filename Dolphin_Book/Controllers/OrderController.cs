using Dolphin_Book.Core.Entities;
using Dolphin_Book.Data.Contexts;
using Dolphin_Book.Service.Services.Implementations;
using Dolphin_Book.Service.Services.Interfaces;
using Dolphin_Book.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Net.Mail;

namespace Dolphin_Book.Controllers
{
    
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;
        private readonly IBasketService _basketService;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly UserManager<User> _userManager;
        private readonly DolphinDbContext _context;
        private readonly IWebHostEnvironment _env;

        public OrderController(IOrderService orderService, IBasketService basketService, IHttpContextAccessor contextAccessor, UserManager<User> userManager, DolphinDbContext context, IWebHostEnvironment env)
        {
            _orderService = orderService;
            _basketService = basketService;
            _contextAccessor = contextAccessor;
            _userManager = userManager;
            _context = context;
            _env = env;
        }

        public async Task<IActionResult> OrderHistory()
        {
            
            //await _orderService.AddOrder();

            return View();
        }
        [HttpGet]
        public async Task<IActionResult> OrderConfirmation()
        {
            return View();
        }

            [HttpPost]
        public async Task<IActionResult> OrderConfirmation(double price)
        {
            User user = await _userManager.FindByNameAsync(_contextAccessor.HttpContext.User.Identity.Name);
            Basket? basket = await _context.Baskets.
                Include(x => x.BasketItems.Where(y => !y.IsDeleted)).
                Where(x => !x.IsDeleted && x.UserId == user.Id).FirstOrDefaultAsync();
            List<Book>? books = await _context.Books.Where(x=>!x.IsDeleted).ToListAsync();
            List<Toy>? toys = await _context.Toys.Where(x=>!x.IsDeleted).ToListAsync();
            if (basket == null)
            {
                return View();
            }
            foreach(var product in basket.BasketItems)
            {
                
                if(product?.type?.ToLower() == "book"){
                    foreach(var book in books)
                    {
                        if (product.BookId == book.Id)
                        {
                            book.StockCount -= product.Count;
                            if(book.StockCount < 1)
                            {
                                book.IsDeleted = true;
                            }
                        }
                    }
                }
                else if (product?.type?.ToLower() == "toy")
                {
                    foreach (var toy in toys)
                    {
                        if (product.ToyId == toy.Id)
                        {
                            toy.StockCount -= toy.Count;
                            if (toy.StockCount < 1)
                            {
                                toy.IsDeleted = true;
                            }
                        }
                    }
                }
                else
                {
                    return View();
                }
                await _context.SaveChangesAsync();
            }
            PaymentConfirmVM vm = new PaymentConfirmVM();
            vm.Email = user.Email;
            vm.DateTime = DateTime.Now;
            vm.totalPrice = price;
            int productNumber =  await _orderService.AddOrder(price);
            if(productNumber == 0)
            {
                return NotFound();
            }
            else
            {
                vm.OrderNumber = productNumber.ToString();
            }
            await _basketService.RemoveAll();
            //var confirmationLink = Url.Action(nameof(ConfirmEmail), "account", new { token, email = user.Email }, Request.Scheme);
            //var message = new Message(new string[] { user.Email }, "Email Confirmation", confirmationLink);
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress("jeikhunjalil@gmail.com");
            mail.To.Add(user.Email);
            mail.Subject = "Sifarişiniz Təsdiqləndi";
            string body = string.Empty;
            string path = Path.Combine(_env.WebRootPath, "assets", "Templates", "orderemail.html");
            using (StreamReader SourceReader = System.IO.File.OpenText(path))
            {
                body = SourceReader.ReadToEnd();
            }
            body = body.Replace("{{Name}}", user.UserName);
            body = body.Replace("{{Text}}", "Sifarişiniz Təsdiqləndi");
            body = body.Replace("{{Price}}", vm.totalPrice?.ToString("0.##"));
            body = body.Replace("{{Code}}", vm.OrderNumber.ToString());
            body = body.Replace("{{Date}}", DateTime.Now.ToString());
            mail.Body = body;
            mail.IsBodyHtml = true;
            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp.gmail.com";
            smtp.EnableSsl = true;
            NetworkCredential networkCredential = new NetworkCredential("jeikhunjalil@gmail.com", "fdgxcltipvqqujug");
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = networkCredential;
            smtp.Port = 587;
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtp.Send(mail);
            return View(vm);
        }
    }
}
