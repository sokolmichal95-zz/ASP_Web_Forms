using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ASP.NetWebForms.Models;

namespace ASP.NetWebForms.Logic
{
    public class ShoppingCartActions : IDisposable
    {
        public string ShoppingCartId { get; set; }
        private ProductContext _db = new ProductContext();
        public const string CartSessionKey = "CartId";
        public void AddToCart(int id)
        {
            ShoppingCartId = GetCartId();
            var cartItem = _db.CartItems.SingleOrDefault(
                c => c.CartId == ShoppingCartId
                && c.ProductId == id);
            if (cartItem == null)
            {
                cartItem = new CartItem
                {
                    ItemId = Guid.NewGuid().ToString(),
                    ProductId = id,
                    CartId = ShoppingCartId,
                    Product = _db.Products.SingleOrDefault(
                        p => p.ProductID == id),
                    Quantity = 1,
                    DateCreated = DateTime.Now
                };

                _db.CartItems.Add(cartItem);
            }
            else
            {
                cartItem.Quantity++;
            }
            _db.SaveChanges();
        }

        public void Dispose()
        {
            if (_db != null)
            {
                _db.Dispose();
                _db = null;
            }
        }

        public string GetCartId()
        {
            if (HttpContext.Current.Session[CartSessionKey] == null)
            {
                if (!string.IsNullOrWhiteSpace(HttpContext.Current.User.Identity.Name))
                {
                    HttpContext.Current.Session[CartSessionKey] = HttpContext.Current.User.Identity.Name;
                }
                else
                {
                    Guid tempCartId = Guid.NewGuid();
                    HttpContext.Current.Session[CartSessionKey] = tempCartId.ToString();
                }
            }
            return HttpContext.Current.Session[CartSessionKey].ToString();
        }

        public List<CartItem> GetCartItems()
        {
            ShoppingCartId = GetCartId();

            return _db.CartItems.Where(c => c.CartId == ShoppingCartId).ToList();
        }

        public decimal GetTotal()
        {
            ShoppingCartId = GetCartId();
            decimal? total = decimal.Zero;
            total = (decimal?)(from cartItems in _db.CartItems
                               where cartItems.CartId == ShoppingCartId
                               select (int?)cartItems.Quantity * cartItems.Product.UnitPrice).Sum();
            return total ?? decimal.Zero;
        }

        public ShoppingCartActions GetCart(HttpContext context)
        {
            using (var cart = new ShoppingCartActions())
            {
                cart.ShoppingCartId = GetCartId();
                return cart;
            }
        }

        public void UpdateShoppingCartDatabase(String cartId, ShoppingCartUpdates[] CartItemUpdates)
        {
            using(var db = new ASP.NetWebForms.Models.ProductContext())
            {
                try
                {
                    int CartItemCount = CartItemUpdates.Count();
                    List<CartItem> ciList = GetCartItems();
                    foreach (var item in ciList)
                    {
                        for(int i = 0; i < CartItemCount; i++)
                        {
                            if (item.Product.ProductID == CartItemUpdates[i].ProductId)
                            {
                                if(CartItemUpdates[i].Quantity < 1 || CartItemUpdates[i].RemoveItem == true)
                                {
                                    RemoveItem(cartId, item.ProductId);
                                }
                                else
                                {
                                    UpdateItem(cartId, item.ProductId, CartItemUpdates[i].Quantity);
                                }
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    throw new Exception("ERROR CartDB was fucked!" + e.Message.ToString(), e);
                }
            }
        }

        public void RemoveItem(String id, int productId)
        {
            using (var _db = new ProductContext())
            {
                try
                {
                    var item = (from c in _db.CartItems where c.CartId == id && c.Product.ProductID == productId select c).FirstOrDefault();
                    if (item != null)
                    {
                        _db.CartItems.Remove(item);
                        _db.SaveChanges();
                    }
                }
                catch (Exception e)
                {
                    throw new Exception("Unable to remove: " + e.Message.ToString(), e);
                }
            }
        }

        public void UpdateItem(String id, int productId, int quantity)
        {
            using(var _db = new ProductContext())
            {
                try
                {
                    var item = (from c in _db.CartItems where c.CartId == id && c.Product.ProductID == productId select c).FirstOrDefault();
                    if (item != null)
                    {
                        item.Quantity = quantity;
                        _db.SaveChanges();
                    }
                }
                catch (Exception e)
                { 
                    throw new Exception("ERROR Updating: " + e.Message.ToString(), e);
                }
            }
        }

        public void EmptyCart()
        {
            ShoppingCartId = GetCartId();
            var items = _db.CartItems.Where(c => c.CartId == ShoppingCartId);
            foreach (var item in items)
            {
                _db.CartItems.Remove(item);
            }
            _db.SaveChanges();
        }

        public int GetCount()
        {
            ShoppingCartId = GetCartId();
            int? count = (from ci in _db.CartItems
                          where ci.CartId == ShoppingCartId
                          select (int?)ci.Quantity).Sum();
            return count ?? 0;
        }

        public struct ShoppingCartUpdates
        {
            public int ProductId;
            public int Quantity;
            public bool RemoveItem;
        }
    }
}