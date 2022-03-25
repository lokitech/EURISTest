using AutoMapper;
using EURIS.Model.Exceptions;
using EURIS.Model.Models;
using EURIS.Model.ViewModels;
using EURIS.Service.Interfaces;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EurisTest.Web.Controllers
{
    public class CatalogController : Controller
    {
        private IProductManager _productManager;
        private ICatalogManager _catalogManager;
        private IMapper _mapper;
        private static readonly ILog _logger = LogManager.GetLogger(typeof(CatalogController));

        public CatalogController(IProductManager productManager, ICatalogManager catalogManager, IMapper mapper)
        {
            _productManager = productManager;
            _catalogManager = catalogManager;
            _mapper = mapper;
        }

        public ActionResult Index(int page = 1)
        {
            try
            {
                var catalogs = _catalogManager.GetCatalogs(page, 10);
                return View(catalogs);
            }
            catch (Exception ex)
            {
                _logger.Error("GET: Index()", ex);
                return View("Error");
            }
        }

        public ActionResult Details(int id)
        {
            try
            {
                var catalog = _catalogManager.GetCatalog(id);
                var catalogViewModel = _mapper.Map<CatalogViewModel>(catalog);
                return View(catalogViewModel);
            }
            catch (Exception ex)
            {
                _logger.Error($"GET: Details(id: {id})", ex);
                return View("Error");
            }
        }

        public ActionResult DetailsPartial(CatalogViewModel catalog)
        {
            return PartialView(catalog);
        }

        public ActionResult Create()
        {
            try
            {
                ViewBag.ProductList = _productManager.GetSelectListProducts();
                return View();
            }
            catch (Exception ex)
            {
                _logger.Error("GET: Create()", ex);
                return View("Error");
            }
        }

        [HttpPost]
        public ActionResult Create(CatalogModel catalog)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _catalogManager.InsertCatalog(catalog);
                    return RedirectToAction("Index");
                }
                else
                {
                    return View();
                }
            }
            catch (UserFriendlyException ex)
            {
                ViewBag.ProductList = _productManager.GetSelectListProducts();
                ViewBag.Error = ex.Message;
                return View();
            }
            catch (Exception ex)
            {
                ViewBag.ProductList = _productManager.GetSelectListProducts();
                _logger.Error($"POST: Create({catalog.ToString()})", ex);
                return View("Error");
            }
        }

        public ActionResult Edit(int id)
        {
            try
            {
                var catalog = _catalogManager.GetCatalog(id);
                var catalogModel = _mapper.Map<CatalogModel>(catalog);
                ViewBag.ProductList = _productManager.GetSelectListProducts();
                return View(catalogModel);
            }
            catch (Exception ex)
            {
                _logger.Error($"GET: Edit({id})", ex);
                return View("Error");
            }
        }

        [HttpPost]
        public ActionResult Edit(CatalogModel catalog)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _catalogManager.UpdateCatalog(catalog);
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.ProductList = _productManager.GetSelectListProducts();
                    return View();
                }
            }
            catch (UserFriendlyException ex)
            {
                ViewBag.ProductList = _productManager.GetSelectListProducts();
                ViewBag.Error = ex.Message;
                return View();
            }
            catch (Exception ex)
            {
                ViewBag.ProductList = _productManager.GetSelectListProducts();
                _logger.Error($"POST: Edit({catalog.ToString()})", ex);
                return View("Error");
            }
        }

        public ActionResult Delete(int id)
        {
            try
            {
                var catalog = _catalogManager.GetCatalog(id);
                var catalogModel = _mapper.Map<CatalogViewModel>(catalog);
                return PartialView(catalogModel);
            }
            catch (Exception ex)
            {
                _logger.Error($"GET: Delete({id})", ex);
                return View("Error");
            }
        }

        [HttpPost]
        public ActionResult Delete(CatalogViewModel catalog)
        {
            try
            {
                _catalogManager.DeleteCatalog(catalog.Id);
                int page;
                int.TryParse(HttpUtility.ParseQueryString(Request.UrlReferrer.Query)["page"], out page);
                return RedirectToAction("Index", new { page = page == 0 ? 1 : page });
            }

            catch (UserFriendlyException ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.Error($"POST: Delete({catalog.ToString()})", ex);
                return View("Error");
            }
        }
    }
}