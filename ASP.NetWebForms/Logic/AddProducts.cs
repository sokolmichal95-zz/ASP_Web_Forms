using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ASP.NetWebForms.Models;

namespace ASP.NetWebForms.Logic
{
    public class AddProducts
    {
        public bool AddProduct(string Name, string Description, string Price, string Category, string ImagePath)
        {
            var product = new Product();
            product.ProductName = Name;
            product.Description = Description;
            product.UnitPrice = Convert.ToDouble(Price);
            product.CategoryID = Convert.ToInt32(Category);
            product.ImagePath = ImagePath;

            using (ProductContext _db = new ProductContext())
            {
                _db.Products.Add(product);
                _db.SaveChanges();
            }

            return true;
        }
    }
}