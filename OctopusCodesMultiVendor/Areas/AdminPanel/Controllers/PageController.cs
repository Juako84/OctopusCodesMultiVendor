using Microsoft.AspNetCore.Mvc;
using OctopusCodesMultiVendor.Models;
using System;
using System.Linq;

namespace OctopusCodesMultiVendor.Areas.AdminPanel.Controllers
{
    [Area("adminpanel")]
    [Route("adminpanel/page")]
    public class PageController : Controller
    {
        private OctopusCodesMultiVendorsEntities ocmde = new OctopusCodesMultiVendorsEntities();

        [Route("index")]
        public IActionResult Index()
        {
            try
            {
                ViewBag.pages = ocmde.Pages.ToList();
                return View();
            }
            catch (Exception e)
            {
                throw;
            }
        }

        [Route("edit/{id}")]
        [HttpGet]
        public IActionResult Edit(int id)
        {
            try
            {
                var page = ocmde.Pages.SingleOrDefault(p => p.Id == id);
                return View("Edit", page);
            }
            catch (Exception e)
            {
                throw;
            }
        }

        [Route("edit/{id?}")]
        [HttpPost]
        public IActionResult Edit(int id, Page page)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var currentPage = ocmde.Pages.Find(page.Id
    );
                    currentPage.Plug = page.Plug;
                    currentPage.Title = page.Title;
                    currentPage.Status = page.Status;
                    currentPage.Detail = page.Detail;
                    ocmde.SaveChanges();
                    return RedirectToAction("Index");
                }
                return View("Edit", page);
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
                var page = ocmde.Pages.SingleOrDefault(p => p.Id == id);
                page.Status = !page.Status;
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