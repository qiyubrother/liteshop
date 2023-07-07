using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LiteShop.Models;

namespace LiteShop.Controllers
{
    public class CartController : Controller
    {
        //
        // GET: /Cart/
        // 显示当前购物车
        public ActionResult Index()
        {
            return new ViewResult();
        }
        // 添加商品到购物车
        [HttpPost]
        public ActionResult AddToChart(int goodsId, int amount = 1)
        {
            if (Session["mobile"] == null)
            {
                Session["returnUrl"] = "/";
                return HttpNotFound();
            }
            var db = new Models.LitShopEntities();
            var goods = db.TGoods.FirstOrDefault(x => x.ID == goodsId);
            if (goods == null)
            {
                return HttpNotFound();
            }
            else
            {
                var mobile = Session["mobile"].ToString();
                var cart =
                    db.TUserCarts.FirstOrDefault(x => x.Mobile == mobile && x.GoodsId == goodsId);
                if (cart == null)
                {
                    cart = new TUserCarts();
                    cart.Mobile = Session["mobile"].ToString();
                    cart.GoodsId = goodsId;
                    cart.Amount = amount;
                    db.TUserCarts.Add(cart);
                    db.SaveChanges();
                }
                else
                {
                    cart.Amount += amount;
                    db.SaveChanges();
                }

            }

            return new ViewResult();
        }

        [HttpPost]
        public ActionResult RemoveChart(int goodsId)
        {
            var db = new LitShopEntities();
            var mobile = Session["mobile"].ToString();
            var cart =
                db.TUserCarts.FirstOrDefault(x => x.Mobile == mobile && x.GoodsId == goodsId); 
            if (cart != null)
            {
                db.TUserCarts.Remove(cart);
                db.SaveChanges();
            }
            return new ViewResult();
        }

        [HttpPost]
        public ActionResult UpdateAmount(int goodsId, int newAmount)
        {
            var db = new LitShopEntities();
            var mobile = Session["mobile"].ToString();
            var cart =
                db.TUserCarts.FirstOrDefault(x => x.Mobile == mobile && x.GoodsId == goodsId); 
            if (cart != null)
            {
                cart.Amount = newAmount;
                db.SaveChanges();
            }
            return new ViewResult();
        }

        /// <summary>
        /// 购买/结算
        /// </summary>
        /// <returns></returns>
        public ActionResult Buy()
        {
            if (Session["mobile"] == null)
            {
                Session["returnUrl"] = "/Cart/Buy";
                return RedirectToAction("Login", "User");
            }
            var db = new LitShopEntities();
            var mobile = Session["mobile"].ToString();
            var query = (from x in db.TUserCarts
                         join e in db.TGoods on x.GoodsId equals e.ID into te
                         from y in te
                         select new { x.Mobile,
                                x.GoodsId,
                                y.GoodsName,
                                y.Price,
                                x.Amount,
                                y.ImgSource
                                }).ToList().Select(x=>
                                    Tuple.Create(
                                        x.Mobile,
                                        x.GoodsId,
                                        x.GoodsName,
                                        x.Price,
                                        x.Amount,
                                        x.ImgSource
                                        )).Where(x => x.Item1 == mobile);
            ViewData["data"] = query;
            return View();
        }

        public ActionResult CreateOrder()
        {
            if (Session["mobile"] == null)
            {
                return RedirectToAction("Login", "User");
            }
            var db = new LitShopEntities();
            var order = new TOrder();
            var mobile = Session["mobile"].ToString();
            order.OrderNo = string.Format("{0}{1}{2}{3}{4}{5}",
                DateTime.Now.Year,
                DateTime.Now.Month.ToString().PadLeft(2, '0'),
                DateTime.Now.Day.ToString().PadLeft(2, '0'),
                DateTime.Now.Hour.ToString().PadLeft(2, '0'),
                DateTime.Now.Minute.ToString().PadLeft(2, '0'),
                DateTime.Now.Second.ToString().PadLeft(2, '0')
                );
            order.Mobile = mobile;
            order.CreateDate = DateTime.Now;
            db.TOrder.Add(order);
            db.SaveChanges();
            order = db.TOrder.FirstOrDefault(x => x.OrderNo == order.OrderNo);
            if (order == null)
            {
                // 系统错误
            }
            else
            {
                var no = 1;
                var Carts = (from x in db.TUserCarts
                             join e in db.TGoods on x.GoodsId equals e.ID into te
                             from y in te
                             select new
                             {
                                 x.Mobile,
                                 x.GoodsId,
                                 y.GoodsName,
                                 y.Price,
                                 y.SpecialPrice,
                                 x.Amount,
                                 y.ImgSource
                             }
                                        ).Where(x => x.Mobile == mobile).ToList();

                foreach(var cart in Carts)
                {
                    var orderDetail = new TOrderDetails();
                    orderDetail.OrderID = order.ID;
                    orderDetail.No = no++;
                    orderDetail.goodsId = cart.GoodsId;
                    orderDetail.GoodsName = cart.GoodsName;
                    orderDetail.Price = cart.Price;
                    orderDetail.Amount = cart.Amount;
                    orderDetail.SubTotal = cart.Price * cart.Amount;
                    orderDetail.ImgSource = cart.ImgSource;
                    db.TOrderDetails.Add(orderDetail);
                }

                var carts = db.TUserCarts.Where(x => x.Mobile == mobile);
                db.TUserCarts.RemoveRange(carts);
                db.SaveChanges();
            }
            return RedirectToAction("Index", "Mobile");
        }
    }
    //public class Cart
    //{
    //    public Models.TGoods Goods { get; set; }

    //    public int Amount { get; set; }
    //}
}