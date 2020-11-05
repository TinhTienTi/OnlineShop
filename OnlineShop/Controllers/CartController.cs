using OnlineShop.Models;
using Models.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Models.EF;
using System.Web.Script.Serialization;
using System.Configuration;
using Common;

namespace OnlineShop.Controllers
{
    public class CartController : Controller
    {
        public ActionResult Index()
        {
            var cart = Session[OnlineShop.Common.CommonConstants.CartSession];
            List<CartItem> list = new List<CartItem>();
            if (cart != null)
            {
                list = (List<CartItem>)cart;
            }
            return View(list);
        }
        public ActionResult AddItem(long productId, int quantity)
        {
            var product = new ProductDao().ViewDetail(productId);
            var cart = Session[OnlineShop.Common.CommonConstants.CartSession];
            List<CartItem> list = new List<CartItem>();
            if (cart != null)
            {
                list = (List<CartItem>)cart;
                if (list.Exists(x => x.Product.ID == productId))
                {
                    foreach (var item in list)
                    {
                        if (item.Product.ID == productId)
                        {
                            item.Quantity += quantity;
                        }
                    }
                }
                else
                {
                    //tạo mới đối tượng cart item
                    var item = new CartItem();
                    item.Product = product;
                    item.Quantity = quantity;
                    list.Add(item);
                }
                //Gán vào session
                Session[OnlineShop.Common.CommonConstants.CartSession] = list;
            }
            else
            {
                //tạo mới đối tượng cart item
                var item = new CartItem();
                item.Product = product;
                item.Quantity = quantity;
                list = new List<CartItem>();
                list.Add(item);
                //Gán vào session
                Session[OnlineShop.Common.CommonConstants.CartSession] = list;
            }
            return RedirectToAction("Index");
        }

        public JsonResult Delete(long id)
        {
            var sessionCart = (List<CartItem>)Session[OnlineShop.Common.CommonConstants.CartSession];
            sessionCart.RemoveAll(x => x.Product.ID == id);
            Session[OnlineShop.Common.CommonConstants.CartSession] = sessionCart;
            return Json(new
            {
                status = true
            });
        }
        public JsonResult DeleteAll()
        {
            Session[OnlineShop.Common.CommonConstants.CartSession] = null;
            return Json(new
            {
                status = true
            });
        }
        public JsonResult Update(string cartModel)
        {
            var jsonCart = new JavaScriptSerializer().Deserialize<List<CartItem>>(cartModel);
            var sessionCart = (List<CartItem>)Session[OnlineShop.Common.CommonConstants.CartSession];
            foreach (var item in sessionCart)
            {
                var jsonItem = jsonCart.SingleOrDefault(x => x.Product.ID == item.Product.ID);
                if (jsonItem != null)
                {
                    var num = QuantityProduct(item.Product.ID);
                    if (num >= jsonItem.Quantity)
                    {
                        item.Quantity = jsonItem.Quantity;
                    }
                    else
                    {

                    }
                }
            }
            Session[OnlineShop.Common.CommonConstants.CartSession] = sessionCart;
            return Json(new
            {
                status = true
            });
        }
        [HttpGet]
        public ActionResult Payment()
        {
            var cart = Session[OnlineShop.Common.CommonConstants.CartSession];
            var list = new List<CartItem>();
            if (cart != null)
            {
                list = (List<CartItem>)cart;
            }
            return View(list);
        }
        [HttpPost]
        public ActionResult Payment(string shipName, string mobile, string address, string email)
        {
            var order = new Order();
            order.CreatedDate = DateTime.Now;
            order.ShipAddress = address;
            order.ShipMobile = mobile;
            order.ShipName = shipName;
            order.ShipEmail = email;
            order.Status = 0;
            order.TypePayMent = "Thanh Toán Khi Nhận Hàng";
            order.StatusOrder = "Active";
            try
            {
                var id = new OrderDao().Insert(order);
                var cart = (List<CartItem>)Session[OnlineShop.Common.CommonConstants.CartSession];
                var detailDao = new OrderDetailDao();
                decimal total = 0;
                foreach (var item in cart)
                {
                    var orderDetail = new OrderDetail();
                    orderDetail.ProductID = item.Product.ID;
                    orderDetail.Quantity = item.Quantity;
                    orderDetail.OrderID = id;
                    orderDetail.Price = (item.Product.PromotionPrice.HasValue ? item.Product.PromotionPrice : item.Product.Price);
                    // Thêm chi tiết đơn hàng
                    OrderDetailModel.Insert(orderDetail.ProductID, orderDetail.OrderID, orderDetail.Quantity, orderDetail.Price);
                    // Cập nhật lại số lượng sản phẩm
                    UpdateProductQuantity.Update(orderDetail.ProductID, orderDetail.Quantity);
                    // Tính tổng tiền
                    total += (item.Product.Price.GetValueOrDefault(0) * item.Quantity);
                }                
                string content = System.IO.File.ReadAllText(Server.MapPath("~/assets/client/Template/NewOrder.html"));
                content = content.Replace("{{CustomerName}}", shipName);
                content = content.Replace("{{Phone}}", mobile);
                content = content.Replace("{{Email}}", email);
                content = content.Replace("{{Address}}", address);
                content = content.Replace("{{Total}}", total.ToString("N0"));
                var toEmail = ConfigurationManager.AppSettings["ToEmailAddress"].ToString();
                new MailHelper().SendMail(email, "Đơn hàng mới từ OnlineShop", content);
                new MailHelper().SendMail(toEmail, "Đơn hàng mới từ OnlineShop", content);
            }
            catch (Exception)
            {

            }
            // Xoá toàn bộ giỏ hàng
            DeleteAll();
            return Redirect("/hoan-thanh");
        }
        public ActionResult Success()
        {
            return View();
        }
        // Kiểm tra số lượng tồn
        public int QuantityProduct(long id)
        {
            int num = ProductQuantity.Get(id);
            return num;
        }
    }
}
