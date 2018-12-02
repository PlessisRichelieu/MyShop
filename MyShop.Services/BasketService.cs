using MyShop.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using MyShop.Core.Contracts;

namespace MyShop.Services
{
    public class BasketService
    {
        IRepository <Product> ProductContext;

        IRepository <Basket> BasketContext;

        public const string BasketSessionName = "eCommerceBasket";

        public BasketService (IRepository<Product> PContext, IRepository <Basket> BContext)
        {
            this.BasketContext = BContext;
            this.ProductContext = PContext;
        }

        private Basket GetBasket (HttpContextBase httpContext, bool CreateIfNull)
        {
            HttpCookie cookie = httpContext.Request.Cookies.Get(BasketSessionName);

            Basket basket = new Basket();

            if (cookie != null )
            {
                string BasketId = cookie.Value;
                if (!string.IsNullOrEmpty(BasketId))
                {
                    basket = BasketContext.Find(BasketId);
                }
                else if (CreateIfNull)
                {
                    basket = CreateNewBasket(httpContext);
                }
            }
            else (CreateIfNull)
            {
                basket = CreateNewBasket(httpContext);
            }

            return basket;

        }

        private Basket CreateNewBasket (HttpContextBase httpContext)
        {
            Basket basket = new Basket();
            BasketContext.Insert(basket);
            BasketContext.Commit();

            HttpCookie cookie = new HttpCookie(BasketSessionName);

            cookie.Value = basket.Id;

            cookie.Expires = DateTime.Now.AddDays(1);

            httpContext.Response.Cookies.Add(cookie);

            return basket;

        }

        public void AddToBasket (HttpContextBase httpContext, string ProductId )
        {
            Basket basket = GetBasket(httpContext, true);
            BasketItem item = basket.BasketItems.FirstOrDefault(i => i.ProductId == ProductId);

            if (item == null)
            {
                item = new BasketItem()
                {
                    BasketId = basket.Id,
                    ProductId = ProductId,
                    Quantity = 1
                };

                basket.BasketItems.Add(item);
            }
            else
            {
                item.Quantity = item.Quantity + 1;
            }

            BasketContext.Commit();
            
        }

        public void RemoveFromBasket (HttpContextBase httpContext, string itemId)
        {
            Basket basket = GetBasket(httpContext, true);
            BasketItem item = basket.BasketItems.FirstOrDefault(i => i.Id == itemId);

            if (item != null)
            {

                basket.BasketItems.Remove(item);
                BasketContext.Commit();

                if (item.Quantity !=0)
                {
                    item.Quantity = item.Quantity - 1;
                }
            }

            

        }
    }
}
