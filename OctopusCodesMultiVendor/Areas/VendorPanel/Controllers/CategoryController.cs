using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using OctopusCodesMultiVendor.Models;
using OctopusCodesMultiVendor.Models.ViewModels;
using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace OctopusCodesMultiVendor.Areas.VendorPanel.Controllers
{
    [Area("vendorpanel")]
    [Route("vendorpanel/category")]
    public class CategoryController : Controller
    {
        private OctopusCodesMultiVendorsEntities ocmde = new OctopusCodesMultiVendorsEntities();

        [Route("index")]
        public IActionResult Index()
        {
            try
            {
                var accountVendor = ocmde.AccountVendors.SingleOrDefault(v => v.Email.Equals(HttpContext.Session.GetString("email_vendor")));
                ViewBag.categories = ocmde.Categories.Where(c => c.VendorId == accountVendor.VendorId).ToList();
                return View();
            }
            catch (Exception e)
            {
                throw;
            }
        }

        [Route("add")]
        [HttpGet]
        public IActionResult Add()
        {
            try
            {
                var accountVendor = ocmde.Vendors.SingleOrDefault(v => v.Email.Equals(HttpContext.Session.GetString("email_vendor")));
                CategoryViewModel categoryViewModel = new CategoryViewModel()
                {
                    category = new Category()
                    {
                        Status = true
                    },
                    Parent = ocmde.Categories.Where(c => c.ParentId == null).Select(c => new SelectListItem()
                    {
                        Value = c.Id.ToString(),
                        Text = c.Name
                    }).ToList()
                };
                return View("Add", categoryViewModel);
            }
            catch (Exception e)
            {
                throw;
            }
        }

        [Route("add")]
        [HttpPost]
        public IActionResult Add(CategoryViewModel categoryViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var accountVendor = ocmde.AccountVendors.SingleOrDefault(v => v.Email.Equals(HttpContext.Session.GetString("email_vendor")));
                    if (categoryViewModel.category.ParentId == -1)
                    {
                        categoryViewModel.category.ParentId = null;
                    }
                    categoryViewModel.category.VendorId = accountVendor.VendorId;
                    ocmde.Categories.Add(categoryViewModel.category);
                    ocmde.SaveChanges();
                    return RedirectToAction("Index");
                }
                return View("Add", categoryViewModel);
            }
            catch (Exception e)
            {
                throw;
            }
        }

        [Route("delete/{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                var category = ocmde.Categories.SingleOrDefault(c => c.Id == id);
                ocmde.Categories.Remove(category);
                ocmde.SaveChanges();
                return RedirectToAction("Index");
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
                var category = ocmde.Categories.SingleOrDefault(c => c.Id == id);
                category.Status = !category.Status;
                ocmde.SaveChanges();
                return RedirectToAction("Index");
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
                //var accountVendor = ocmde.AccountVendors.SingleOrDefault(v => v.Email.Equals(HttpContext.Session.GetString("email_vendor")));
                CategoryViewModel categoryViewModel = new CategoryViewModel()
                {
                    category = ocmde.Categories.SingleOrDefault(c => c.Id == id),
                    Parent = ocmde.Categories.Where(c => c.ParentId == null).Select(c => new SelectListItem()
                    {
                        Value = c.Id.ToString(),
                        Text = c.Name
                    }).ToList()
                };
                return View("Edit", categoryViewModel);
            }
            catch (Exception e)
            {
                throw;
            }
        }

        [Route("edit/{id?}")]
        [HttpPost]
        public IActionResult Edit(int id, CategoryViewModel categoryViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var currentCategory = ocmde.Categories.SingleOrDefault(c => c.Id == categoryViewModel.category.Id);
                    if (categoryViewModel.category.ParentId == -1)
                    {
                        currentCategory.ParentId = null;
                    }
                    else
                    {
                        currentCategory.ParentId = categoryViewModel.category.ParentId;
                    }
                    currentCategory.Name = categoryViewModel.category.Name;
                    currentCategory.Status = categoryViewModel.category.Status;
                    ocmde.SaveChanges();
                    return RedirectToAction("Index");
                }
                categoryViewModel.Parent = ocmde.Categories.Where(c => c.ParentId == null).Select(c => new SelectListItem()
                {
                    Value = c.Id.ToString(),
                    Text = c.Name
                }).ToList();
                return View("Edit", categoryViewModel);
            }
            catch (Exception e)
            {
                throw;
            }
        }

    }
}