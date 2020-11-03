using Models.DAO;
using Models.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineShop.Areas.Admin.Controllers
{
    public class FeedBackController : BaseController
    {
        OnlineShopDbContext db = new OnlineShopDbContext();

        public ActionResult Index()
        {
            return View();
        }

        // xem san pham. co phan trang 
        public ActionResult Select(string searchString, int page = 1, int pageSize = 10)
        {
            var dao = new FeedBackDao();
            var model = dao.ListAllPaging(searchString, page, pageSize);
            ViewBag.SearchString = searchString;
            return View(model);
        }


        
        [HttpDelete] 
        public ActionResult Delete(int id)
        {
            new FeedBackDao().Delete(id);
            return RedirectToAction("Index");
        }

    }
}
