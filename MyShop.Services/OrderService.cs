using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyShop.Core.Contracts;
using MyShop.Core.Models;
using MyShop.Core.ViewModels;

namespace MyShop.Services
{
    public class OrderService : IOrderService
    {
        IRepository<Order> OrderContext;
        public OrderService (IRepository <Order> orders)
        {

            this.OrderContext = orders;

        }

        public void CreateOrder(Order BaseOrder, List<BasketItemViewModel> basketItems)
        {
            foreach (var item in basketItems)
            {
                BaseOrder.OrderItems.Add(new OrderItem()
                {
                    ProductId = item.Id,
                    Image = item.ImageUrl,
                    Price = item.Price,
                    ProductName = item.ProductName,
                    Quantity = item.Quantity

                });
            }

            OrderContext.Insert(BaseOrder);
            OrderContext.Commit();
        }
    }
}
