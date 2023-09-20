using Dolphin_Book.Data.Contexts;
using Dolphin_Book.Core.Entities;
using Dolphin_Book.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Dolphin_Toy.Controllers
{
    public class ToyController : Controller
    {
        private readonly DolphinDbContext _context;
        public ToyController(DolphinDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<IActionResult> Index(ToyVM model)
        {
            int? catId = model.CategoryId;
            //var Toys = FilterByCategory(model.toys, model.CategoryId);
            var pageCount = await GetPageCountAsync(model.Take);


            if (model.Page <= 0 || model.Page > pageCount) return NotFound();
            model = new ToyVM
            {
                toys = await FilterToyAsync(model),
                categories = await _context.Categories.Where(x => !x.IsDeleted).ToListAsync(),
                categoriesSelect = await _context.ToyCategories.Where(x => !x.IsDeleted).Select(x => new SelectListItem
                {
                    Text = x.Category.Name,
                    Value = x.Id.ToString()
                })
                .ToListAsync(),
                Publishers = await _context.Publishers.Where(x => !x.IsDeleted).ToListAsync(),
                PageCount = pageCount,
                Page = model.Page,
                Take = model.Take,

            };
            var queryable = model.toys.AsQueryable();
            var toys = await PaginateToysAsync(model.Page, model.Take, queryable);
            model.toys = toys;
            model.ToysCount = model.toys.Count;
            //var BC = _context.ToyCategories.Where(x => x.ToyId == 1);
            //List<Toy> BC1 = await _context.Toys.Where(x => x.ToyCategories.Any(x => x.CategoryId==2))
            //    .ToListAsync();
            return View(model);

        }
        private async Task<List<Toy>> FilterToyAsync(ToyVM model)
        {
            var products = FilterByCategory(model.CategoryId);
            products = FilterByPrice(products, model.MinPrice, model.MaxPrice);

            return await products.ToListAsync();
        }
        private IQueryable<Toy> FilterByCategory(int? categoryId)
        {
            if (categoryId == null || categoryId == 0)
            {
                IQueryable<Toy>? toys = _context?.Toys?.Where(x => !x.IsDeleted)?
                     .Include(x => x.ToyCategories)?
                     .ThenInclude(x => x.Category)
                     .Include(x => x.Publisher)
                     .Include(x => x.ProductType)
                     .Include(x => x.toyImages).Where(x=>!x.IsDeleted);

                return toys;

            }
            return _context.Toys.Where(x => x.ToyCategories.Any(x => x.CategoryId == categoryId))
                     .Include(x => x.Publisher)
                     .Include(x => x.ProductType)
                     .Include(x => x.toyImages).Where(x => !x.IsDeleted);

        }
        private async Task<List<Toy>> PaginateToysAsync(int page, int take, IQueryable<Toy> queryable)
        {





            IQueryable<Toy> item = queryable
                 .OrderByDescending(b => b.Id);

            List<Toy> toysItems = queryable
                 .OrderByDescending(b => b.Id)
                 .Skip((page - 1) * take).Take(take)
                 .ToList();
            return toysItems;

        }


        private async Task<int> GetPageCountAsync(int take)
        {

            var toysCount = await _context.Toys.CountAsync();

            return (int)Math.Ceiling((decimal)toysCount / take);

        }
        private IQueryable<Toy> FilterByPrice(IQueryable<Toy> products, double? minPrice, double? maxPrice)
        {
            if (minPrice != null || maxPrice != null)
            {
                return products = products.Where(p => p.SalePrice >= minPrice && p.SalePrice <= maxPrice);

            }
            return products;

        }


        public async Task<IActionResult> ToyDetails(int id)
        {
            Toy? toy = await _context.Toys.Where(x => !x.IsDeleted && x.Id == id)
                .Include(x => x.ToyCategories)
                 .ThenInclude(x => x.Category)
                 .Include(x=>x.toyImages)
                 .Include(x=>x.Publisher)
                .Include(x => x.Seller)
                    .FirstOrDefaultAsync();
            ToyVM vm = new ToyVM();
            vm.Id = toy.Id;
            vm.toy = toy;
            vm.StockCount = toy.StockCount;
            vm.CategoryType = "toy";
            if (vm == null)
            {
                return NotFound();
            }
            return View(vm);
        }
    }
}
