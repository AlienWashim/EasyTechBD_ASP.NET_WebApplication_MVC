using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DynamicProductUpdate.Models;

namespace DynamicProductUpdate.Controllers
{
    public class HomeController : Controller
    {

        private ProductUpdateEntities2 db = new ProductUpdateEntities2();

        public ActionResult Index()
        {
            var products = db.Products.ToList();
            var categories = db.Categories.ToList();

            ViewBag.Categories = categories;
            return View(products);
        }

        public ActionResult UserDetails(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        public ActionResult GuestView(string category = "", string searchTerm = "")
        {
            var products = db.Products.AsQueryable();

            // Filter by category if provided
            if (!string.IsNullOrEmpty(category))
            {
                products = products.Where(p => p.Category.categoryName == category);
            }

            // Filter by search term if provided
            if (!string.IsNullOrEmpty(searchTerm))
            {
                products = products.Where(p => p.productName.Contains(searchTerm));
            }

            // Fetch categories for the filter dropdown
            ViewBag.Categories = db.Categories.ToList();

            return View(products.ToList());
        }



        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}