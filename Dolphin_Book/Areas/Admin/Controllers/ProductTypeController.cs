using Dolphin_Book.Core.Entities;
using Dolphin_Book.Data.Contexts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Dolphin_Book.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "SuperAdmin, Admin")]
    public class ProductTypeController : Controller
    {
        private readonly DolphinDbContext _context;
        public ProductTypeController(DolphinDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            IEnumerable<ProductType> productTypes = await _context.ProductTypes
                .Where(x => !x.IsDeleted).ToListAsync();
            return View(productTypes);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductType productType)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }


            productType.CreatedAt = DateTime.Now;
            await _context.ProductTypes.AddAsync(productType);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            ProductType? productType = await _context.ProductTypes.Where(x => !x.IsDeleted && x.Id == id).FirstOrDefaultAsync();

            if (productType == null)
            {
                return NotFound();
            }
            return View(productType);

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, ProductType newProductType)
        {
            ProductType? productType = await _context.ProductTypes.Where(x => !x.IsDeleted && x.Id == id).FirstOrDefaultAsync();
            if (!ModelState.IsValid)
            {
                return View(productType);
            }

            if (productType == null)
            {
                return NotFound();
            }

            productType.Type = newProductType.Type;
            productType.UpdatedAt = DateTime.Now;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            ProductType? productType = await _context.ProductTypes.Where(x => !x.IsDeleted).FirstOrDefaultAsync();
            if (productType == null)
            {
                return NotFound();
            }

            productType.IsDeleted = true;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }

}
