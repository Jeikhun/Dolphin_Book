using Dolphin_Book.Core.Entities.BaseEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dolphin_Book.Core.Entities
{
    public class OrderItem:BaseEntity
    {
        public int OrderId { get; set; }
        public Order Order { get; set; }
        public List<OrderProduct>? orderProducts { get; set; }
        public string Status { get; set; } = "Yoldadır";
        public double TotalPrice { get; set; }
    }
}
