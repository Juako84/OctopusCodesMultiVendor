using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using OctopusCodesMultiVendor.Models;

namespace OctopusCodesMultiVendor.Areas.AdminPanel.Controllers
{
    [Area("adminpanel")]
    [Route("adminpanel/account")]
    public class AccountController : Controller
    {
        private readonly IStringLocalizer<SharedResource> sharedLocalizer;
        private OctopusCodesMultiVendorsEntities ocmde = new OctopusCodesMultiVendorsEntities();

        public AccountController(IStringLocalizer<SharedResource> sharedLocalizer)
        {
            this.sharedLocalizer = sharedLocalizer;
        }
       

      

        [Route("users")]
        public IActionResult Users()
        {
            try
            {
                ViewBag.Users = ocmde.Users.OrderByDescending(o => o.Id).ToList();
                return View("Users");
            }
            catch (Exception e)
            {
                throw;
            }
        }

       

        [Route("membership/{id}")]
        public IActionResult MemberShip(int id)
        {
            try
            {
                ViewBag.memberShipVendors = ocmde.MemberShipVendors.Where(o => o.VendorId == id).OrderByDescending(m => m.Id);
                return View("MemberShip");
            }
            catch (Exception e)
            {
                throw;
            }
        }

        [Route("status/{id}")]
        public IActionResult Status(int id)
        {
            try
            {
                var customer = ocmde.AccountCustomer.SingleOrDefault(a => a.Id == id);
                customer.Status = !customer.Status;
                ocmde.SaveChanges();
                return RedirectToAction("Customer", "Account");
            }
            catch (Exception e)
            {
                throw;
            }
        }

        [Route("statusvendor/{id}")]
        public IActionResult StatusVendor(int id)
        {
            try
            {
                var vendor = ocmde.Vendors.SingleOrDefault(a => a.Id == id);
                vendor.Status = !vendor.Status;
                ocmde.SaveChanges();
                return RedirectToAction("Vendor", "Account");
            }
            catch (Exception e)
            {
                throw;
            }
        }

    }
}