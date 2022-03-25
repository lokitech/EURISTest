using EURIS.Model.ViewModels;
using EURIS.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;

namespace EurisTest.Web.Controllers
{
    public class CatalogApiController : ApiController
    {
        ICatalogManager _catalogManager;
        public CatalogApiController(ICatalogManager catalogManager)
        {
            _catalogManager = catalogManager;
        }

        /// <summary>
        /// Gets all the products that belong to a certain catalog.
        /// </summary>
        /// <param name="id">Catalog ID.</param>
        /// <returns>JSON list of products.</returns>
        [Route("api/GetProductsFromCatalog/{id}")]
        [HttpGet]
        public JsonResult<IEnumerable<ProductViewModel>> GetProductsFromCatalog(int id)
        {
            return Json(_catalogManager.GetProductsFromCatalog(id));
        }

    }
}
