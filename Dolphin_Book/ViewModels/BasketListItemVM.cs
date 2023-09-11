using Dolphin_Book.Core.Entities;

namespace Dolphin_Book.ViewModels
{
    public class BasketListItemVM
    {
        public string Name { get; set; }
        public double Price { get; set; }
        public string Image { get; set; }
        public int Count { get; set; }
        public int BookId { get; set; }
        public string AuthorName { get; set; }
    }
}
