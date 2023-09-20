using Dolphin_Book.Core.Entities;
using Dolphin_Book.Core.Entities.BaseEntities;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Dolphin_Book.ViewModels
{
    public class ToyVM:BaseEntity
    {
        public List<Toy>? toys { get; set; }
        public Toy? toy { get; set; }
        public List<Category>? categories { get; set; }
        public List<SelectListItem>? categoriesSelect { get; set; }
        public List<Publisher> Publishers { get; set; }
        public int? CategoryId { get; set; }
        public double? MinPrice { get; set; }
        public double? MaxPrice { get; set; }
        public int? ToysCount { get; set; }
        public int Page { get; set; } = 1;

        public int Take { get; set; } = 4;

        public int? PageCount { get; set; }
        public string? Image { get; set; }
        public string? CategoryType { get; set; }
        public int? StockCount { get; set; }
    }
}
