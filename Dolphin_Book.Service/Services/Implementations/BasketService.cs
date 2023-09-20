using Dolphin_Book.Core.Entities;
using Dolphin_Book.Core.ViewModels;
using Dolphin_Book.Data.Contexts;
using Dolphin_Book.Service.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Dolphin_Book.Service.Services.Implementations
{
    public class BasketService : IBasketService
    {
        private readonly DolphinDbContext _context;
        private readonly IHttpContextAccessor _httpContext;
        private readonly UserManager<User> _userManager;

        public BasketService(DolphinDbContext context, IHttpContextAccessor httpContext, UserManager<User> userManager)
        {
            _context = context;
            _httpContext = httpContext;
            _userManager = userManager;
        }
        
        public async Task AddBasket(int id, int? count, string? type, int stockCount)
        {
            if (!await _context.Books.AnyAsync(x => x.Id == id && !x.IsDeleted) && type.ToLower()=="book")
            {
                throw new Exception("Not found");
            }
            else if (!await _context.Books.AnyAsync(x => x.Id == id && !x.IsDeleted) && type.ToLower() == "book")
            {
                throw new Exception("Not found");
            }

            if (_httpContext.HttpContext.User.Identity.IsAuthenticated)
            {
                User user = await _userManager.FindByNameAsync(_httpContext.HttpContext.User.Identity.Name);
                Basket? basket = await _context.Baskets.
                    Include(x => x.BasketItems.Where(y => !y.IsDeleted)).
                    Where(x => !x.IsDeleted && x.UserId == user.Id).FirstOrDefaultAsync();
                if (basket is null)
                {
                    basket = new Basket()
                    {
                        UserId = user.Id,
                        CreatedAt = DateTime.Now,
                    };
                    await _context.AddAsync(basket);
                    BasketItem basketItem = new BasketItem()
                    {
                        Basket = basket,
                        Count = count ?? 1,
                        CreatedAt = DateTime.Now,
                        type = type 
                    };
                    if (type.ToLower() == "book")
                    {
                        basketItem.BookId = id;
                    }
                    else
                    {
                        basketItem.ToyId = id;
                    }
                    await _context.AddAsync(basketItem);
                }
                else
                {
                    if (type.ToLower() == "book")
                    {
                        BasketItem? basketItem = basket.BasketItems.Where(x => x.BookId == id && !x.IsDeleted).FirstOrDefault();
                        if (basketItem is null)
                        {
                            basketItem = new BasketItem()
                            {
                                Basket = basket,
                                Count = count ?? 1,
                                CreatedAt = DateTime.Now,
                                BookId = id,
                                type = type
                            };
                            await _context.AddAsync(basketItem);
                        }
                        else
                        {
                            if (stockCount > basketItem.Count)
                                basketItem.Count += count ?? 1;
                            else
                            {
                                await _context.SaveChangesAsync();
                                throw new Exception("Not Found");
                            }
                        }
                    }
                    else
                    {
                        BasketItem basketItem = await _context.BasketItems.Where(x => x.ToyId == id && !x.IsDeleted).FirstOrDefaultAsync();
                        if (basketItem is null)
                        {
                            basketItem = new BasketItem()
                            {
                                Basket = basket,
                                Count = count ?? 1,
                                CreatedAt = DateTime.Now,
                                ToyId = id,
                                type = type
                            };
                            await _context.AddAsync(basketItem);
                        }

                        else
                        {
                            if(stockCount>basketItem.Count)
                            basketItem.Count += count ?? 1;
                            else
                            {
                                await _context.SaveChangesAsync();
                                throw new Exception("Not Found");
                            }
                        }
                    }
                }
                await _context.SaveChangesAsync();
            }
            else
            {
                var CookiesJson = _httpContext.HttpContext?.Request.Cookies["basket"];
                if (CookiesJson is null)
                {
                    List<BasketVM> baskets = new List<BasketVM>();
                    BasketVM basket = new BasketVM
                    {
                        Id = id,
                        Count = count ?? 1,
                        type = type
                    };
                    baskets.Add(basket);
                    CookiesJson = JsonConvert.SerializeObject(baskets);
                    _httpContext.HttpContext?.Response.Cookies.Append("basket", CookiesJson);
                }
                else
                {
                    List<BasketVM>? baskets = JsonConvert.
                        DeserializeObject<List<BasketVM>>(CookiesJson);
                    BasketVM? basket = baskets.FirstOrDefault(x => x.Id == id && x.type?.ToLower()==type.ToLower());
                    if (basket is null)
                    {
                        BasketVM basket1 = new BasketVM
                        {
                            Id = id,
                            Count = count ?? 1,
                            type=type
                        };
                        baskets.Add(basket1);
                    }
                    else
                    {
                        if (stockCount > basket.Count)
                        {
                            basket.Count += count ?? 1;

                        }

                    }
                    CookiesJson = JsonConvert.SerializeObject(baskets);
                    _httpContext.HttpContext?.Response.Cookies.Append("basket", CookiesJson);
                }
            }
        }

        public async Task<List<BasketListItemVM>> GetAll()
        {
            
            if (_httpContext.HttpContext.User.Identity.IsAuthenticated)
            {
                User user = await _userManager.FindByNameAsync(_httpContext.HttpContext.User.Identity.Name);
                Basket? basket = await _context.Baskets
                    .Include(x => x.BasketItems.Where(y => !y.IsDeleted))
                            .ThenInclude(x => x.toy)
                             .ThenInclude(x => x.toyImages)
                             .Include(x => x.BasketItems)
                             .ThenInclude(x => x.toy)
                             .ThenInclude(x => x.Publisher)
                             .Include(x => x.BasketItems)
                             .ThenInclude(x => x.book)
                             .ThenInclude(x => x.Author)
                             .Include(x => x.BasketItems)
                             .ThenInclude(x => x.book)
                             .ThenInclude(x => x.Publisher)
                               .Where(x => !x.IsDeleted && x.UserId == user.Id).FirstOrDefaultAsync();




                if (basket != null)
                {
                    List<BasketListItemVM> basketItemViewModels = new List<BasketListItemVM>();
                    foreach (var item in basket.BasketItems)
                    {
                        if (item.type.ToLower() == "book")
                        {
                            basketItemViewModels.Add(new BasketListItemVM
                            {
                                Image = item.book.Image,
                                Count = item.Count,
                                Name = item.book.Name,
                                Id = item.BookId,
                                Price = item.book.SalePrice,
                                AuthorName = item.book.Author.FullName,
                                PublisherName = item.book.Publisher.Name,
                                type = "book"
                                
                            });

                        }
                        else
                        {
                            basketItemViewModels.Add(new BasketListItemVM
                            {
                                Image = item.toy.toyImages.FirstOrDefault(x=>x.isMain).Image,
                                Count = item.Count,
                                Name = item.toy.Name,
                                Id = item.ToyId,
                                Price = item.toy.SalePrice,
                                PublisherName = item.toy.Publisher.Name,
                                type = "toy"
                            });;
                        }
                    }
                    return basketItemViewModels;
                }
            }
            else
            {
                var jsonBasket = _httpContext?.HttpContext?.Request.Cookies["basket"];


            if (jsonBasket != null)
            {
                List<BasketVM>? basketViewModels = JsonConvert
                         .DeserializeObject<List<BasketVM>>(jsonBasket);
                List<BasketListItemVM> basketItemViewModels = new();
                foreach (var item in basketViewModels)
                {
                    if (item.type.ToLower() == "book")
                    {
                        Book? book = await _context.Books
                                          .Where(x => !x.IsDeleted && x.Id == item.Id)
                                           .Include(x => x.Author)
                                           .Include(x=>x.ProductType)
                                           .Include(x=>x.Publisher)
                                           
                                           .FirstOrDefaultAsync();
						if (book != null)
						{
							basketItemViewModels.Add(new BasketListItemVM
							{
								Id = item.Id,
								Count = item.Count,
								Image = book.Image,
								Name = book.Name,
								Price = book.SalePrice,
                                AuthorName = book.Author.FullName,
                                PublisherName = book.Publisher.Name,
                                type=book.ProductType.Type
							});

						}
					}
                    else
                    {
						Toy? toy = await _context.Toys
										  .Where(x => !x.IsDeleted && x.Id == item.Id)
										   .Include(x => x.Publisher)
                                           .Include(x => x.ProductType)
                                           .Include(x=>x.toyImages)
										   .FirstOrDefaultAsync();
						if (toy != null)
						{
							basketItemViewModels.Add(new BasketListItemVM
							{
								Id = item.Id,
								Count = item.Count,
								Image = toy.toyImages?.FirstOrDefault(x=>x.isMain).Image,
								Name = toy.Name,
								Price = toy.SalePrice,
								PublisherName = toy.Publisher?.Name,
                                type=toy.ProductType?.Type
							});

						}
					}

                    
                }
                return basketItemViewModels;
            }
        }
            return new List<BasketListItemVM>();
        }

        public async Task Remove(int id, string? type)
        {
            if (_httpContext.HttpContext.User.Identity.IsAuthenticated)
            {
                User user = await _userManager.FindByNameAsync(_httpContext.HttpContext.User.Identity.Name);
                Basket? basket = await _context.Baskets.
                    Include(x => x.BasketItems.Where(y => !y.IsDeleted)).
                    Where(x => !x.IsDeleted && x.UserId == user.Id).FirstOrDefaultAsync();
                if (type.ToLower() == "book")
                {
                    BasketItem basketItem = await _context.BasketItems.Where(x => !x.IsDeleted && x.BasketId == basket.Id && x.BookId == id).FirstOrDefaultAsync();
                    if (basketItem is not null)
                    {
                        basketItem.IsDeleted = true;
                        await _context.SaveChangesAsync();
                    }

                }
                else
                {
                    BasketItem basketItem = await _context.BasketItems.Where(x => !x.IsDeleted && x.BasketId == basket.Id && x.ToyId == id).FirstOrDefaultAsync();
                    if (basketItem is not null)
                    {
                        basketItem.IsDeleted = true;
                        await _context.SaveChangesAsync();
                    }
                }
            }
            else
            {
                var JsonBasket = _httpContext.HttpContext.Request.Cookies["basket"];

                if (JsonBasket is not null)
                {
                    List<BasketVM>? baskets = JsonConvert.DeserializeObject<List<BasketVM>>(JsonBasket);
                    BasketVM basket = baskets.FirstOrDefault(x => x.Id == id && x.type==type);
                    if (basket is not null)
                    {
                        baskets.Remove(basket);
                        JsonBasket = JsonConvert.SerializeObject(baskets);
                        _httpContext.HttpContext.Response.Cookies.Append("basket", JsonBasket);
                    }
                }
            }
        }
        public async Task RemoveAll()
        {
            if (_httpContext.HttpContext.User.Identity.IsAuthenticated)
            {
                User user = await _userManager.FindByNameAsync(_httpContext.HttpContext.User.Identity.Name);
                Basket? basket = await _context.Baskets.
                    Include(x => x.BasketItems.Where(y => !y.IsDeleted)).
                    Where(x => !x.IsDeleted && x.UserId == user.Id).FirstOrDefaultAsync();
                foreach(var item in basket.BasketItems)
                {
                    _context.BasketItems.Remove(item);
                }
                await _context.SaveChangesAsync();
            }
        }
    }
}
