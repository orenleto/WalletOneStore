
using WalletOne.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WalletOne.Infrastructure;

namespace WalletOne.Controllers
{
    public class AdminController : Controller {
        public AdminController() {
        }
        public ViewResult Index() {
            return View(DataBaseExplorer.GetProductsFromDataBase(null, 1, DataBaseExplorer.GetTotalCount()));
        }
        public ViewResult Edit(int productId) {
            Product product = DataBaseExplorer.GetProductFromDataBase(productId);
            return View(product);
        }
        [HttpPost]
        public ActionResult Edit(Product product, HttpPostedFileBase image) {
            if (ModelState.IsValid)
            {
                Product prev = DataBaseExplorer.GetProductFromDataBase(product.ProductID);
                if (prev != null) {
                    product.ImageMimeType = prev.ImageMimeType;
                    product.ImageData = prev.ImageData;
                }
                DataBaseExplorer.InsertProduct(product.ProductID == 0, product, image);
                TempData["message"] = string.Format("{0} has been saved", product.Name);
                return RedirectToAction("Index");
            } else {
                return View(product);
            }
        }
        public ViewResult Create() {
            return View("Edit", new Product());
        }
        [HttpPost]
        public ActionResult Delete(int productId) {
            DataBaseExplorer.DeleteProduct(productId);
            return RedirectToAction("Index");
        }
    }
}