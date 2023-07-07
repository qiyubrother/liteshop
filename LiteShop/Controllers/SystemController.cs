using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LiteShop.Controllers
{
    public class SystemController : Controller
    {
        //
        // GET: /System/
        public ActionResult AboutUs()
        {
            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }
	}
}