using ColorLibBack.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ColorLibBack.ViewModels
{
    public class HomeVM
    {
        public List<Product> Products { get; set; }
        public List<Category > Categories { get; set; }
    }
}
