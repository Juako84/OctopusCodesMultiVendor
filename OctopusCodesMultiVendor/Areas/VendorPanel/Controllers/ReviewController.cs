using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using OctopusCodesMultiVendor.Models;
using System;
using System.Linq;

namespace OctopusCodesMultiVendor.Areas.VendorPanel.Controllers
{
    [Area("vendorpanel")]
    [Route("vendorpanel/review")]
    public class ReviewController : Controller
    {
        private OctopusCodesMultiVendorsEntities ocmde = new OctopusCodesMultiVendorsEntities();

        [Route("index")]
        public IActionResult Index()
        {
            try
            {
                var accountVendor = ocmde.AccountVendors.SingleOrDefault(v => v.Email.Equals(HttpContext.Session.GetString("email_vendor")));
                ViewBag.reviews = ocmde.Reviews.Where(r => r.VendorId == accountVendor.VendorId).OrderByDescending(r => r.Id).ToList();
                return View();
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public IActionResult Customer(int id)
        {
            try
            {
                var accountVendor = ocmde.AccountVendors.SingleOrDefault(v => v.Email.Equals(HttpContext.Session.GetString("email_vendor")));
                ViewBag.reviews = ocmde.Reviews.Where(r => r.VendorId == accountVendor.VendorId && r.CustomerId == id).OrderByDescending(r => r.Id).ToList();
                return View("Index");
            }
            catch (Exception e)
            {
                throw;
            }
        }

    }
}