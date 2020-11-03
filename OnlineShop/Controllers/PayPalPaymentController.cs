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
            return RedirectToAction("Success", "PayPalPayment");
        }
        private Payment CreatePayment(APIContext apiContext, string redirectUrl)
        {
            var itemList = new ItemList() { items = new List<Item>() };
            itemList.items.Add(new Item()
            {
                name = "item Name",
                currency = "USD",
                price = "5",
                quantity = "1",
                sku = "sku"
            });
            var payer = new Payer() { payment_method = "paypal" };
            var redirUrls = new RedirectUrls()
            {
                cancel_url = redirectUrl,
                return_url = redirectUrl
            };
            var details = new Details()
            {
                tax = "1",
                shipping = "1",
                subtotal = "5"
            };
            var amount = new Amount()
            {
                currency = "USD",
                total = "7",
                details = details
            };
            var transactionList = new List<Transaction>();
            transactionList.Add(new Transaction()
            {
                description = "Transaction description.",
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