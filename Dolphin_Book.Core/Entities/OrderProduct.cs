using Dolphin_Book.Core.Entities.BaseEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dolphin_Book.Core.Entities
{
    public class OrderProduct:BaseEntity
    {
        public OrderItem OrderItem { get; set; }
        public int OrderItemId { get; set; }
        public string Name { get; set; }
        public string? PublisherName { get; set; }
        public string? Image { get; set; }
        public int Count { get; set; }
        public string? type { get; set; }
    }
}
