using EURIS.Entities;
using EURIS.Model.Models;
using EURIS.Model.ViewModels;
using System.Collections.Generic;
using System.Web.Mvc;

namespace EURIS.Service.Interfaces
{
    public interface ICatalogManager
    {
        ListViewModel<CatalogViewModel> GetCatalogs(int page, int pageSize);
        IEnumerable<SelectListItem> GetSelectListCatalogs();
        IEnumerable<ProductViewModel> GetProductsFromCatalog(int catalogId);
        Catalog GetCatalog(int catalogId);
        Catalog GetCatalog(string catalogCode);
        int InsertCatalog(CatalogModel catalog);
        int UpdateCatalog(CatalogModel catalog);
        void DeleteCatalog(int catalogId);
    }
}
