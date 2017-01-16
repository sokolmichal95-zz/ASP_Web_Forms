using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace ASP.NetWebForms.Models
{
    public class Category
    {
        [ScaffoldColumn(false)]
        public int CategoryID { get; set; }

        [Required, StringLength(100), Display(Name = "Name")]
        public string CategoryName { get; set; }

        [StringLength(10000), Display(Name = "Category Description")]
        public string CategoryDescription { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}