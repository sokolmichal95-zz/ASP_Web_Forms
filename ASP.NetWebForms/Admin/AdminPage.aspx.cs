using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ASP.NetWebForms.Models;
using ASP.NetWebForms.Logic;

namespace ASP.NetWebForms.Admin
{
    public partial class AdminPage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string productAction = Request.QueryString["ProductAction"];
            if(productAction == "add")
            {
                LabelAddStatus.Text = "Product added!";
            }

            if(productAction == "remove")
            {
                LabelRemoveProduct.Text = "Product removed";
            }
        }

        protected void AddProductButton_Click(object sender, EventArgs e)
        {
            Boolean fileOK = false;
            String path = Server.MapPath("~/Catalog/Images");
            if (ProductImage.HasFile)
            {
                String fileExtension = System.IO.Path.GetExtension(ProductImage.FileName).ToLower();
                String[] allowedExceptions = { ".gif", ".jpg", ".jpeg", ".png" };
                for(int i = 0; i < allowedExceptions.Length; i++)
                {
                    if(fileExtension == allowedExceptions[i])
                    {
                        fileOK = true;
                    }
                }
            }
            if (fileOK)
            {
                try
                {
                    ProductImage.PostedFile.SaveAs(path + ProductImage.FileName);
                    ProductImage.PostedFile.SaveAs(path + "Thumbs/" + ProductImage.FileName);
                }
                catch (Exception ex)
                {
                    LabelAddStatus.Text = ex.Message;
                }

                AddProducts products = new AddProducts();
                bool addSuccess = products.AddProduct(AddProductName.Text, AddProductDescription.Text,
                    AddProductPrice.Text, DropDownAddCategory.SelectedValue, ProductImage.FileName);
                if (addSuccess)
                {
                    string pageUrl = Request.Url.AbsoluteUri.Substring(0, Request.Url.AbsoluteUri.Count() - Request.Url.Query.Count());
                    Response.Redirect(pageUrl + "?ProductAction=add");
                }
                else
                {
                    LabelAddStatus.Text = "Unable to add new product to database.";
                }
            }
            else
            {
                LabelAddStatus.Text = "Unable to accept file type.";
            }
        }
        

        public IQueryable GetCategories()
        {
            var _db = new ProductContext();
            IQueryable query = _db.Categories;
            return query;
        }

        public IQueryable GetProducts()
        {
            var _db = new ProductContext();
            IQueryable query = _db.Products;
            return query;
        }

        protected void RemoveProductButton_Click(object sender, EventArgs e)
        {
            using (var _db = new ProductContext())
            {
                int productId = Convert.ToInt16(DropDownRemoveProduct.SelectedValue);
                var item = (from c in _db.Products where c.ProductID == productId select c).FirstOrDefault();
                if(item != null)
                {
                    _db.Products.Remove(item);
                    _db.SaveChanges();

                    string pageUrl = Request.Url.AbsoluteUri.Substring(0, Request.Url.AbsoluteUri.Count() - Request.Url.Query.Count());
                    Response.Redirect(pageUrl + "?ProductAction=remove");

                }
                else
                {
                    LabelRemoveStatus.Text = "Unable to locate product";
                }
            }
        }
    }
}