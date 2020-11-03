using Models.DAO;
using Models.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using PagedList.Mvc;
using System.Text;
using System.Web.Script.Serialization;
using System.Xml.Linq;
using Common;


namespace OnlineShop.Areas.Admin.Controllers
{
    public class SlideController : BaseController
    {
        // GET: Admin/Product
        OnlineShopDbContext db = new OnlineShopDbContext();

        public ActionResult Index()
        {
            return View();
        }

        // xem san pham. co phan trang 
        public ActionResult Select(string searchString, int page = 1, int pageSize = 10)
        {
            var dao = new SlideDao();
            var model = dao.ListAllPaging(searchString, page, pageSize);
            ViewBag.SearchString = searchString;
            return View(model);
        }


        [HttpGet] //  truyen viewbag len view them san pham
        public ActionResult Create()
        {
            //SetViewBag();
            return View("Create");
        }



        [HttpPost] // them san pham
        public ActionResult Create(Slide slide)
        {
            if (slide.Description != null)
            {
                if (ModelState.IsValid)
                {
                    var dao = new SlideDao();
                    var session = (Common.UserLogin)Session[OnlineShop.Common.CommonConstants.USER_SESSION];
                    slide.Image = slide.Image;
                    slide.DisplayOrder = slide.DisplayOrder;
                    slide.Link = slide.Link;
                    slide.Description = slide.Description;
                    slide.CreatedDate = DateTime.Now;
                    slide.CreatedBy = session.UserName;
                    slide.ModifiedBy = session.UserName;
                    slide.ModifiedDate = DateTime.Now;
                    long id = dao.Insert(slide);
                    if (id > 0)
                    {

                        //  SetAlert("Thêm slide thành công", "success");
                        SetAlert(StaticResources.Resources.AddSlidesuccessfully, "success");
                        return RedirectToAction("Select", "Slide");
                    }
                    else
                    {
                     //   ModelState.AddModelError("", "Thêm slide không thành công");
                        ModelState.AddModelError("",StaticResources.Resources.Addfailed);
                    }
                }

            }
            //SetViewBag();
            // SetAlert("Thêm sản phẩm  không thành công", "success");
          //  SetAlert("Tên slide bắt buộc", "error");
            SetAlert(StaticResources.Resources.RequiredSlidename, "error");
            return View("Create");
        }


        public ActionResult Edit(int id)
        {
            //SetViewBag();
            var slide = new SlideDao().ViewDetail(id);
            return View(slide);
        }

        [HttpPost]
        public ActionResult Edit(Slide slide)
        {
            if (ModelState.IsValid)
            {
                var dao = new SlideDao();

                var session = (Common.UserLogin)Session[OnlineShop.Common.CommonConstants.USER_SESSION];
                slide.Image = slide.Image;
                slide.DisplayOrder = slide.DisplayOrder;
                slide.Link = slide.Link;
                slide.Description = slide.Description;
                slide.CreatedDate = DateTime.Now;
                slide.CreatedBy = session.UserName;
                slide.ModifiedBy = session.UserName;
                slide.ModifiedDate = DateTime.Now;
                slide.Status = slide.Status;
                var result = dao.Update(slide);
                if (result)
                {
                  //  SetAlert("Cập nhật slide thành công", "success");
                    SetAlert(StaticResources.Resources.Updatedslidessuccessfully, "success");
                    return RedirectToAction("Select", "Slide");
                }
                else
                {
                 //   ModelState.AddModelError("", "Cập nhật không thành công");
                    ModelState.AddModelError("", StaticResources.Resources.Updatefailed);
                }
            }
            //SetViewBag();
            return View("Select");
        }


        [HttpDelete] // xoa san pham
        public ActionResult Delete(int id)
        {
            new SlideDao().Delete(id);
          //  SetAlert("Xoá thành công", "success");
            SetAlert(StaticResources.Resources.Deletesuccessful, "success");
            return RedirectToAction("Index");
        }

        //// viewbag truyen dropdownlist category
        //public void SetViewBag(long? selectedId = null)
        //{
        //    var dao = new ProductCategoryDao();
        //    ViewBag.CategoryID = new SelectList(dao.ListAll(), "ID", "Name", selectedId);
        //}




    }
}