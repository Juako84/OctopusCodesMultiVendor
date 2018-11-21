using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using OctopusCodesMultiVendor.Models;
using System;
using System.Linq;

namespace OctopusCodesMultiVendor.Areas.VendorPanel.Controllers
{
    [Area("vendorpanel")]
    [Route("vendorpanel/orders")]
    public class OrdersController : Controller
    {
        private OctopusCodesMultiVendorsEntities ocmde = new OctopusCodesMultiVendorsEntities();

        [Route("index")]
        public IActionResult Index()
        {
            try
            {
                var accountVendor = ocmde.AccountVendors.SingleOrDefault(v => v.Email.Equals(HttpContext.Session.GetString("email_vendor")));
                ViewBag.orders = ocmde.Orderss.Where(o => o.VendorId == accountVendor.VendorId).OrderByDescending(o => o.Id).ToList();
                return View();
            }
            catch (Exception e)
            {
                throw;
            }
        }

        [Route("detail/{id}")]
        public IActionResult Detail(int id)
        {
            try
            {
                ViewBag.order = ocmde.Orderss.Find(id);
                ViewBag.orderId = id;
                ViewBag.payments = ocmde.Payments.Where(p => p.Status).ToList();
                ViewBag.orderStatus = ocmde.OrderStatuss.Where(os => os.Status).ToList();
                return View("Detail");
            }
            catch (Exception e)
            {
                throw;
            }
        }

        [Route("customer/{id}")]
        public IActionResult Customer(int id)
        {
            try
            {
                var accountVendor = ocmde.AccountVendors.SingleOrDefault(v => v.Email.Equals(HttpContext.Session.GetString("email_vendor")));
                ViewBag.orders = ocmde.Orderss.Where(o => o.VendorId == accountVendor.VendorId && o.CustomerId == id).OrderByDescending(o => o.Id).ToList();
                return View("Index");
            }
            catch (Exception e)
            {
                throw;
            }
        }

        [Route("update")]
        [HttpPost]
        public IActionResult Update(IFormCollection fc)
        {
            int id = int.Parse(fc["id"]);
            int paymentId = int.Parse(fc["payment"]);
            int orderStatusId = int.Parse(fc["orderStatus"]);
            var order = ocmde.Orderss.Find(id);
            order.PaymentId = paymentId;
            order.OrderStatusId = orderStatusId;
            ocmde.SaveChanges();
            return RedirectToAction("Index");
        }

    }
}