using MyShop.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using MyShop.Core.Contracts;
using MyShop.Core.ViewModels;

namespace MyShop.Services
{
    public class BasketService : IBasketService
    {
        IRepository<Product> ProductContext;

        IRepository<Basket> BasketContext;

        public const string BasketSessionName = "eCommerceBasket";

        public BasketService(IRepository<Product> PContext, IRepository<Basket> BContext)
        {
            this.BasketContext = BContext;
            this.ProductContext = PContext;
        }

        private Basket GetBasket(HttpContextBase httpContext, bool CreateIfNull)
        {
            HttpCookie cookie = httpContext.Request.Cookies.Get(BasketSessionName);

            Basket basket = new Basket();

            if (cookie != null)
            {
                string BasketId = cookie.Value;
                if (!string.IsNullOrEmpty(BasketId))
                {
                    basket = BasketContext.Find(BasketId);
                }
                else
                {
                    if (CreateIfNull)
                    {
                        basket = CreateNewBasket(httpContext);
                    }
                }
            }
            else
            {
                if (CreateIfNull)
                {
                    basket = CreateNewBasket(httpContext);
                }
            }

            return basket;

        }

        private Basket CreateNewBasket(HttpContextBase httpContext)
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

        public void AddToBasket(HttpContextBase httpContext, string ProductId)
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

        public void RemoveFromBasket(HttpContextBase httpContext, string itemId)
        {
            Basket basket = GetBasket(httpContext, true);
            BasketItem item = basket.BasketItems.FirstOrDefault(i => i.Id == itemId);

            if (item != null)
            {

                basket.BasketItems.Remove(item);
                BasketContext.Commit();

                if (item.Quantity != 0)
                {
                    item.Quantity = item.Quantity - 1;
                }
            }



        }

        public List<BasketItemViewModel> GetBasketItems(HttpContextBase httpContext)
        {

            Basket basket = GetBasket(httpContext, false);

            if (basket != null)
            {
                var results = (from b in basket.BasketItems
                               join p in ProductContext.Collection() on b.ProductId equals p.Id
                               select new BasketItemViewModel()
                               {
                                   Id = b.Id,
                                   Quantity = b.Quantity,
                                   ProductName = p.Name,
                                   Price = p.price,
                                   ImageUrl = p.Image




                               }
                              ).ToList();

                return results;

            }
            else
            {
                return new List<BasketItemViewModel>();
            }

        }

        public BasketSummaryViewModel GetBasketSummary(HttpContextBase httpContext)
        {
            Basket basket = GetBasket(httpContext, false);

            BasketSummaryViewModel model = new BasketSummaryViewModel(0, 0);

            if (basket != null)
            {
                int? BasketCount = (from item in basket.BasketItems
                                    select item.Quantity
                                    ).Sum();

                decimal? BasketTotal = (from item in basket.BasketItems
                                        join p in ProductContext.Collection() on item.ProductId equals p.Id
                                        select item.Quantity * p.price).Sum();
                model.BasketCount = BasketCount ?? 0;
                model.BasketTotal = BasketTotal ?? Decimal.Zero;

                return model;
            }
            else
            {
                return model;
            }
        }

        public void ClearBasket (HttpContextBase httpContext)
        {
            Basket basket = GetBasket(httpContext, false);
            basket.BasketItems.Clear();
            BasketContext.Commit();
        }

    }
}
