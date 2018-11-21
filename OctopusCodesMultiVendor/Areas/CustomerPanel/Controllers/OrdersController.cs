using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using OctopusCodesMultiVendor.Models;
using System;
using System.Linq;

namespace OctopusCodesMultiVendor.Areas.CustomerPanel.Controllers
{
    [Area("customerpanel")]
    [Route("customerpanel/orders")]
    public class OrdersController : Controller
    {
        private OctopusCodesMultiVendorsEntities ocmde = new OctopusCodesMultiVendorsEntities();

        [Route("index")]
        public IActionResult Index()
        {
            try
            {
                var customer = ocmde.AccountCustomer.SingleOrDefault(a => a.Email.Equals(HttpContext.Session.GetString("email_customer")));
                ViewBag.orders = ocmde.Orderss.Where(o => o.CustomerId == customer.Id).OrderByDescending(o => o.Id).ToList();
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
                return View("Detail");
            }
            catch (Exception e)
            {
                throw;
            }
        }

    }
}