using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ASP.NetWebForms.Models;
using System.Web.ModelBinding;

namespace ASP.NetWebForms
{
    public partial class ProductDetails : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        
        public IQueryable<Product> GetProduct ([QueryString("productID")] int? productID)
        {
            var _db = new ASP.NetWebForms.Models.ProductContext();
            IQueryable<Product> query = _db.Products;
            if(productID.HasValue && productID > 0)
            {
                query = query.Where(p => p.ProductID == productID);
            }
            else
            {
                query = null;
            }
            return query;
        }

    }
}