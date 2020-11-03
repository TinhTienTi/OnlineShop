using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineShop.Areas.Admin.Models
{
    public class Parameter
    {
        public static string connect = System.Configuration.ConfigurationManager.ConnectionStrings["OnlineShopDbConnect"].ToString();
    }
}