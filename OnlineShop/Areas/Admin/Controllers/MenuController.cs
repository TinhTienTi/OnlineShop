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
    public class MenuController : BaseController
    {
        // GET: Admin/Product
        OnlineShopDbContext db = new OnlineShopDbContext();

        public ActionResult Index()
        {
            return View();
        }
        [HashCredential(RoleID = "EDIT_CONTENT")]
        // xem san pham. co phan trang 
        public ActionResult Select(string searchString, int page = 1, int pageSize = 10)
        {
            var dao = new MenuDao();
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
        public ActionResult Create(Menu menu)
        {
            if (menu.Text != null)
            {
                if (ModelState.IsValid)
                {
                    var dao = new MenuDao();
                    var session = (Common.UserLogin)Session[OnlineShop.Common.CommonConstants.USER_SESSION];
                    menu.Text = menu.Text;
                    menu.Link = menu.Link;
                    menu.DisplayOrder = menu.DisplayOrder;
                    menu.Status = menu.Status;
                    long id = dao.Insert(menu);
                    if (id > 0)
                    {
                      //  SetAlert("Thêm menu thành công", "success");
                        SetAlert(StaticResources.Resources.Addmenusuccessfully, "success");
                        return RedirectToAction("Select", "Menu");
                    }
                    else
                    {
                     //   ModelState.AddModelError("", "Thêm sản không thành công");
                        ModelState.AddModelError("", StaticResources.Resources.Addfailed);
                    }
                }
            }   
          //  SetAlert("Tên menu bắt buộc", "error");
            SetAlert(StaticResources.Resources.RequiredMenuname, "error");
            return View("Create");
        }


        public ActionResult Edit(int id)
        {
            //SetViewBag();
            var menu = new MenuDao().ViewDetail(id);
            return View(menu);
        }

        [HttpPost]
        public ActionResult Edit(Menu menu)
        {
            if (ModelState.IsValid)
            {
                var dao = new MenuDao();
               
                var session = (Common.UserLogin)Session[OnlineShop.Common.CommonConstants.USER_SESSION];
                menu.Text = menu.Text;
                menu.Link = menu.Link;
                menu.DisplayOrder = menu.DisplayOrder;
                menu.TypeID = menu.TypeID;
                menu.Status = menu.Status;
                var result = dao.Update(menu);
                if (result)
                {
                 //   SetAlert("Cập nhật menu thành công", "success");
                    SetAlert(StaticResources.Resources.Updatemenusuccessful, "success");
                    return RedirectToAction("Select", "Menu");
                }
                else
                {
                  //  ModelState.AddModelError("", "Cập nhật không thành công");
                    ModelState.AddModelError("", StaticResources.Resources.Updatefailed);
                }
            }
            //SetViewBag();
            return View("Select");
        }


        [HttpDelete] // xoa san pham
        public ActionResult Delete(int id)
        {
            new MenuDao().Delete(id);
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