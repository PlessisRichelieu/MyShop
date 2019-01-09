using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyShop.Core.Models;
using MyShop.Core.Contracts;
using MyShop.Core.ViewModels;

namespace MyShop.Services
{
    public class OrderService : IOrderService
    {
        IRepository<Order> orderContext;
        public OrderService (IRepository <Order> OrderContext)
        {
            this.orderContext = OrderContext;
        }
        public void CreateOrder(Order BaseOrder, List<BasketItemViewModel> items)
        {
            foreach (var item in items)
            {
                BaseOrder.OrderItems.Add(new OrderItem()
                {
                    OrderId = item.Id,
                    ProductName = item.ProductName,
                    Price = item.Price,
                    Quantity = item.Quanity,
                    Image = item.Image
                });
                
            }

            orderContext.Insert(BaseOrder);
            orderContext.Commit();
        }
    }
}
