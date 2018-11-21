using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OctopusCodesMultiVendor.Models;

namespace OctopusCodesMultiVendor.Areas.AdminPanel.Controllers.Account
{
    [Area("adminpanel")]
    [Route("adminpanel/account/vendor/")]
    public class VendorController : Controller
    {
        private OctopusCodesMultiVendorsEntities ocmde = new OctopusCodesMultiVendorsEntities();

        [Route("index")]
        public IActionResult Index()
        {
            try
            {
                ViewBag.vendors = ocmde.Vendors.OrderByDescending(o => o.Id).ToList();
                return View();
            }
            catch (Exception e)
            {
                throw;
            }
        }

        [Route("Edit/{id}")]
        
        public IActionResult Edit(int id)
        {
            try
            {
                var vendor = ocmde.Vendors.SingleOrDefault(v => v.Id.Equals(id));
                return View("Edit", vendor);
            }
            catch (Exception e)
            {
                throw;
            }
        }
    }
}