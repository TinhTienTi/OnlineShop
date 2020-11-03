using Models.DAO;
using Models.EF;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using OnlineShop.Areas.Admin.Models;

namespace OnlineShop.Areas.Admin.Controllers
{
    public class OrderController : BaseController
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
            var dao = new OrderDao();
            var model = dao.ListAllPaging(searchString, page, pageSize);
            ViewBag.SearchString = searchString;
            return View(model);
        }
        public ActionResult Detail(string searchString, int page = 1, int pageSize = 10)
        {
            var dao = new OrderDetailDao();
            var modelDetail = dao.ListAllPaging(searchString, page, pageSize);
            ViewBag.SearchString = searchString;
            return View(modelDetail);
        }
        // Xem danh sách chi tiết đơn hàng
        public ActionResult DetailByID(long? orderID, string searchString, int page = 1, int pageSize = 10)
        {
            var dao = new OrderDetailDao();
            var modelDetail = dao.List(orderID, searchString, page, pageSize);
            return View(modelDetail);;
        }

        [HttpDelete] // xoa san pham
        public ActionResult Delete(int id)
        {
            new OrderDao().Delete(id);
            SetAlert(StaticResources.Resources.Deletesuccessful, "success");
            return RedirectToAction("Index");
        }
        [HttpPost]
        public JsonResult DeleteUsingJson(int id)
        {
            List<DBOrder> listResult = DBOrder.Delete(id);
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
        //
        [HttpPost]
        public JsonResult JsonGetDetailOrderByOrderiD(int orderID)
        {
            List<DBOrderDetail> listResult = DBOrderDetail.GetProductByOrderID(orderID);
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
    }
}