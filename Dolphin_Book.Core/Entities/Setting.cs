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
    public class Setting:BaseEntity
    {
        public string? Image { get; set; }
        [NotMapped]
        public IFormFile? FormFile { get; set; }
        public double Area1 { get; set; }
        public double Area2 { get; set; }
        public double Area3 { get; set; }
        public double Area4 { get; set; }
        public double Area5 { get; set; }
        public double Area6 { get; set; }
    }
}
