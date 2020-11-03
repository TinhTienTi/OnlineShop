using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Models.DAO;
using Models.EF;

namespace OnlineShop.Controllers
{
    public class AboutController : Controller
    {
        //
        // GET: /About/

        public ActionResult Index()
        {
             var model = new AboutDao().GetActiveAbout();
            return View(model);
        }
           
    }

    
}
