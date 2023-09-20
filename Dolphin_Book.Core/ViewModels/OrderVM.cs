using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dolphin_Book.Core.ViewModels
{
    public class OrderVM
    {
        public int Id { get; set; }
        public int Count { get; set; }
        public string? type { get; set; }
    }
}
