
using Models.DAO;
using OnlineShop.Areas.Admin.Models;
using OnlineShop.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineShop.Areas.Admin.Controllers
{
    public class LoginController : Controller
    {
        // GET: Admin/Login
        public ActionResult Index()
        {
            return View();
        }

            public ActionResult Login(LoginModel model)
            {
                if (ModelState.IsValid)
                {
                    var dao = new UserDao();
                    var result = dao.Login(model.UserName, Encryptor.MD5Hash(model.PassWord), true);
                    if (result == 1)
                    {
                        var user = dao.GetById(model.UserName);
                        var userSession = new UserLogin();
                        userSession.UserName = user.UserName;
                        userSession.UserID = user.ID;
                        var listCredentials = dao.GetListCredential(model.UserName);
                        userSession.GroupID = user.GroupID;
                        Session.Add(CommonConstants.SESSION_CREDENTIALS, listCredentials);
                        Session.Add(CommonConstants.USER_SESSION, userSession);
                        return RedirectToAction("Index", "Home");
                    }
                    else if (result == 0)
                        ModelState.AddModelError("", " Tài Khoản Không Tồn Tại");
                    else if (result == -1)
                        ModelState.AddModelError("", " Tài Khoản Đang Bị Khoá");
                    else if (result == -2)
                        ModelState.AddModelError("", " Mật Khẩu Không Đúng");
                    else if (result == -3)
                        ModelState.AddModelError("", "Tài khoản của bạn không có quyền đăng nhập.");
                    else
                        ModelState.AddModelError("", "Đăng Nhập Không Đúng.");
                }
                return View("Index");
            }
    }
}