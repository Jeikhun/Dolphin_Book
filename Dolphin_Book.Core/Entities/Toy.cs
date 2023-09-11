using Dolphin_Book.Core.Entities.BaseEntities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dolphin_Book.Core.Entities
{
    public class Toy:BaseEntity
    {
        public string Name { get; set; }
        public double PurshacePrice { get; set; }
        public double SalePrice { get; set; }
        public int StockCount { get; set; }
        public int Count { get; set; }
        public string Description { get; set; }
        public Seller? Seller { get; set; }
        public Publisher? Publisher { get; set; }
        public ProductType? ProductType { get; set; }

        public List<ToyImage>? toyImages { get; set; }
        [NotMapped]
        public List<IFormFile>? FormFiles { get; set; }
        [NotMapped]
        public List<int>? CategoryIds { get; set; }
        public int? PublisherId { get;set; }
        public int? SellerId { get; set; }
        public int? ProductTypeId { get; set; }

        public List<ToyCategory>? ToyCategories { get; set; }

    }
}
