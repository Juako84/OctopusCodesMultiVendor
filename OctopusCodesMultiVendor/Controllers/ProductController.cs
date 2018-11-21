using OctopusCodesMultiVendor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using OctopusCodesMultiVendor.Helpers;
using Microsoft.AspNetCore.Mvc;
using PagedList.Core;

namespace OctopusCodesMultiVendor.Controllers
{
    [Route("product")]
    public class ProductController : Controller
    {
        private OctopusCodesMultiVendorsEntities ocmde = new OctopusCodesMultiVendorsEntities();

        [Route("search")]
        [HttpGet]
        public IActionResult Search(int page = 1, int pageSize = 9)
        {
            try
            {
                pageSize = int.Parse(ocmde.Settings.Find(9).Value);
                string keyword = Request.Query["keyword"];
                int categoryId = int.Parse(Request.Query["category"]);
                List<Product> listProducts = ocmde.Products.Where(p => p.Name.Contains(keyword) && p.CategoryId == categoryId && p.Status).ToList();
                var products = new List<Product>();
                listProducts.ForEach(p =>
                {
                    if (VendorHelper.checkExpires(p.VendorId))
                    {
                        products.Add(p);
                    }
                });
                PagedList<Product> model = new PagedList<Product>(products.AsQueryable(), page, pageSize);
                ViewBag.keyword = keyword;
                ViewBag.categoryId = categoryId;
                return View("Search", model);
            }
            catch (Exception e)
            {
                throw;
            }
        }

        [Route("category/{id}")]
        public IActionResult Category(int id, int page = 1, int pageSize = 9)
        {
            try
            {
                pageSize = int.Parse(ocmde.Settings.Find(9).Value);
                List<Product> listProducts = ocmde.Products.Where(p => p.CategoryId == id && p.Status).ToList();
                var products = new List<Product>();
                listProducts.ForEach(p =>
                {
                    if (VendorHelper.checkExpires(p.VendorId))
                    {
                        products.Add(p);
                    }
                });
                PagedList<Product> model = new PagedList<Product>(products.AsQueryable(), page, pageSize);
                ViewBag.category = ocmde.Categories.Find(id);
                TempData["categorySelected"] = ocmde.Categories.Find(id).Parent.Id;
                return View("Category", model);
            }
            catch (Exception e)
            {
                throw;
            }
        }

        [Route("expires")]
        public IActionResult Expires()
        {
            return View("Expires");
        }

        [Route("detail/{id}")]
        public IActionResult Detail(int id)
        {
            try
            {
                var product = ocmde.Products.Find(id);
                if (!VendorHelper.checkExpires(product.VendorId))
                {
                    return RedirectToAction("Expires", "Product");
                }
                else
                {
                    product.Views = product.Views + 1;
                    ocmde.SaveChanges();
                    ViewBag.product = product;
                    ViewBag.relatedProducts = ocmde.Products.Where(p => p.Id != id && p.CategoryId == product.CategoryId && p.VendorId == product.VendorId).Take(6).ToList();
                    return View("Detail");
                }
            }
            catch (Exception e)
            {
                throw;
            }
        }
    }
}