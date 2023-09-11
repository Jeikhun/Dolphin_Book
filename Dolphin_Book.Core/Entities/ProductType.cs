using Dolphin_Book.Core.Entities.BaseEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dolphin_Book.Core.Entities
{
    public class ProductType:BaseEntity
    {
        public string Type { get; set; }
		public List<Book>? Books { get; set; }
		public List<Toy>? Toys { get; set; }

    }
}
