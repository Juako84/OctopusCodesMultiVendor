using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using OctopusCodesMultiVendor.Models;
using System;
using System.Linq;

namespace OctopusCodesMultiVendor.Areas.VendorPanel.Controllers
{
    [Area("vendorpanel")]
    [Route("vendorpanel/message")]
    public class MessageController : Controller
    {
        private OctopusCodesMultiVendorsEntities ocmde = new OctopusCodesMultiVendorsEntities();

        [Route("index")]
        public IActionResult Index()
        {
            try
            {
                var accountVendor = ocmde.AccountVendors.SingleOrDefault(v => v.Email.Equals(HttpContext.Session.GetString("email_vendor")));
                ViewBag.messages = ocmde.Messages.Where(m => m.VendorId == accountVendor.VendorId).OrderByDescending(m => m.Id).ToList();
                return View();
            }
            catch (Exception e)
            {
                throw;
            }
        }



    }
}