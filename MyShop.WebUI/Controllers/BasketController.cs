using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyShop.Core.Contracts;

namespace MyShop.WebUI.Controllers
{
    public class BasketController : Controller
    {
        // GET: Basket
        IBasketService BasketService;

        public BasketController (IBasketService basketService)
        {
            this.BasketService = basketService;
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
    }
}