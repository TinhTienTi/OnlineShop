using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OnlineShop.Areas.Admin.Models;
namespace OnlineShop.Areas.Admin.Controllers
{
    public class HomeController : BaseController
    {
        // GET: Admin/Home
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public JsonResult ReportTop10ProductSelling()
        {
            List<ReportRevenue> listResult = ReportRevenue.Get();
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
        [HttpPost]
        public JsonResult ReportTop10QuantityProduct()
        {
            List<ReportTop5ProductQuantity> listResult = ReportTop5ProductQuantity.Get();
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