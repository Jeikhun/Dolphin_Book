using Dolphin_Book.Core.Entities;

namespace Dolphin_Book.Core.ViewModels
{
    public class BasketVM
    {
        public int Id { get; set; }
        public int Count { get; set; }
        public string? type { get; set; }
    }
}
