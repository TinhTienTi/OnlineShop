using OnlineShop.Models;
using Models.DAO;
using System;
using System.Collections.Generic;

using System.Web.Mvc;
using Models.EF;
using System.Configuration;
using Common;
using OnlineShop.Helper;
using PayPal.Api;
using Order = Models.EF.Order;
using Configuration = OnlineShop.Helper.Configuration;

namespace OnlineShop.Controllers
{
    public class PayPalPaymentController : Controller
    {
        private Payment payment;

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Success()
        {
            return View();
        }
        public ActionResult PaymentWithPaypal()
        {
            APIContext apiContext = Configuration.GetAPIContext();
            try
            {
                string payerID = Request.Params["PayerID"];
                if (string.IsNullOrEmpty(payerID))
                {
                    string baseURI = Request.Url.Scheme + "://" + Request.Url.Authority + "/PayPalPayment/PaymentWithPayPal?";
                    var guid = Convert.ToString((new Random()).Next(100000));
                    var createdPayment = this.CreatePayment(apiContext, baseURI + "guid=" + guid);
                    // Insert vào DB
                    var order = new Order();
                    order.CreatedDate = DateTime.Now;
                    order.ShipAddress = "TP HCM";
                    order.ShipMobile = "0939 536 236";
                    order.ShipName = "Hutech";
                    order.ShipEmail = "Hutechxxx@Tpxxx@gmail.com";
                    order.Status = 0;
                    order.TypePayMent = "Thanh Toán Online";
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
                    }
                    catch
                    {

                    }
                    var links = createdPayment.links.GetEnumerator();
                    string paypalRedirectUrl = null;
                    while (links.MoveNext())
                    {
                        Links lnk = links.Current;
                        if (lnk.rel.ToLower().Trim().Equals("approval_url"))
                        {
                            paypalRedirectUrl = lnk.href;
                        }
                    }
                    Session.Add(guid, createdPayment.id);
                    return Redirect(paypalRedirectUrl);
                }
                else
                {
                    var guid = Request.Params["guid"];
                    var executedPayment = ExecutePayment(apiContext, payerID, Session[guid] as string);
                    if (executedPayment.state.ToLower() != "approved")
                    {
                        return RedirectToAction("Index", "PayPalPayment");
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Log("Error" + ex.Message);
                return RedirectToAction("Index", "PayPalPayment");
            }
            Session[OnlineShop.Common.CommonConstants.CartSession] = null;
            return RedirectToAction("Success", "PayPalPayment");
        }

        private Payment CreatePayment(APIContext apiContext, string redirectUrl)
        {
            // gọi cart
            var cart = (List<CartItem>)Session[OnlineShop.Common.CommonConstants.CartSession];
            // khởi tạo itemList để thanh toán
            var itemList = new ItemList() { items = new List<Item>() };
            double prProduct = 0;
            foreach (var item in cart)
            {
                // khoi tạo tong tien cho san pham
                prProduct = Math.Round(Convert.ToDouble((item.Product.PromotionPrice == null ? item.Product.Price : item.Product.PromotionPrice) / 23450), 0);
                // Them vao List
                itemList.items.Add(new Item()
                {
                    name = item.Product.Name,
                    currency = "USD",
                    price = prProduct.ToString(),
                    quantity = "1",
                    sku = item.Product.Code
                });
            }

            var payer = new Payer() { payment_method = "paypal" };
            var redirUrls = new RedirectUrls()
            {
                cancel_url = redirectUrl,
                return_url = redirectUrl
            };

            double subtotal = 0;
            foreach (var item in cart)
            {
                subtotal += Math.Round(Convert.ToDouble((item.Product.PromotionPrice == null ? item.Product.Price : item.Product.PromotionPrice) / 23450), 0);
            }
            var details = new Details()
            {
                tax = "100",
                shipping = "100",
                subtotal = subtotal.ToString()
            };
            double total = subtotal + 200;
            var amount = new Amount()
            {
                currency = "USD",
                total = total.ToString(),
                details = details
            };
            var transactionList = new List<Transaction>();
            transactionList.Add(new Transaction()
            {
                description = "New Bill",
                invoice_number = Guid.NewGuid().ToString(),
                amount = amount,
                item_list = itemList
            });
            this.payment = new Payment()
            {
                intent = "sale",
                payer = payer,
                transactions = transactionList,
                redirect_urls = redirUrls
            };
            return this.payment.Create(apiContext);
        }
        private Payment ExecutePayment(APIContext apiContext, string payerId, string paymentId)
        {
            var paymentExecution = new PaymentExecution() { payer_id = payerId };
            this.payment = new Payment() { id = paymentId };
            var result = payment.Execute(apiContext, paymentExecution);
            return result;
        }
    }
}