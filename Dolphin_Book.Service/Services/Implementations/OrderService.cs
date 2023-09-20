using Dolphin_Book.Core.Entities;
using Dolphin_Book.Core.ViewModels;
using Dolphin_Book.Data.Contexts;
using Dolphin_Book.Service.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Dolphin_Book.Service.Services.Implementations
{
    public class OrderService:IOrderService
    {
        private readonly DolphinDbContext _context;
        private readonly IHttpContextAccessor _httpContext;
        private readonly UserManager<User> _userManager;
        private readonly IBasketService _basketService;

        public OrderService(DolphinDbContext context, IHttpContextAccessor httpContext, UserManager<User> userManager, IBasketService basketService)
        {
            _context = context;
            _httpContext = httpContext;
            _userManager = userManager;
            _basketService = basketService;
        }
        public async Task<int> AddOrder(double totalPrice)
        {
           List<BasketListItemVM>? vm =await _basketService.GetAll();
            if (_httpContext.HttpContext.User.Identity.IsAuthenticated)
            {
                User user = await _userManager.FindByNameAsync(_httpContext.HttpContext.User.Identity.Name);
                Order? order = await _context.Order.
                    Include(x=>x.OrderItems.Where(x=>x.IsDeleted)).ThenInclude(x=>x.orderProducts).
                    Where(x => !x.IsDeleted && x.UserId == user.Id).FirstOrDefaultAsync();
                if(order == null)
                {
                    order = new Order()
                    {
                        UserId = user.Id,
                        CreatedAt = DateTime.Now,
                    };
                    await _context.AddAsync(order);
                    OrderItem item = new OrderItem()
                    {
                        Order = order,
                        Status = "Hazırlanır",
                        CreatedAt= DateTime.Now,
                        TotalPrice = totalPrice,
                        
                    };
                    await _context.AddAsync(item);
                    foreach(var product in vm)
                    {
                            OrderProduct orderProduct = new OrderProduct()
                            {
                                OrderItem = item,
                                CreatedAt = DateTime.Now,
                                Name = product.Name,
                                PublisherName = product.PublisherName,
                                Count = product.Count,
                                Image = product.Image,
                                type = product.type,

                            };
                        await _context.AddAsync(orderProduct);
                    }
                    return item.Id;
                }
                else
                {
                    OrderItem item = new OrderItem()
                    {
                        Order = order,
                        Status = "Hazırlanır",
                        CreatedAt = DateTime.Now,
                        TotalPrice = totalPrice,
                    };
                    await _context.AddAsync(item);
                    foreach (var product in vm)
                    {

                        OrderProduct orderProduct = new OrderProduct()
                        {
                            OrderItem = item,
                            CreatedAt = DateTime.Now,
                            Name = product.Name,
                            PublisherName = product.PublisherName,
                            Count = product.Count,
                            Image = product.Image,
                            type = product.type,

                        };
                        await _context.AddAsync(orderProduct);
                    }

                    await _context.SaveChangesAsync();
                    return item.Id;
                }
            }
            return 0;
        }

        public async Task<List<OrderListItemVM>> GetOrders()
        {
            if (_httpContext.HttpContext.User.Identity.IsAuthenticated)
            {
                User? user = await _userManager.FindByNameAsync(_httpContext.HttpContext.User.Identity.Name);
                Order? order = await _context.Order.Where(x => !x.IsDeleted && x.UserId == user.Id).
                    Include(x => x.OrderItems.Where(x => !x.IsDeleted)).FirstOrDefaultAsync();
                if (order is not null)
                {
                    List<OrderListItemVM> orderListItemVMs = new List<OrderListItemVM>();
                    foreach (var item in order.OrderItems)
                    {
                        orderListItemVMs.Add(new OrderListItemVM
                        {
                            TotalPrice = item.TotalPrice,
                            Status = item.Status,
                            CreatedAt = item.CreatedAt,
                            Id = item.Id,
                        });
                    }
                    return orderListItemVMs;
                }

            }
            return new List<OrderListItemVM>();
        }
    }
}
