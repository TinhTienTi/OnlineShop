using Common;
using Models.DAO;
using Models.EF;
using OnlineShop.Areas.Admin.Models;
using OnlineShop.Common;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace OnlineShop.Areas.Admin.Controllers
{
    public class UserController : BaseController
    {

        public ActionResult Logout()
        {
            Session[Common.CommonConstants.USER_SESSION] = null;
            return RedirectToAction("Index", "Login");
        }
        public ActionResult Select(string searchString, int page = 1, int pageSize = 5)
        {
            var dao = new UserDao();
            var model = dao.ListAllPaging(searchString, page, pageSize);
            ViewBag.SearchString = searchString;
            return View(model);
        }

        [HashCredential(RoleID = "EDIT_USER")]
        public ActionResult Edit(int id)
        {
            var user = new UserDao().ViewDetail(id);
            return View(user);
        }

        public void SetViewBag(string selectedId = null)
        {
            var dao = new UserGroupDao();
            ViewBag.GroupID = new SelectList(dao.ListAll(), "ID", "Name", selectedId);
        }
        [HttpGet]
        [HashCredential(RoleID = "ADD_USER")]
        public ActionResult Create()
        {
            SetViewBag();
            return View();
        }

        [HttpPost]
        [HashCredential(RoleID = "ADD_USER")]
        public ActionResult Create(User user)
        {
            if (user.UserName == null )
            {
                SetViewBag();
                SetAlert(StaticResources.Resources.Accountnamerequired, "error");
                return View("Create");
            }
            else if(UserNameModel.GetUserName(user.UserName) == 1)
            {
                SetViewBag();
                SetAlert(StaticResources.Resources.AccountnameExists, "error");
                return View("Create");
            }
            else if (user.Password == null)
            {
                SetViewBag();
                SetAlert(StaticResources.Resources.PasswordRequired, "error");
                return View("Create");
            }
            else if (ModelState.IsValid)
            {
                var dao = new UserDao();
                user.Password = Encryptor.MD5Hash(user.Password);
                user.GroupID = user.GroupID;
                var session = (Common.UserLogin)Session[OnlineShop.Common.CommonConstants.USER_SESSION];
                user.CreatedBy = session.UserName;
                user.CreatedDate = DateTime.Now;
                long id = dao.Insert(user);
                if (id > 0)
                {
                    SetAlert(StaticResources.Resources.Addaccountsuccessfully, "success");
                    return RedirectToAction("Select", "User");
                }
                else
                {
                    ModelState.AddModelError("", StaticResources.Resources.Addingaccountfailed);
                }
            }
            return View("Create");
        }

        [HttpPost]
        [HashCredential(RoleID = "EDIT_USER")]
        public ActionResult Edit(User user)
        {
            if (ModelState.IsValid)
            {
                var dao = new UserDao();
                var session = (Common.UserLogin)Session[OnlineShop.Common.CommonConstants.USER_SESSION];
                user.ModifiedBy = session.UserName;
                user.ModifiedDate = DateTime.Now;
                var result = dao.Update(user);
                if (result)
                {
                    //   SetAlert("Cập nhật tài khoản thành công", "success");
                    SetAlert(StaticResources.Resources.Updateaccountsuccessfully, "success");
                    return RedirectToAction("Select", "User");
                }
                else
                {
                    //   ModelState.AddModelError("", "Cập nhật tài khoản không thành công");
                    ModelState.AddModelError("", StaticResources.Resources.Updatefailed);
                }
            }
            return View("Select");
        }

        [HttpDelete]
        [HashCredential(RoleID = "EDIT_USER")]
        public ActionResult Delete(int id)
        {
            new UserDao().Delete(id);
            return RedirectToAction("Index");
        }
        [HashCredential(RoleID = "EDIT_USER")]
        public JsonResult ChangeStatus(long id)
        {
            var result = new UserDao().ChangeStatus(id);
            return Json(new
            {
                status = result
            });
        }
        [HttpPost]
        public JsonResult DeleteUsingJson(int id)
        {
            List<UserModel> listResult = UserModel.Delete(id);
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
        public JsonResult GetGroupUser()
        {
            List<UserGroupModel> listResult = UserGroupModel.Get();
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