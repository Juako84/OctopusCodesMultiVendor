using OctopusCodesMultiVendor.Models;
using System;
using System.Linq;
using OctopusCodesMultiVendor.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace OctopusCodesMultiVendor.Areas.AdminPanel.Controllers
{
    [Area("adminpanel")]
    [Route("adminpanel/category")]
    public class CategoryController : Controller
    {
        private OctopusCodesMultiVendorsEntities ocmde = new OctopusCodesMultiVendorsEntities();

        [Route("index")]
        public IActionResult Index()
        {
            try
            {
                ViewBag.categories = ocmde.Categories.Where(c => c.ParentId == null).ToList();
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
                categoryViewModel.Parent.Insert(0, new SelectListItem() { Value = "-1", Text = "Root" });
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
                    if (categoryViewModel.category.ParentId == -1)
                    {
                        categoryViewModel.category.ParentId = null;
                    }
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
                CategoryViewModel categoryViewModel = new CategoryViewModel()
                {
                    category = ocmde.Categories.SingleOrDefault(c => c.Id == id),
                    Parent = ocmde.Categories.Where(c => c.ParentId == null).Select(c => new SelectListItem()
                    {
                        Value = c.Id.ToString(),
                        Text = c.Name
                    }).ToList()
                };
                categoryViewModel.Parent.Insert(0, new SelectListItem() { Value = "-1", Text = "Root" });
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
                return View("Edit", categoryViewModel);
            }
            catch (Exception e)
            {
                throw;
            }
        }

    }
}