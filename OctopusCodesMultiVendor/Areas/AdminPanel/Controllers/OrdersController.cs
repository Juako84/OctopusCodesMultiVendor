using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using OctopusCodesMultiVendor.Models;

namespace OctopusCodesMultiVendor.Areas.AdminPanel.Controllers
{
    [Area("adminpanel")]
    [Route("adminpanel/orders")]
    public class OrdersController : Controller
    {
        private OctopusCodesMultiVendorsEntities ocmde = new OctopusCodesMultiVendorsEntities();

        [Route("index")]
        public IActionResult Index()
        {
            try
            {
                ViewBag.orders = ocmde.Orderss.OrderByDescending(o => o.Id).ToList();
                return View();
            }
            catch (Exception e)
            {
                throw;
            }
        }

    }
}