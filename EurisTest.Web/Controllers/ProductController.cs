using System.Web.Mvc;
using EURIS.Service;

namespace EurisTest.Web.Controllers
{
    public class ProductController : Controller
    {
        public ActionResult Index()
        {
            var productManager = new ProductManager();
            var products = productManager.GetProducts();
            ViewBag.Products = products;
            return View();
        }
    }
}