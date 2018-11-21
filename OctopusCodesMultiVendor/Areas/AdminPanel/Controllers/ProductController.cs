using System;
using System.Linq;
using OctopusCodesMultiVendor.Models;
using Microsoft.AspNetCore.Mvc;

namespace OctopusCodesMultiVendor.Areas.AdminPanel.Controllers
{
    [Area("adminpanel")]
    [Route("adminpanel/product")]
    public class ProductController : Controller
    {
        private OctopusCodesMultiVendorsEntities ocmde = new OctopusCodesMultiVendorsEntities();

        [Route("index")]
        public IActionResult Index()
        {
            try
            {
                ViewBag.products = ocmde.Products.OrderByDescending(o => o.Id).ToList();
                return View();
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
                var product = ocmde.Products.SingleOrDefault(p => p.Id == id);
                product.Status = !product.Status;
                ocmde.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                throw;
            }
        }

    }
}