using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using OctopusCodesMultiVendor.Models;
using System;
using System.Linq;

namespace OctopusCodesMultiVendor.Areas.VendorPanel.Controllers
{
    [Area("vendorpanel")]
    [Route("vendorpanel/memberShip")]
    public class MemberShipController : Controller
    {
        private OctopusCodesMultiVendorsEntities ocmde = new OctopusCodesMultiVendorsEntities();

        [Route("index")]
        public IActionResult Index()
        {
            try
            {
                var accountVendor = ocmde.AccountVendors.SingleOrDefault(v => v.Email.Equals(HttpContext.Session.GetString("email_vendor")));
                ViewBag.memberShipVendors = accountVendor.Vendor.MemberShipVendors.OrderByDescending(m => m.Id);
                return View(); 
            }
            catch (Exception e)
            {
                throw;
            }
        }

    }
}