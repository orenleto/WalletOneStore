using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WalletOne.Domain.Entities;
using WalletOne.Infrastructure;
using WalletOne.Models;

namespace WalletOne.Controllers {
    public class ProductController : Controller {
        public static int PageSize = 4;

        public ProductController() { }
        public ViewResult List(string category, int page = 1) {
             ProductsListViewModel model = new ProductsListViewModel {
                Products = DataBaseExplorer.GetProductsFromDataBase(category, page, PageSize),
                PagingInfo = new PagingInfo {
                    CurrentPage = page,
                    ItemsPerPage = PageSize,
                    TotalItems = DataBaseExplorer.GetTotalCount(category)
                },
                CurrentCategory = category
            };
            return View(model);
        }
        public FileContentResult GetImage(int productId) {
            Product prod = DataBaseExplorer.GetProductFromDataBase(productId);
            if (prod != null) {
                return File(prod.ImageData, prod.ImageMimeType);
            } else {
                return null;
            }
        }

        
    }
}