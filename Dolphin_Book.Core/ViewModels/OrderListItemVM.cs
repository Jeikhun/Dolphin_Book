using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dolphin_Book.Core.ViewModels
{
    public class OrderListItemVM
    {
        public DateTime CreatedAt { get; set; }
        public string? Status { get; set; }
        public int Id { get; set; }
        public double TotalPrice { get; set; }

    }
}
