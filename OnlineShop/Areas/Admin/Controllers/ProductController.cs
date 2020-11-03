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
using OnlineShop.Areas.Admin.Models;

namespace OnlineShop.Areas.Admin.Controllers
{
    public class ProductController : BaseController
    {
        // GET: Admin/Product
        OnlineShopDbContext db = new OnlineShopDbContext();

        public ActionResult Index()
        {
            return View();
        }

        // xem san pham. co phan trang 
        [HashCredential(RoleID = "EDIT_CONTENT")]
        public ActionResult Select(string searchString, int page = 1, int pageSize = 5)
        {
            var dao = new ProductDao();
            var model = dao.ListAllPaging(searchString, page, pageSize);
            ViewBag.SearchString = searchString;
            return View(model);
        }
        [HashCredential(RoleID = "EDIT_CONTENT")]
        public ActionResult ListAllPagingByID(long? id, string searchString, int page = 1, int pageSize = 5)
        {
            var dao = new ProductDao();
            var model = dao.ListAllPagingByID(id, searchString, page, pageSize);
            ViewBag.SearchString = searchString;
            return View(model);
        }
        //  truyen viewbag len view them san pham
        [HttpGet]
        public ActionResult Create()
        {
            SetViewBag();
            return View("Create");
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult Create(Product product)
        {
            if (product.Name != null)
            {
                if (ProductNameModel.GetProductName(product.Name) == 1)
                {
                    SetViewBag();
                    SetAlert(StaticResources.Resources.ProductNameExists, "error");
                    return View("Create");
                }
                else if (ModelState.IsValid)
                {
                    var dao = new ProductDao();
                    product.MetaTitle = CommonConstants.convertToUnSign3(product.Name);
                    product.Description = product.Description;
                    product.MoreImages = ("<Images></Images>").ToString();
                    product.CreatedDate = DateTime.Now;
                    var session = (Common.UserLogin)Session[OnlineShop.Common.CommonConstants.USER_SESSION];
                    product.CreatedBy = session.UserName;
                    product.MetaKeywords = product.MetaTitle;
                    product.MetaDescriptions = product.MetaTitle;
                    product.TopHot = DateTime.Now;
                    product.ViewCount = product.ViewCount;
                    product.Status = product.Status;
                    long id = dao.Insert(product);
                    if (id > 0)
                    {
                        SetAlert(StaticResources.Resources.Addproductsuccessfully, "success");
                        return RedirectToAction("Select", "Product");
                    }
                    else
                    {
                        ModelState.AddModelError("", StaticResources.Resources.Addproductfailed);
                    }
                }

            }
            SetViewBag();
            SetAlert(StaticResources.Resources.Requiredproductname, "error");
            return View("Create");
        }

        [HashCredential(RoleID = "EDIT_USER")]
        public ActionResult Edit(int id)
        {
            SetViewBag();
            var product = new ProductDao().ViewDetail(id);
            return View(product);
        }

        [HttpPost, ValidateInput(false)]
        [HashCredential(RoleID = "EDIT_CONTENT")]
        public ActionResult Edit(Product product)
        {
            if (ModelState.IsValid)
            {
                var dao = new ProductDao();
                product.MetaTitle = CommonConstants.convertToUnSign3(product.Name);
                product.Description = product.Description;
                var session = (Common.UserLogin)Session[OnlineShop.Common.CommonConstants.USER_SESSION];
                product.ModifiedBy = session.UserName;
                product.MetaKeywords = product.MetaTitle;
                product.MetaDescriptions = product.MetaTitle;
                product.TopHot = DateTime.Now;
                product.Detail = product.Detail;
                product.ViewCount = product.ViewCount;
                product.Status = product.Status;
                var result = dao.Update(product);
                if (result)
                {
                    SetAlert(StaticResources.Resources.Productupdatesuccessful, "success");
                    return RedirectToAction("Select", "Product");
                }
                else
                {
                    ModelState.AddModelError("", StaticResources.Resources.Updatefailed);
                }
            }
            SetViewBag();
            return View("Select");
        }

        [HttpDelete] // xoa san pham
        public ActionResult Delete(int id)
        {
            new ProductDao().Delete(id);
            SetAlert(StaticResources.Resources.Deletesuccessful, "success");
            return RedirectToAction("Index");
        }
        [HttpPost]
        public JsonResult DeleteUsingJson(int id)
        {
            List<ProductModel> listResult = ProductModel.Delete(id);
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
            var dao = new ProductCategoryDao();
            ViewBag.CategoryID = new SelectList(dao.ListNotParentID(), "ID", "Name", selectedId);
        }

        // thay doi trang thai san pham
        public JsonResult ChangeStatus(long id)
        {
            var result = new ProductDao().ChangeStatus(id);
            return Json(new
            {
                status = result
            });
        }

        // luu nhiu hinh anh cho san pham
        public JsonResult SaveImages(long id, string images)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            var listImages = serializer.Deserialize<List<string>>(images);

            XElement xElement = new XElement("Images");

            foreach (var item in listImages)
            {
                var subStringItem = item.Substring(21);
                xElement.Add(new XElement("Image", subStringItem));
            }
            ProductDao dao = new ProductDao();
            try
            {
                dao.UpdateImages(id, xElement.ToString());
                return Json(new
                {
                    status = true
                });
            }
            catch (Exception)
            {
                return Json(new
                {
                    status = false
                });
            }

        }
        //cap nhat hinh anh cho san pham
        public JsonResult LoadImages(long id)
        {
            ProductDao dao = new ProductDao();
            var product = dao.ViewDetail(id);
            if (product.MoreImages == null)
            {
                return Json(new
                {
                    status = false
                });
            }
            var images = product.MoreImages;
            XElement xImages = XElement.Parse(images);
            List<string> listImagesReturn = new List<string>();

            foreach (XElement element in xImages.Elements())
            {
                listImagesReturn.Add(element.Value);
            }
            return Json(new
            {
                data = listImagesReturn
            }, JsonRequestBehavior.AllowGet);
        }
    }
}