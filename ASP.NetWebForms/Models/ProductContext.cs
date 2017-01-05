using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace ASP.NetWebForms.Models
{
    public class ProductContext : DbContext
    {
        public ProductContext() : base("ASP.NetWebForms")
        {
        }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
    }
}