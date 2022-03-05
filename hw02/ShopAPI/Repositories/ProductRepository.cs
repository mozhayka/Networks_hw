using ShopAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopAPI.Repositories
{
    public class ProductRepository : IProductRepository
    {
        List<Product> products;
        int lastId;

        public ProductRepository()
        {
            products = new List<Product>();
            lastId = 0;
            products.Add(new Product { Name = "First", Description = "Des"});
            Console.WriteLine("Start");
        }

        public List<Product> GetAll()
        {
            return products;
        }

        public Product Get(int id)
        {
            return products.Find(x => x.Id == id);
        }

        public void Add(Product p)
        {
            lastId++;
            p.Id = lastId;
            products.Add(p);
            products.ForEach(Console.WriteLine);
        }

        public void Delete(int id)
        {
            
            //products = (List<Product>)products.Where(x => x.Id != id);
            products.RemoveAll(x => x.Id == id);
        }

        public void Update(int id, Product p)
        {
            //products = (List<Product>)products.Select(x => (x.Id != p.Id) ? x : p);
            var prod = products.FirstOrDefault(x => (x.Id == id));
            if (prod != null)
            {
                prod.Id = id;
                prod.Name = p.Name;
                prod.Description = p.Description;
            }
        }
    }
}
