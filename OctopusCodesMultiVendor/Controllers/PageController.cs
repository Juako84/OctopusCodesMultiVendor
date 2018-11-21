using Microsoft.AspNetCore.Mvc;
using OctopusCodesMultiVendor.Models;
using System;
using System.Linq;

namespace OctopusCodesMultiVendor.Controllers
{
    [Route("page")]
    public class PageController : Controller
    {
        private OctopusCodesMultiVendorsEntities ocmde = new OctopusCodesMultiVendorsEntities();

        [Route("detail/{id}")]
        [HttpGet]
        public IActionResult Detail(string id)
        {
            try
            {
                ViewBag.page = ocmde.Pages.SingleOrDefault(p => p.Plug.Equals(id));
                return View("Index");
            }
            catch (Exception e)
            {
                throw;
            }
        }

        
    }
}