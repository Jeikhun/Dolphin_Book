using Dolphin_Book.Core.Entities;
using Dolphin_Book.Data.Contexts;
using Dolphin_Book.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Dolphin_Book.Controllers
{
    public class BookController : Controller
    {
        private readonly DolphinDbContext _context;

        public BookController(DolphinDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<IActionResult> Index(BookVM model)
        {
            int? catId = model.CategoryId;
            //var Books = FilterByCategory(model.books, model.CategoryId);
            var pageCount = await GetPageCountAsync(model.Take);


            if (model.Page <= 0 || model.Page > pageCount) return NotFound();
            model = new BookVM
            {
                books = await FilterBookAsync(model),
                categories = await _context.Categories.Where(x => !x.IsDeleted).ToListAsync(),
                categoriesSelect = await _context.BookCategories.Where(x => !x.IsDeleted).Select(x => new SelectListItem
                {
                    Text = x.Category.Name,
                    Value = x.Id.ToString()
                })
                .ToListAsync(),
                Publishers = await _context.Publishers.Where(x => !x.IsDeleted).ToListAsync(),
                Authors = await _context.Authors.Where(x => !x.IsDeleted).ToListAsync(),
                PageCount = pageCount,
                Page = model.Page,
                Take = model.Take
            };
            var queryable = model.books.AsQueryable();
            var books = await PaginateBooksAsync(model.Page, model.Take, queryable);
            model.books = books;
            model.BooksCount = model.books.Count;
            //var BC = _context.BookCategories.Where(x => x.BookId == 1);
            //List<Book> BC1 = await _context.Books.Where(x => x.BookCategories.Any(x => x.CategoryId==2))
            //    .ToListAsync();
            return View(model);

        }
        private async Task<List<Book>> FilterBookAsync(BookVM model)
        {
            var products = FilterByCategory(model.CategoryId);
            products = FilterByPrice(products, model.MinPrice, model.MaxPrice);

            return await products.ToListAsync();
        }
        private IQueryable<Book> FilterByCategory(int? categoryId) 
        {
            if(categoryId == null || categoryId ==0)
            {
                var books = _context.Books.Where(x => !x.IsDeleted)
                     .Include(x => x.BookCategories)
                     .ThenInclude(x => x.Category)
                     .Include(x => x.Publisher)
                     .Include(x => x.Author)
                     .Include(x => x.ProductType);
                
                return books;

            }
            return _context.Books.Where(x =>x.BookCategories.Any(x => x.CategoryId == categoryId)).Include(x=>x.Author);

        }
        private async Task<List<Book>> PaginateBooksAsync(int page, int take, IQueryable<Book> queryable)
        {



            

            IQueryable<Book> item = queryable
                 .OrderByDescending(b => b.Id);

            List<Book> booksItems = queryable
                 .OrderByDescending(b => b.Id)
                 .Skip((page - 1) * take).Take(take)
                 .ToList();
            return booksItems;

        }


        private async Task<int> GetPageCountAsync(int take)
        {

            var booksCount = await _context.Books.CountAsync();

            return (int)Math.Ceiling((decimal)booksCount / take);

        }
        private IQueryable<Book> FilterByPrice(IQueryable<Book> products, double? minPrice, double? maxPrice)
        {
            if (minPrice != null || maxPrice != null)
            {
                return products = products.Where(p => p.SalePrice >= minPrice && p.SalePrice <= maxPrice);

            }
            return products;

        }


        public async Task<IActionResult> BookDetails(int id)
        {
            BookVM vm = new BookVM();
            vm.book = await _context.Books.Where(x => !x.IsDeleted && x.Id==id)
                .Include(x => x.BookCategories)
                 .ThenInclude(x => x.Category)
                .Include(x => x.Author)
                .Include(x => x.Seller)
            .Include(x => x.Language)
                    .FirstOrDefaultAsync();
            if (vm.book == null)
            {
                return NotFound();
            }
            return View(vm);
        }
    }
}
