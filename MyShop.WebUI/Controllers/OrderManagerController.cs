using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyShop.Core.Contracts;
using MyShop.Core.Models;

namespace MyShop.WebUI.Controllers
{
    public class OrderManagerController : Controller
    {

        IOrderService orderService;

        public OrderManagerController (IOrderService OrderService)
        {
            this.orderService = OrderService;
        }
        public ActionResult Index()
        {
            List<Order> orders = orderService.GetOrderList();
            return View(orders);
        }

        public ActionResult UpdateOrder (string Id)
        {
            ViewBag.StatusList = new List<string>()
            {
                "Order Created",
                "Payment processed",
                "Order shipped",
                "Order Complete"
            };

            Order order = orderService.GetOrder(Id);
            return View(order);
        }

        [HttpPost]
        public ActionResult UpdateOrder (Order UpdatedOrder, string Id)
        {
            Order order = orderService.GetOrder(Id);

            order.OrderStatus = UpdatedOrder.OrderStatus;

            orderService.UpdateOrder(order);

            return RedirectToAction("Index");
        }
    }
}