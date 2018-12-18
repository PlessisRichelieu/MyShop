using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyShop.Core.Contracts;
using MyShop.Core.Models;

namespace MyShop.WebUI.Controllers
{
    public class BasketController : Controller
    {
       
        IOrderService orderService;
        IBasketService BasketService;

        public BasketController (IBasketService BasketService, IOrderService OrderService)
        {
            this.BasketService = BasketService;
            this.orderService = OrderService;
        }
        public ActionResult Index()
        {
            var model = BasketService.GetBasketItems(this.HttpContext);
            return View(model);
        }

        public ActionResult AddToBasket(string Id)
        {
            BasketService.AddToBasket(this.HttpContext, Id);

            return RedirectToAction("Index");
        }

        public ActionResult RemoveFromBasket (string Id)
        {
            BasketService.RemoveFromBasket(this.HttpContext, Id);

            return RedirectToAction("Index");
        }

        public PartialViewResult BasketSummary ()
        {
            var BasketSummary = BasketService.GetBasketSummary(this.HttpContext);

            return PartialView(BasketSummary);
        }

        public ActionResult Checkout ()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Checkout (Order Order)
        {
            var BasketItems = BasketService.GetBasketItems(this.HttpContext);

            Order.OrderStatus = "Order Created";

            Order.OrderStatus = "Payment Processed";

            orderService.CreateOrder(Order, BasketItems);
            BasketService.ClearBasket(this.HttpContext);

            return RedirectToAction("ThankYou", new { OrderId = Order.Id });
        }

        public ActionResult ThankYou (string OrderId)
        {
            ViewBag.OrderId = OrderId;
            return View();
        }
    }
}