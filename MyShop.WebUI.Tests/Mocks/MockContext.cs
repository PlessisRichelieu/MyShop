using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyShop.Core.Contracts;
using MyShop.Core.Models;

namespace MyShop.WebUI.Tests.Mocks
{
   public class MockContext <T> : IRepository <T> where T : BaseEntity
    {
        List<T> items;
        string ClassName;

        public MockContext  ()
        {
           
            items = new List<T>();
           
        }

        public void Commit()
        {
            return;
        }

        public void Insert(T t)
        {
            items.Add(t);
        }

        public void Update(T t)
        {
            T TtoUpdate = items.Find(i => i.Id == t.Id);

            if (TtoUpdate != null)
            {
                TtoUpdate = t;
            }
            else
            {
                throw new Exception(ClassName + "Not Found");
            }
        }

        public T Find(string Id)
        {
            T t = items.Find(i => i.Id == Id);

            if (t != null)
            {
                return t;
            }
            else
            {
                throw new Exception(ClassName + " not found");
            }

        }

        public IQueryable<T> Collection()
        {
            return items.AsQueryable();
        }

        public void Delete(String Id)
        {

            T TtoDelete = items.Find(i => i.Id == Id);

            if (TtoDelete != null)
            {
                items.Remove(TtoDelete);
            }
            else
            {
                throw new Exception(ClassName + "Not Found");
            }

        }


    }
}
