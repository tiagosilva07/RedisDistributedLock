using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DistributedLock
{
    internal class ProductsDatabase
    {
        public List<Product> Products = new List<Product>();


        public void SaveProduct(Product product)
        {
            Products.Add(product);
        }

    }
}
