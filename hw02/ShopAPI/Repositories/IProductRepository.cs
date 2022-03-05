using ShopAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopAPI.Repositories
{
    public interface IProductRepository
    {
        public List<Product> GetAll();

        public Product Get(int id);

        public void Add(Product p);

        public void Delete(int id);

        public void Update(int id, Product p);
    }
}
