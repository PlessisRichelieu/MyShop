using MyShop.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.DataAccess.InMemory
{
    public class CategoryRepository
    {
        ObjectCache cashe = MemoryCache.Default;
        List<ProductCategory> ProductCategories = new List<ProductCategory>();

        public CategoryRepository()
        {
            ProductCategories = cashe["ProductCategories"] as List<ProductCategory>;

            if (ProductCategories == null)
            {
                ProductCategories = new List<ProductCategory>();
            }
        }

        public void Commit()
        {
            cashe["ProductCategories"] = ProductCategories;

        }

        public void Insert(ProductCategory PC)
        {
            ProductCategories.Add(PC);
        }

        public void Update(ProductCategory Category)
        {
            ProductCategory CategoryToUpdate = ProductCategories.Find(PC => PC.Id == PC.Id);

            if (CategoryToUpdate != null)
            {
                CategoryToUpdate = Category;
            }
            else
            {
                throw new Exception("Category not found");
            }
        }

        public ProductCategory Find(string Id)
        {
            ProductCategory Category = ProductCategories.Find(pc => pc.Id == Id);

            if (Category != null)
            {
                return Category;
            }
            else
            {
                throw new Exception("Category not found");
            }

        }

        public IQueryable<ProductCategory> Collection()
        {
            return ProductCategories.AsQueryable();
        }

        public void Delete(string Id)
        {
            ProductCategory CategoryToDelete = ProductCategories.Find(pc => pc.Id == Id);

            if (CategoryToDelete != null)
            {
                ProductCategories.Remove(CategoryToDelete);
            }

            else
            {
                throw new Exception("Category not found");
            }
        }
    }
}

