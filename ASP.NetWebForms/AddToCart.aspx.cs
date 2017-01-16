using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Diagnostics;
using ASP.NetWebForms.Logic;

namespace ASP.NetWebForms
{
    public partial class AddToCart : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string rawId = Request.QueryString["ProductId"];
            int productId;
            if(!String.IsNullOrEmpty(rawId) && int.TryParse(rawId, out productId))
            {
                using(ShoppingCartActions usersShoppingCart = new ShoppingCartActions())
                {
                    usersShoppingCart.AddToCart(Convert.ToInt16(rawId));
                }
            }
            else
            {
                Debug.Fail("ERROR: Cannot get to AddToCart.aspx. No ProductId.");
                throw new Exception("ERROR: Illegal to load AddToCart.aspx without setting ProductId.");
            }
            Response.Redirect("ShoppingCart.aspx");
        }

    }
}