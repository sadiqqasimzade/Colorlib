using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ColorLibBack.Models
{
    public class Product
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Desc { get; set; }
        [Required,Range(0,double.MaxValue)]
        public double Price { get; set; }
        public bool IsDeleted { get; set; }
        [Required]
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public string Img { get; set; }
        [NotMapped]
        public IFormFile File { get; set; }


    }
}
