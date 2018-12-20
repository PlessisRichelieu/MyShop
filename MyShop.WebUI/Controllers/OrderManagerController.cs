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
        IOrderService OrderService;

        public OrderManagerController (IOrderService OrderService)
        {
            this.OrderService = OrderService;
        }
        // GET: OrderManager
        public ActionResult Index()
        {
            List<Order> Orders = OrderService.GetOrderLst();
            return View();
        }

        public ActionResult UpdateOrder (String Id)
        {
            ViewBag.StatusList = new List<string>()
            {
                "Order Created",
                "Payment Processed",
                "Order shipped",
                "Order complete"
            };
            Order Order = OrderService.GetOrder(Id);
            return View(Order);

        }

        [HttpPost]

        public ActionResult UpdateOrder (Order UpdatedOrder, String Id)
        {
            Order Order = OrderService.GetOrder(Id);
            OrderService.UpdateOrder(Order);
            return RedirectToAction("Index");
        }
    }
}