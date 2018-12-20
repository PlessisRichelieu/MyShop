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

        IRepository<Customer> Customers;
        IOrderService OrderService;
        IBasketService BasketService;

        public BasketController (IBasketService BasketService, IOrderService OrderService, IRepository <Customer> Customers)
        {
            this.BasketService = BasketService;
            this.OrderService = OrderService;
            this.Customers = Customers;
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

        [Authorize]
        public ActionResult Checkout()
        {
            Customer customer = Customers.Collection().FirstOrDefault(c => c.EMail == User.Identity.Name);

            if (customer != null)
            {
                Order order = new Order()
                {
                    EMail = customer.EMail,
                    City = customer.City,
                    Province = customer.Province,
                    Street = customer.Street,
                    FirstName = customer.FirstName,
                    LastName = customer.LastName,
                    PostCode = customer.PostCode
                };

                return View(order);
            }
            else
            {
                return RedirectToAction("Error");
            }
            
        }

        [HttpPost]
        [Authorize]
        public ActionResult Checkout (Order Order)
        {
            var BasketItems = BasketService.GetBasketItems(this.HttpContext);

            Order.OrderStatus = "Order Created";

            Order.EMail = User.Identity.Name;

            Order.OrderStatus = "Payment Processed";

            OrderService.CreateOrder(Order, BasketItems);
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