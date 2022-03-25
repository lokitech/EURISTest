using System;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using EURIS.Model.Exceptions;
using EURIS.Model.Models;
using EURIS.Model.ViewModels;
using EURIS.Service.Interfaces;
using log4net;

namespace EurisTest.Web.Controllers
{
    public class ProductController : Controller
    {
        private IProductManager _productManager;
        private ICatalogManager _catalogManager;
        private IMapper _mapper;
        private static readonly ILog _logger = LogManager.GetLogger(typeof(ProductController));

        public ProductController(IProductManager productManager, ICatalogManager catalogManager, IMapper mapper)
        {
            _productManager = productManager;
            _catalogManager = catalogManager;
            _mapper = mapper;
        }

        public ActionResult Index(int page=1)
        {
            try
            {
                var products = _productManager.GetProducts(page, 10);
                return View(products);
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
                var product = _productManager.GetProduct(id);
                var productViewModel = _mapper.Map<ProductViewModel>(product);
                return View(productViewModel);
            }
            catch (Exception ex)
            {
                _logger.Error($"GET: Details(id: {id})", ex);
                return View("Error");
            }
        }

        public ActionResult DetailsPartial(ProductViewModel product)
        {
            return PartialView(product);
        }

        public ActionResult Create()
        {
            try
            {
                ViewBag.CatalogList = _catalogManager.GetSelectListCatalogs();
                return View();
            }
            catch (Exception ex)
            {
                _logger.Error("GET: Create()", ex);
                return View("Error");
            }
        }

        [HttpPost]
        public ActionResult Create(ProductModel product)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _productManager.InsertProduct(product);
                    return RedirectToAction("Index");
                }
                else
                {
                    return View();
                }
            }
            catch (UserFriendlyException ex)
            {
                ViewBag.CatalogList = _catalogManager.GetSelectListCatalogs();
                ViewBag.Error = ex.Message;
                return View();
            }
            catch (Exception ex)
            {
                ViewBag.CatalogList = _catalogManager.GetSelectListCatalogs();
                _logger.Error($"POST: Create({product.ToString()})", ex);
                return View("Error");
            }
        }

        public ActionResult Edit(int id)
        {
            try
            {
                var product = _productManager.GetProduct(id);
                var productModel = _mapper.Map<ProductModel>(product);
                ViewBag.CatalogList = _catalogManager.GetSelectListCatalogs();
                return View(productModel);
            }
            catch(Exception ex)
            {
                _logger.Error($"GET: Edit({id})", ex);
                return View("Error");
            }
        }


        [HttpPost]
        public ActionResult Edit(ProductModel product)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _productManager.UpdateProduct(product);
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.CatalogList = _catalogManager.GetSelectListCatalogs();
                    return View();
                }
            }
            catch (UserFriendlyException ex)
            {
                ViewBag.CatalogList = _catalogManager.GetSelectListCatalogs();
                ViewBag.Error = ex.Message;
                return View();
            }
            catch (Exception ex)
            {
                ViewBag.CatalogList = _catalogManager.GetSelectListCatalogs();
                _logger.Error($"POST: Edit({product.ToString()})", ex);
                return View("Error");
            }
        }

        public ActionResult Delete(int id)
        {
            try
            {
                var product = _productManager.GetProduct(id);
                var productModel = _mapper.Map<ProductViewModel>(product);
                return PartialView(productModel);
            }
            catch (Exception ex)
            {
                _logger.Error($"GET: Delete({id})", ex);
                return View("Error");
            }
        }

        [HttpPost]
        public ActionResult Delete(ProductViewModel product)
        {
            try
            {
                _productManager.DeleteProduct(product.Id);
                int page;
                int.TryParse(HttpUtility.ParseQueryString(Request.UrlReferrer.Query)["page"], out page);
                return RedirectToAction("Index", new { page = page == 0 ? 1 : page});
            }

            catch (UserFriendlyException ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.Error($"POST: Delete({product.ToString()})", ex);
                return View("Error");
            }
        }
    }
}