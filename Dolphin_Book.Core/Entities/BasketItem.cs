using Dolphin_Book.Core.Entities.BaseEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dolphin_Book.Core.Entities
{
    public class BasketItem:BaseEntity
    {
        public int BasketId { get; set; }
        public Basket Basket { get; set; }
        public int? BookId { get; set; }
        public int? ToyId { get; set; }
        public Book? book { get; set; }
        public Toy? toy { get; set; }
        public int Count { get; set; }
        public string? type { get; set; }
    }
}
