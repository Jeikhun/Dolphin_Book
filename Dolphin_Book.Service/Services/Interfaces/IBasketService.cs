using Dolphin_Book.Core.Entities;
using Dolphin_Book.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dolphin_Book.Service.Services.Interfaces
{
    public interface IBasketService
    {
        public Task AddBasket(int id, int? count, string? type, int stockCount);
        public Task<List<BasketListItemVM>> GetAll();
        public Task Remove(int id, string? type);
        public Task RemoveAll();

    }

}
