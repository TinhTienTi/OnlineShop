using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model.Dao;
using Models.DAO;
using PagedList;
using PagedList.Mvc;
using Models;
using OnlineShop.Models;

namespace OnlineShop.Controllers
{
    public class ProductController : Controller
    {
        //
        // GET: /Product
        public ActionResult Index()
        {
            var productDao = new ProductDao();
            ViewBag.NewProducts = productDao.ListNewProduct(4);
            ViewBag.ListProduct = productDao.ListProduct();
            ViewBag.ListFeatureProducts = productDao.ListFeatureProduct(4);
            return View();
        }
        [ChildActionOnly]
        public PartialViewResult ProductCategory()
        {
            var model = new ProductCategoryDao().ListAll();
            return PartialView(model);
        }
        [ChildActionOnly]
        public PartialViewResult DMCategory()
        {
            var model = new CategoryDao().ListAll1();
            return PartialView(model);
        }
        public JsonResult ListName(string q)
        {
            var data = new ProductDao().ListName(q);
            return Json(new
            {
                data = data,
                status = true
            }, JsonRequestBehavior.AllowGet);
        }
        //Phan trang danh sach san pham
        public ActionResult Category(long cateId, int page = 1, int pageSize = 4)
        {
            var category = new CategoryDao().ViewDetail(cateId);
            ViewBag.Category = category;
            int totalRecord = 0;
            var model = new ProductDao().ListProductPaging(cateId, ref totalRecord, page, pageSize);
            ViewBag.Total = totalRecord;
            ViewBag.Page = page;
            int maxPage = 5;
            int totalPage = 0;
            //Math.Ceiling lam tron len
            totalPage = (int)Math.Ceiling((double)(totalRecord / pageSize));
            ViewBag.TotalPage = totalPage;
            ViewBag.MaxPage = maxPage;
            ViewBag.First = 1;
            ViewBag.Last = totalPage;
            ViewBag.Next = page + 1;
            ViewBag.Prev = page - 1;
            return View(model);
        }
        public ActionResult Category1(long cateId, int page = 1, int pageSize = 4)
        {
            var category = new CategoryDao().ViewDetail1(cateId);
            ViewBag.Category = category;
            int totalRecord = 0;
            var model = new ContentDao().ListProductPaging1(cateId, ref totalRecord, page, pageSize);

            ViewBag.Total = totalRecord;
            ViewBag.Page = page;

            int maxPage = 5;
            int totalPage = 0;
            //Math.Ceiling lam tron len
            totalPage = (int)Math.Ceiling((double)(totalRecord / pageSize));
            ViewBag.TotalPage = totalPage;
            ViewBag.MaxPage = maxPage;
            ViewBag.First = 1;
            ViewBag.Last = totalPage;
            ViewBag.Next = page + 1;
            ViewBag.Prev = page - 1;
            return View(model);
        }
        public ActionResult Search(string keyword, int page = 1, int pageSize = 1)
        {
            int totalRecord = 0;
            var model = new ProductDao().Search(keyword, ref totalRecord, page, pageSize);

            ViewBag.Total = totalRecord;
            ViewBag.Page = page;
            ViewBag.Keyword = keyword;
            int maxPage = 5;
            int totalPage = 0;

            totalPage = (int)Math.Ceiling((double)(totalRecord / pageSize));
            ViewBag.TotalPage = totalPage;
            ViewBag.MaxPage = maxPage;
            ViewBag.First = 1;
            ViewBag.Last = totalPage;
            ViewBag.Next = page + 1;
            ViewBag.Prev = page - 1;

            return View(model);
        }
        public ActionResult Detail(long cateId)
        {
            var product = new ProductDao().ViewDetail(cateId);
            ViewBag.Category = new ProductCategoryDao().ViewDetail(product.CategoryID.Value);
            ViewBag.RelatedProducts = new ProductDao().ListRelatedProducts(cateId);
            return View(product);
        }
        public ActionResult Detail1(long cateId)
        {
            var content = new ContentDao().GetByID(cateId);
            ViewBag.Category = new CategoryDao().ViewDetail1(content.CategoryID.Value);
            ViewBag.RelatedProducts = new ContentDao().ListRelatedProducts(cateId);
            ViewBag.User = new UserDao().ViewDetail(cateId);
            return View(content);
        }
        //Update viewcount
        [HttpPost]
        public void UpdateUsingStored(long id)
        {
            ProductViewCount.Update(id);
        }
    }
}
