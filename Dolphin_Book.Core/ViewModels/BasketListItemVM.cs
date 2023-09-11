using Dolphin_Book.Core.Entities;

namespace Dolphin_Book.Core.ViewModels
{
    public class BasketListItemVM
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public string Image { get; set; }
        public int Count { get; set; }
        public string? AuthorName { get; set; }
        public string? PublisherName { get; set; }
        public string? type { get; set; }
    }
}
