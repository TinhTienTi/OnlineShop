using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OnlineShop.Areas.Admin.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Chưa Nhập Tài Khoản ")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Chưa Nhập Mật Khẩu ")]
        public string PassWord { get; set; }

        public bool RememberMe { get; set; }
    }
}