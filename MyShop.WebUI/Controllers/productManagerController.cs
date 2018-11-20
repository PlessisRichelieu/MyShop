using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyShop.Core.Models;
using MyShop.DataAccess.InMemory;
using MyShop.Core.ViewModels;

namespace MyShop.WebUI.Controllers
{
    public class productManagerController : Controller
    {
        InMemoryRepository <Product> context;
        InMemoryRepository <ProductCategory> ProductCategories;

        public productManagerController()
        {
            context = new InMemoryRepository<Product>();
            ProductCategories = new InMemoryRepository <ProductCategory>();
        }
        // GET: productManager
        public ActionResult Index()
        {
            List<Product> products = context.Collection().ToList();
            return View(products);
        }

        public ActionResult Create ()
        {
            ProductManagerViewModel ViewModel = new ProductManagerViewModel();
            ViewModel.Product = new Product();
            ViewModel.ProductCategories = ProductCategories.Collection();
            return View(ViewModel);
        }

        [HttpPost]
        public ActionResult Create (Product product)
        {
            if (!ModelState.IsValid)
            {
                return View(product);
            }
            else
            {
                context.Insert(product);
                context.Commit();

                return RedirectToAction("Index");
            }
        }

        public ActionResult Edit(string Id)
        {
            Product product = context.Find(Id);
            if (product == null)
            {
                return HttpNotFound();
            }
            else
            {
                ProductManagerViewModel ViewModel = new ProductManagerViewModel();
                ViewModel.Product = product;
                ViewModel.ProductCategories = ProductCategories.Collection();
                return View(ViewModel);
            }
        }

        [HttpPost]
        public ActionResult Edit(Product product, string Id)
        {
            Product ProductToEdit = context.Find(Id);
            if (ProductToEdit == null)
            {
                return HttpNotFound();
            }
            else
            {
                if (!ModelState.IsValid)
                {
                    return View(product);
                }

                ProductToEdit.category = product.category;
                ProductToEdit.Decription = product.Decription;
                ProductToEdit.Name = product.Name;
                ProductToEdit.Image = product.Image;
                ProductToEdit.price = product.price;

                context.Commit();

                return RedirectToAction("Index");
            }
        }

        public ActionResult Delete(string Id)
        {
            Product ProductToDelete = context.Find(Id);

            if (ProductToDelete == null)
            {
                return HttpNotFound();
            }
            else
            {

                return View(ProductToDelete);
            }
        }

        [HttpPost]
        [ActionName("Delete")]
        public ActionResult ConfirmDelete(string Id)
        {
            Product ProductToDelete = context.Find(Id);

            if (ProductToDelete == null)
            {
                return HttpNotFound();
            }
            else
            {
                context.Delete(Id);
                context.Commit();
                return RedirectToAction("Index");
            }

        }
    }

}
