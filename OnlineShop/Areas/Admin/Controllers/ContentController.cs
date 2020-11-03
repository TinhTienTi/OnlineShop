
using Model.Dao;
using Models.DAO;
using Models.EF;
using OnlineShop.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineShop.Areas.Admin.Controllers
{
    public class ContentController : BaseController
    {
        // GET: Admin/Content
       
        public ActionResult Index(string searchString, int page = 1, int pageSize = 10)
        {
            var dao = new ContentDao();
            var model = dao.ListAllPaging(searchString, page, pageSize);

            ViewBag.SearchString = searchString;
            return View(model);
        }

        [HttpGet]
        public ActionResult Create()
        {
            SetViewBag();
            return View();
        }

        [HttpGet]
        public ActionResult Edit(long id)
        {
            var dao = new ContentDao();
            var content = dao.GetByID(id);

            SetViewBag(content.CategoryID);

            return View(content);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(Content content)
        {
            if (ModelState.IsValid)
            {
                var dao = new ContentDao();

                var session = (Common.UserLogin)Session[OnlineShop.Common.CommonConstants.USER_SESSION];
                content.Name = content.Name;
                content.Image = content.Image;
                content.Detail = content.Detail;
                content.CreatedDate = content.CreatedDate;
                content.Status = content.Status;
                var result = dao.Edit(content);
                if (result > 0)
                {
                    SetAlert(StaticResources.Resources.Updatenewssuccessfully, "success");
                    return RedirectToAction("Index", "Content");
                }
                else
                {
                    ModelState.AddModelError("", StaticResources.Resources.Updatefailed);
                }
            }
            SetViewBag(content.CategoryID);
            return View("Index");
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(Content model)
        {
            if (ModelState.IsValid)
            {
                var session = (UserLogin)Session[CommonConstants.USER_SESSION];
                model.CreatedBy = session.UserName;
                var culture = Session[CommonConstants.CurrentCulture];
                new ContentDao().Create(model);
                return RedirectToAction("Index");
            }
            SetViewBag();
            return View();
        }

        public void SetViewBag(long? selectedId = null)
        {
            var dao = new CategoryDao();
            ViewBag.CategoryID = new SelectList(dao.ListAll(), "ID", "Name", selectedId);
        }
        public ActionResult Delete(int id)
        {
            new ContentDao().Delete(id);
           // SetAlert("Xoá thành công", "success");
            SetAlert(StaticResources.Resources.Deletesuccessful, "success");
            return RedirectToAction("Index");
        }
    }
}