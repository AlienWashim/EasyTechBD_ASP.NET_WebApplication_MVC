using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DynamicProductUpdate;

namespace DynamicProductUpdate.Controllers
{
    [Authorize]
    public class ProductsController : Controller
    {
        private ProductUpdateEntities2 db = new ProductUpdateEntities2();

        // GET: Products
        public ActionResult Index()
        {
            var products = db.Products.Include(p => p.Category);
            return View(products.ToList());
        }

        // GET: Products/Details/5
        public ActionResult Details(int? id)
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

        

        // GET: Products/Create
        public ActionResult Create()
        {
            ViewBag.categoryId = new SelectList(db.Categories, "categoryId", "categoryName");
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "productId,productName,productDescription,productPrice,categoryId")] Product product, HttpPostedFileBase productImage)
        {
            if (ModelState.IsValid)
            {
                // Check if an image is uploaded
                if (productImage != null && productImage.ContentLength > 0)
                {
                    string fileExtension = Path.GetExtension(productImage.FileName);
                    string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(productImage.FileName);
                    string dateSuffix = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                    string fileName = $"{fileNameWithoutExtension}_{dateSuffix}{fileExtension}"; // Ensure the date suffix is added before the extension
                    string path = Path.Combine(Server.MapPath("~/Images"), fileName);
                    productImage.SaveAs(path);
                    product.productImage = "~/Images/" + fileName;
                }
                else
                {
                    product.productImage = "~/Images/Default.png";
                }


                db.Products.Add(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.categoryId = new SelectList(db.Categories, "categoryId", "categoryName", product.categoryId);
            return View(product);
        }


        // GET: Products/Edit/5
        public ActionResult Edit(int? id)
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
            ViewBag.categoryId = new SelectList(db.Categories, "categoryId", "categoryName", product.categoryId);
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "productId,productName,productDescription,productPrice,productImage,categoryId")] Product product, HttpPostedFileBase productImage)
        {
            if (ModelState.IsValid)
            {
                var existingProduct = db.Products.AsNoTracking().FirstOrDefault(p => p.productId == product.productId);

                // Check if an image is uploaded
                if (productImage != null && productImage.ContentLength > 0)
                {
                    string fileExtension = Path.GetExtension(productImage.FileName);
                    string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(productImage.FileName);
                    string dateSuffix = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                    string fileName = $"{fileNameWithoutExtension}_{dateSuffix}{fileExtension}"; // Ensure the date suffix is added before the extension
                    string path = Path.Combine(Server.MapPath("~/Images"), fileName);
                    productImage.SaveAs(path);
                    product.productImage = "~/Images/" + fileName;
                }
                else
                {
                    // If no new image is uploaded, retain the existing image
                    product.productImage = existingProduct?.productImage;
                }
                db.Products.Add(product);
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.categoryId = new SelectList(db.Categories, "categoryId", "categoryName", product.categoryId);
            return View(product);
        }

        // GET: Products/Delete/5
        public ActionResult Delete(int? id)
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

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Product product = db.Products.Find(id);
            db.Products.Remove(product);
            db.SaveChanges();
            return RedirectToAction("Index");
        }


        //For unauthorized view
        public ActionResult UserView()
        {
            var products = db.Products.ToList(); // Fetch products from the database
            return View(products); // Return the view with the list of products
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
