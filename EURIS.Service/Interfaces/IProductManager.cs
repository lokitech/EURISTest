using EURIS.Entities;
using EURIS.Model.Models;
using EURIS.Model.ViewModels;
using System.Collections.Generic;
using System.Web.Mvc;

namespace EURIS.Service.Interfaces
{
    public interface IProductManager
    {
        ListViewModel<ProductViewModel> GetProducts(int page, int pageSize);
        IEnumerable<SelectListItem> GetSelectListProducts();
        Product GetProduct(int prodId);
        Product GetProduct(string prodCode);
        int InsertProduct(ProductModel product);
        int UpdateProduct(ProductModel product);
        void DeleteProduct(int productId);
    }
}
