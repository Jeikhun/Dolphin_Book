using Dolphin_Book.Core.Entities;
using Dolphin_Book.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dolphin_Book.Service.Services.Interfaces
{
    public interface IOrderService
    {
        public Task<int> AddOrder(double totalPrice);
        public Task<List<OrderListItemVM>> GetOrders();
    }
}
