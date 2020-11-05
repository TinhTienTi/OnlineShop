using Common;
using Models.DAO;
using Models.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OnlineShop.Areas.Admin.Models;
namespace OnlineShop.Areas.Admin.Controllers
{
    public class ProductCategoryController : BaseController
    {
        //
        // GET: /Admin/ProductCategory/
        OnlineShopDbContext db = new OnlineShopDbContext();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Select(string searchString, int page = 1, int pageSize = 5)
        {
            var dao = new ProductCategoryDao();
            var model = dao.ListAllPaging(searchString, page, pageSize);
            ViewBag.SearchString = searchString;
            return View(model);
        }
        [HttpGet] //  truyen viewbag len view them san pham
        public ActionResult Create()
        {
            SetParentID();
            return View("Create");
        }

        [HttpPost] // them san pham
        public ActionResult Create(ProductCategory pdcategory)
        {
            if (pdcategory.Name != null)
            {
                if (GetProductCateName.GetProdCateName(pdcategory.Name) == 1)
                {
                    SetParentID();
                    SetAlert(StaticResources.Resources.ProdCateNameExists, "error");
                    return View("Create");
                }
                else if (ModelState.IsValid)
                {
                    var dao = new ProductCategoryDao();
                    pdcategory.MetaTitle = pdcategory.MetaTitle;
                    pdcategory.CreatedDate = DateTime.Now;
                    var session = (Common.UserLogin)Session[OnlineShop.Common.CommonConstants.USER_SESSION];
                    pdcategory.CreatedBy = session.UserName;
                    pdcategory.MetaKeywords = pdcategory.MetaTitle;
                    pdcategory.MetaDescriptions = pdcategory.MetaTitle;
                    long id = dao.Insert(pdcategory);
                    if (id > 0)
                    {
                        SetAlert(StaticResources.Resources.Addsuccessfulproductcategories, "success");
                        return RedirectToAction("Select", "ProductCategory");
                    }
                    else
                    {
                        ModelState.AddModelError("", StaticResources.Resources.Addproducttypefailed);
                    }
                }
            }
            SetParentID();
            SetAlert(StaticResources.Resources.Requiredproductname, "error");
            return View("Create");
        }
        public ActionResult Edit(int id)
        {
            SetParentID();
            var pdproduct = new ProductCategoryDao().ViewDetail(id);
            return View(pdproduct);
        }

        [HttpPost]
        public ActionResult Edit(ProductCategory pdproduct)
        {
            if (ModelState.IsValid)
            {
                var dao = new ProductCategoryDao();
                var session = (Common.UserLogin)Session[OnlineShop.Common.CommonConstants.USER_SESSION];
                pdproduct.Name = pdproduct.Name;
                pdproduct.MetaTitle = pdproduct.MetaTitle;
                pdproduct.CreatedBy = session.UserName;
                pdproduct.ModifiedBy = session.UserName;

                var result = dao.Update(pdproduct);
                if (result)
                {
                    SetAlert(StaticResources.Resources.Updateproducttypesuccessfully, "success");
                    return RedirectToAction("Select", "ProductCategory");
                }
                else
                {
                    //   ModelState.AddModelError("", "Cập nhật không thành công");
                    ModelState.AddModelError("", StaticResources.Resources.Updatefailed);
                }
            }
            SetParentID();
            return View("Select");
        }

        [HttpDelete] // xoa san pham
        public ActionResult Delete(int id)
        {
            new ProductCategoryDao().Delete(id);
            SetAlert(StaticResources.Resources.Deletesuccessful, "success");
            return RedirectToAction("Index");
        }
        [HttpPost]
        public JsonResult DeleteUsingJson(int id)
        {
            List<ProductCateModel> listResult = ProductCateModel.Delete(id);
            if (listResult.Count > 0)
            {
                var listSuccess = new
                {
                    Result = 1,
                    Today = DateTime.Now.ToString(),
                    ListResult = listResult
                };
                var jsonResult = Json(listSuccess, JsonRequestBehavior.AllowGet);
                jsonResult.MaxJsonLength = int.MaxValue;
                return jsonResult;
            }
            else
                return Json(new { Result = 0, Error = string.Format("Không tìm thấy thông tin", ""), JsonRequestBehavior.AllowGet });
        }

        // viewbag truyen dropdownlist category
        public void SetViewBag(long? selectedId = null)
        {
            var dao = new CategoryDao();
            ViewBag.CategoryID = new SelectList(dao.ListAll(), "ID", "Name", selectedId);
        }
        public void SetParentID(long? selectedId = null)
        {
            var dao = new ProductCategoryDao();
            ViewBag.ParentID = new SelectList(dao.ListParentID(), "ID", "Name", selectedId);
        }
    }
}
