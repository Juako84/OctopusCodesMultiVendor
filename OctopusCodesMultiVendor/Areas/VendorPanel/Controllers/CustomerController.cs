using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using OctopusCodesMultiVendor.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OctopusCodesMultiVendor.Areas.VendorPanel.Controllers
{
    [Area("vendorpanel")]
    [Route("vendorpanel/customer")]
    public class CustomerController : Controller
    {
        private OctopusCodesMultiVendorsEntities ocmde = new OctopusCodesMultiVendorsEntities();

        [Route("index")]
        public IActionResult Index()
        {
            try
            {
                var accountVendor = ocmde.AccountVendors.SingleOrDefault(v => v.Email.Equals(HttpContext.Session.GetString("email_vendor")));
                var customerIds = ocmde.Vendors.Find(accountVendor.VendorId).Orderss.Select(o => o.CustomerId).Distinct().ToList();
                var customers = new List<AccountCustomer>();
                customerIds.ForEach(id =>
                {
                    customers.Add(ocmde.AccountCustomer.Find(id));
                });
                ViewBag.customers = customers;
                return View();
            }
            catch (Exception e)
            {
                throw;
            }
        }
    }
}