using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Models.DAO;
using Models.EF;
using OnlineShop.Common;
using OnlineShop.Controllers;
using OnlineShop.Models;
namespace OnlineShop.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            ViewBag.Slides = new SlideDao().ListAll();
            var productDao = new ProductDao();
            ViewBag.NewProducts = productDao.ListNewProduct(8);
            ViewBag.NewProductss = productDao.ListNewProduct1(1);
            ViewBag.ListFeatureProducts = productDao.ListFeatureProduct(8);
            return View();
        }
        //Menu chinh
        [ChildActionOnly]
        public ActionResult MainMenu()
        {
            var model = new MenuDao().ListByGroupID(1);
            return PartialView(model);
        }
        //Menu Top
        [ChildActionOnly]
        public ActionResult TopMenu()
        {
            var model = new MenuDao().ListByGroupID(2);
            return PartialView(model);
        }
        [ChildActionOnly]
        public PartialViewResult HeaderCart()
        {
            var cart = Session[CommonConstants.CartSession];
            var list = new List<CartItem>();
            if(cart!=null)
            {
                list = (List<CartItem>)cart;
            }
            return PartialView(list);
        }
        //Footer
        [ChildActionOnly]
        public ActionResult Footer()
        {
            var model = new FooterDao().GetFooter();
            return PartialView(model);
        }
        // Chính sách bảo mật thanh toán
        public ActionResult PolicyPayment()
        {
            return View();
        }
        // Chính sách bảo mật thông tin cá nhân
        public ActionResult PolicyInfomation()
        {
            return View();
        }
        // Đổi trả
        public ActionResult Returns()
        {
            return View();
        }
        // Giao Nhận
        public ActionResult Shipping()
        {
            return View();
        }
    }
}
