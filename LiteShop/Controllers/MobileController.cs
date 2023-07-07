using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LiteShop.Models;
namespace LiteShop.Controllers
{
    public class MobileController : Controller
    {
        //
        // GET: /Mobile/
        public ActionResult Index()
        {
            var db = new LiteShop.Models.LitShopEntities();
            return View(db.TGoods.ToList());
        }

        /// <summary>
        /// 系统消息
        /// </summary>
        /// <returns></returns>
        public ActionResult SystemMessage()
        {
            return View();
        }

       
	}
}