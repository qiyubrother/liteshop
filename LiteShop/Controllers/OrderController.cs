using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LiteShop.Models;

namespace LiteShop.Controllers
{
    public class OrderController : Controller
    {
        // GET: Order
        public ActionResult Index()
        {
            if (Session["mobile"] == null)
            {
                Session["returnUrl"] = "/Order/Index";
                return RedirectToAction("Login", "User");
            }
            var db = new LitShopEntities();
            var dt = DateTime.Now.AddDays(-7);
            var orders = db.TOrder.Where(x => x.CreateDate >= dt).OrderByDescending(x => x.ID);
            ViewData["queryType"] = "W";
            return View("Index", orders);
        }

        public ActionResult GetMonthData()
        {
            if (Session["mobile"] == null)
            {
                Session["returnUrl"] = "/Order/Index";
                return RedirectToAction("Login", "User");
            }
            var db = new LitShopEntities();
            var dt = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1, 0, 0, 0);
            var orders = db.TOrder.Where(x => x.CreateDate >= dt).OrderByDescending(x => x.ID);
            ViewData["queryType"] = "M";
            return View("Index", orders);
        }

        public ActionResult Get3MonthData()
        {
            if (Session["mobile"] == null)
            {
                Session["returnUrl"] = "/Order/Index";
                return RedirectToAction("Login", "User");
            }
            var db = new LitShopEntities();
            var tNow = DateTime.Now.AddMonths(-2);
            var dt = new DateTime(tNow.Year, tNow.Month, 1, 0, 0, 0);
            var orders = db.TOrder.Where(x => x.CreateDate >= dt).OrderByDescending(x => x.ID);
            ViewData["queryType"] = "Mn";
            return View("Index", orders);
        }

        public ActionResult GetYearData()
        {
            if (Session["mobile"] == null)
            {
                Session["returnUrl"] = "/Order/Index";
                return RedirectToAction("Login", "User");
            }
            var db = new LitShopEntities();
            var dt = new DateTime(DateTime.Now.Year, 1, 1, 0, 0, 0);
            var orders = db.TOrder.Where(x => x.CreateDate >= dt).OrderByDescending(x => x.ID);
            ViewData["queryType"] = "Y";
            return View("Index", orders);
        }

        public ActionResult GetAll()
        {
            if (Session["mobile"] == null)
            {
                Session["returnUrl"] = "/Order/Index";
                return RedirectToAction("Login", "User");
            }
            var db = new LitShopEntities();
            ViewData["queryType"] = "A";
            return View("Index", db.TOrder.OrderByDescending(x=>x.ID));
        }
    }
}