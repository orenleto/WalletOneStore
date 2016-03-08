using WalletOne.Domain.Entities;
using WalletOne.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WalletOne.HtmlHelpers;
using WalletOne.Infrastructure;

namespace WalletOne.Controllers
{
    public class CartController : Controller
    {
        public CartController() {}
        public ViewResult Index(Cart cart, string returnUrl) {
            return View(new CartIndexViewModel {
                Cart = cart,
                ReturnUrl = returnUrl
            });
        }
        public RedirectToRouteResult AddToCart(Cart cart, int productId, string returnUrl)
        {
            Product product = DataBaseExplorer.GetProductFromDataBase(productId);
            if (product != null) {
                cart.AddItem(product, 1);
            }
            return RedirectToAction("Index", new { returnUrl });
        }
        public RedirectToRouteResult TakeFromCart(Cart cart, int productId, string returnUrl) {
            Product product = DataBaseExplorer.GetProductFromDataBase(productId);
            if (product != null) {
                cart.AddItem(product, -1);
            }
            return RedirectToAction("Index", new { returnUrl });
        }
        public RedirectToRouteResult RemoveFromCart(Cart cart, int productId, string returnUrl) {
            Product product = DataBaseExplorer.GetProductFromDataBase(productId);
            if (product != null) {
                cart.RemoveLine(product);
            }
            return RedirectToAction("Index", new { returnUrl });
        }
        public PartialViewResult Summary(Cart cart) {
            return PartialView(cart);
        }
        [HttpPost]
        public ViewResult Checkout(Cart cart, ShippingDetails shippingDetails) {
            if (cart.Lines.Count() == 0) {
                ModelState.AddModelError("", "Sorry, your cart is empty!");
            }
            if (ModelState.IsValid) {
                System.Web.HttpContext.Current.Items.Add("Cart", cart);
                System.Web.HttpContext.Current.Items.Add("ShippingDetails", shippingDetails);
                PaymentForm paymentForm = new PaymentForm();
                paymentForm.ProcessRequest(System.Web.HttpContext.Current);
                cart.Clear();
                return View("Completed");
            } else {
                return View(shippingDetails);
            }
        }
        public ViewResult Checkout()
        {
            return View(new ShippingDetails());
        }
    }
}