using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyShop.Core.Models;
using MyShop.DataAccess.InMemory;

namespace MyShop.WebUI.Controllers
{
    public class ProductCategoryController : Controller
    {
        InMemoryRepository <ProductCategory> context;

        public ProductCategoryController()
        {
            context = new InMemoryRepository <ProductCategory>();
        }
        // GET: productManager
        public ActionResult Index()
        {
            List<ProductCategory> categories = context.Collection().ToList();
            return View(categories);
        }

        public ActionResult Create()
        {
            ProductCategory category = new ProductCategory();
            return View(category);
        }

        [HttpPost]
        public ActionResult Create(ProductCategory category)
        {
            if (!ModelState.IsValid)
            {
                return View(category);
            }
            else
            {
                context.Insert(category);
                context.Commit();

                return RedirectToAction("Index");
            }
        }

        public ActionResult Edit(string Id)
        {
            ProductCategory category = context.Find(Id);
            if (category == null)
            {
                return HttpNotFound();
            }
            else
            {
                return View(category);
            }
        }

        [HttpPost]
        public ActionResult Edit(ProductCategory category, string Id)
        {
            ProductCategory CategoryToEdit = context.Find(Id);
            if (CategoryToEdit == null)
            {
                return HttpNotFound();
            }
            else
            {
                if (!ModelState.IsValid)
                {
                    return View(category);
                }

                CategoryToEdit.CategoryName = category.CategoryName;


                context.Commit();

                return RedirectToAction("Index");
            }
        }

        public ActionResult Delete(string Id)
        {
            ProductCategory CategoryToDelete = context.Find(Id);

            if (CategoryToDelete == null)
            {
                return HttpNotFound();
            }
            else
            {

                return View(CategoryToDelete);
            }
        }

        [HttpPost]
        [ActionName("Delete")]
        public ActionResult ConfirmDelete(string Id)
        {
            ProductCategory CategoryToDelete = context.Find(Id);

            if (CategoryToDelete == null)
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
