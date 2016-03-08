using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WalletOne.Infrastructure;

namespace WalletOne.Controllers
{
    public class NavController : Controller
    {
        public NavController() {
        }
        public PartialViewResult Menu(string category = null) {
            ViewBag.SelectedCategory = category;
            IEnumerable<string> categories = DataBaseExplorer.GetCategories();
            return PartialView(categories);
        }
    }
}