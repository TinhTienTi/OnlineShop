using Models.DAO;
using Models.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineShop.Areas.Admin.Controllers
{
    public class FooterController : BaseController
    {
        //
        // GET: /Admin/Footer/

        public ActionResult Index()
        {
            return View();
        }
        [HashCredential(RoleID = "EDIT_CONTENT")]
        public ActionResult Select(string searchString, int page = 1, int pageSize = 10)
        {
            var dao = new FooterDao();
            var model = dao.ListAllPaging(searchString, page, pageSize);
            ViewBag.SearchString = searchString;
            return View(model);
        }
        [HttpGet]
        public ActionResult Create()
        {
            //SetViewBag();
            return View("Create");
        }
        [HttpPost]
        public ActionResult Create(Footer footer)
        {
            if (footer.Content != null)
            {
                if (ModelState.IsValid)
                {
                    var dao = new FooterDao();
                    var session = (Common.UserLogin)Session[OnlineShop.Common.CommonConstants.USER_SESSION];
                    footer.Content = footer.Content;
                    footer.Status = footer.Status;
                    string id = dao.Insert(footer);
                }
            }
            return View("Create");
        }

        public ActionResult Edit(int id)
        {
            //SetViewBag();
            var footer = new FooterDao().ViewDetail(id);
            return View(footer);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult Edit(Footer footer)
        {
            if (ModelState.IsValid)
            {
                var dao = new FooterDao();
                var session = (Common.UserLogin)Session[OnlineShop.Common.CommonConstants.USER_SESSION];
                footer.Content = footer.Content;
                footer.Status = footer.Status;
                var result = dao.Update(footer);
                if (result)
                {

                    return RedirectToAction("Select", "Footer");
                }
                else
                {
                    ModelState.AddModelError("", StaticResources.Resources.Updatefailed);
                }
            }
            return View("Select");
        }

    }
}
