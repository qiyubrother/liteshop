using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using LiteShop.Models;

namespace LiteShop.Controllers
{
    public class UserController : Controller
    {
        //
        // GET: /User/
        public ActionResult Index()
        {
            if (Session["mobile"] == null)
            {
                Session["returnUrl"] = "/User/Index";
                return RedirectToAction("Login");
            }
            var mobile = Session["mobile"].ToString();
            var db = new Models.LitShopEntities();
            var user = db.TUser.FirstOrDefault(x => x.Mobile == mobile);
            if (user == null)
            {
                Session["returnUrl"] = "/User/Index";
                return RedirectToAction("Login");
            }

            return View(user);
        }

        public ActionResult Edit()
        {
            if (Session["mobile"] == null)
            {
                Session["returnUrl"] = "/User/Edit";
                return RedirectToAction("Login");
            }
            return View();
        }

        public ActionResult Info()
        {
            if (Session["mobile"] == null)
            {
                Session["returnUrl"] = "/User/Info";
                return RedirectToAction("Login");
            }
            return View();
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(string mobile, string password1, string mobileValidCode)
        {
            var db = new Models.LitShopEntities();
            if (db.TUser.FirstOrDefault(x => x.Mobile == mobile) != null)
            {
                return HttpNotFound("手机号已注册，请直接登录！");
            }
            var user = new TUser();
            user.Mobile = mobile;
            user.Psd = password1;
            db.TUser.Add(user);
            db.SaveChanges();
            Session["mobile"] = mobile;

            return RedirectToAction("Index");
        }
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string mobile, string password)
        {
            var db = new LiteShop.Models.LitShopEntities();
            var user = db.TUser.FirstOrDefault(x => x.Mobile == mobile);
            if (user == null)
            {
                // 用户不存在
                ModelState.AddModelError("mobile", "用户不存在");
            }
            else
            {
                if (user.Psd == password)
                {
                    // 认证通过
                    Session["mobile"] = mobile;
                    Session["userName"] = user.UserName;
                    Session["address"] = user.Address;
                    if (Session["returnUrl"] == null)
                    {
                        return RedirectToAction("Index", "User");
                    }
                    else
                    {
                        Response.Redirect((string)Session["returnUrl"]);
                        Session["returnUrl"] = null;
                    }
                }
                else
                {
                    // 错误的登录密码
                    ModelState.AddModelError("password", "错误的登录密码");
                }
            }
            return View();
        }

        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Login", "User");
        }

        public ActionResult Reset()
        {
            return View();
        }
	}
}