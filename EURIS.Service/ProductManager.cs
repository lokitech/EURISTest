using System.Collections.Generic;
using System.Linq;
using EURIS.Entities;

namespace EURIS.Service
{
    public class ProductManager
    {
        readonly LocalDbEntities _context = new LocalDbEntities();

        public List<Product> GetProducts()
        {
            var products = (from item in _context.Product select item).ToList();
            return products;
        }
    }
}
