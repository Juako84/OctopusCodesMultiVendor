using System;
using System.Linq;
using OctopusCodesMultiVendor.Models.ViewModels;
using OctopusCodesMultiVendor.Models;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Localization;

namespace OctopusCodesMultiVendor.Areas.VendorPanel.Controllers
{
    [Area("vendorpanel")]
    [Route("vendorpanel/product")]
    public class ProductController : Controller
    {
        private readonly IStringLocalizer<ProductController> localizer;

        private readonly IStringLocalizer<SharedResource> sharedLocalizer;

        private OctopusCodesMultiVendorsEntities ocmde = new OctopusCodesMultiVendorsEntities();

        public ProductController(IStringLocalizer<ProductController> localizer, IStringLocalizer<SharedResource> sharedLocalizer)
        {
            this.localizer = localizer;
            this.sharedLocalizer = sharedLocalizer;
        }

        [Route("index")]
        public IActionResult Index()
        {
            try
            {
                var accountVendor = ocmde.AccountVendors.SingleOrDefault(v => v.Email.Equals(HttpContext.Session.GetString("email_vendor")));
                ViewBag.products = ocmde.Products.Where(c => c.VendorId == accountVendor.VendorId).OrderByDescending(o => o.Id).ToList();
                return View();
            }
            catch (Exception e)
            {
                throw;
            }
        }

        [Route("category/{id}")]
        public IActionResult Category(int id)
        {
            try
            {
                var accountVendor = ocmde.AccountVendors.SingleOrDefault(v => v.Email.Equals(HttpContext.Session.GetString("email_vendor")));
                ViewBag.products = ocmde.Products.Where(c => c.VendorId == accountVendor.VendorId && c.CategoryId == id).OrderByDescending(o => o.Id).ToList();
                return View("Index");
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
                var accountVendor = ocmde.AccountVendors.SingleOrDefault(v => v.Email.Equals(HttpContext.Session.GetString("email_vendor")));
                var productViewModel = new ProductViewModel();
                productViewModel.Categories = new System.Collections.Generic.List<SelectListItem>();
                var categories = ocmde.Categories.Where(c => c.ParentId == null).ToList();
                foreach (var category in categories)
                {
                    var group = new SelectListGroup { Name = category.Name };
                    foreach (var subCategory in category.InverseParents)
                    {
                        if (subCategory.VendorId == accountVendor.VendorId || (subCategory.VendorId == null && subCategory.ParentId != null))
                        {
                            var selectListItem = new SelectListItem()
                            {
                                Text = subCategory.Name,
                                Value = subCategory.Id.ToString(),
                                Group = group
                            };
                            productViewModel.Categories.Add(selectListItem);
                        }
                    }
                }

                return View("Add", productViewModel);
            }
            catch (Exception e)
            {
                throw;
            }
        }

        [Route("add")]
        [HttpPost]
        public IActionResult Add(ProductViewModel productViewModel)
        {
            try
            {
                var accountVendor = ocmde.AccountVendors.SingleOrDefault(v => v.Email.Equals(HttpContext.Session.GetString("email_vendor")));
                if (ModelState.IsValid)
                {
                    productViewModel.product.VendorId = accountVendor.Id;
                    productViewModel.product.Views = 0;
                    ocmde.Products.Add(productViewModel.product);
                    ocmde.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    productViewModel = new ProductViewModel();
                    productViewModel.Categories = new System.Collections.Generic.List<SelectListItem>();
                    var categories = ocmde.Categories.Where(c => c.ParentId == null).ToList();
                    foreach (var category in categories)
                    {
                        var group = new SelectListGroup { Name = category.Name };
                        foreach (var subCategory in category.InverseParents)
                        {
                            if (subCategory.VendorId == accountVendor.VendorId || (subCategory.VendorId == null && subCategory.ParentId != null))
                            {
                                var selectListItem = new SelectListItem()
                                {
                                    Text = subCategory.Name,
                                    Value = subCategory.Id.ToString(),
                                    Group = group
                                };
                                productViewModel.Categories.Add(selectListItem);
                            }
                        }
                    }
                    return View("Add", productViewModel);
                }
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
                var accountVendor = ocmde.AccountVendors.SingleOrDefault(v => v.Email.Equals(HttpContext.Session.GetString("email_vendor")));
                var productViewModel = new ProductViewModel();
                productViewModel.Categories = new System.Collections.Generic.List<SelectListItem>();
                var categories = ocmde.Categories.Where(c => c.ParentId == null).ToList();
                foreach (var category in categories)
                {
                    var group = new SelectListGroup { Name = category.Name };
                    foreach (var subCategory in category.InverseParents)
                    {
                        if (subCategory.VendorId == accountVendor.VendorId || (subCategory.VendorId == null && subCategory.ParentId != null))
                        {
                            var selectListItem = new SelectListItem()
                            {
                                Text = subCategory.Name,
                                Value = subCategory.Id.ToString(),
                                Group = group
                            };
                            productViewModel.Categories.Add(selectListItem);
                        }
                    }
                }
                productViewModel.product = ocmde.Products.Find(id);
                return View("Edit", productViewModel);
            }
            catch (Exception e)
            {
                throw;
            }
        }

        [Route("edit/{id?}")]
        [HttpPost]
        public IActionResult Edit(int id, ProductViewModel productViewModel)
        {
            try
            {
                var accountVendor = ocmde.AccountVendors.SingleOrDefault(v => v.Email.Equals(HttpContext.Session.GetString("email_vendor")));
                if (ModelState.IsValid)
                {
                    var currentProduct = ocmde.Products.Find(productViewModel.product.Id);
                    currentProduct.Name = productViewModel.product.Name;
                    currentProduct.Price = productViewModel.product.Price;
                    currentProduct.Quantity = productViewModel.product.Quantity;
                    currentProduct.Description = productViewModel.product.Description;
                    currentProduct.Status = productViewModel.product.Status;
                    currentProduct.CategoryId = productViewModel.product.CategoryId;
                    ocmde.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    productViewModel = new ProductViewModel();
                    productViewModel.Categories = new System.Collections.Generic.List<SelectListItem>();
                    var categories = ocmde.Categories.Where(c => c.ParentId == null).ToList();
                    foreach (var category in categories)
                    {
                        var group = new SelectListGroup { Name = category.Name };
                        foreach (var subCategory in category.InverseParents)
                        {
                            if (subCategory.VendorId == accountVendor.VendorId || (subCategory.VendorId == null && subCategory.ParentId != null))
                            {
                                var selectListItem = new SelectListItem()
                                {
                                    Text = subCategory.Name,
                                    Value = subCategory.Id.ToString(),
                                    Group = group
                                };
                                productViewModel.Categories.Add(selectListItem);
                            }
                        }
                    }
                    productViewModel.product = ocmde.Products.Find(id);
                    return View("Edit", productViewModel);
                }
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
                var product = ocmde.Products.SingleOrDefault(p => p.Id == id);
                ocmde.Products.Remove(product);
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

        [Route("statusphoto/{id}")]
        public IActionResult StatusPhoto(int id)
        {
            try
            {
                var photo = ocmde.Photos.SingleOrDefault(p => p.Id == id);
                photo.Status = !photo.Status;
                ocmde.SaveChanges();
                return RedirectToAction("Photos", "Product", new { id = photo.ProductId });
            }
            catch (Exception e)
            {
                throw;
            }
        }

        [Route("photos/{id}")]
        public IActionResult Photos(int id)
        {
            try
            {
                ViewBag.product = ocmde.Products.Find(id);
                return View("Photos");
            }
            catch (Exception e)
            {
                throw;
            }
        }

        [Route("addphoto/{id}")]
        [HttpGet]
        public IActionResult AddPhoto(int id)
        {
            try
            {
                var photo = new Photo()
                {
                    ProductId = id
                };
                ViewBag.product = ocmde.Products.Find(id);
                return View("AddPhoto", photo);
            }
            catch (Exception e)
            {
                throw;
            }
        }

        [Route("addphoto/{id?}")]
        [HttpPost]
        public IActionResult AddPhoto(int id, Photo photo, IFormFile name)
        {
            try
            {
                if (name != null && name.Length > 0 && !name.ContentType.Contains("image"))
                {
                    ViewBag.errorPhoto = sharedLocalizer["Photo_Invalid"];
                    return View("AddPhoto", photo);
                }
                if (ModelState.IsValid)
                {
                    if (name != null && name.Length > 0 && name.ContentType.Contains("image"))
                    {
                        var fileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + Path.GetFileName(name.FileName);
                        var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/user/images", fileName);
                        var stream = new FileStream(path, FileMode.Create);
                        name.CopyTo(stream);
                        photo.Name = fileName;
                    }
                    photo.Main = false;
                    var newPhoto = new Photo() {
                        Name = photo.Name,
                        Main = photo.Main,
                        Status = photo.Status,
                        ProductId = photo.ProductId
                    };
                    ocmde.Photos.Add(newPhoto);
                    ocmde.SaveChanges();
                    return RedirectToAction("Photos", "Product", new { id = photo.ProductId });
                }
                return View("AddPhoto", photo);
            }
            catch (Exception e)
            {
                throw;
            }
        }

        [Route("deletephoto/{id}")]
        public IActionResult DeletePhoto(int id)
        {
            try
            {
                var photo = ocmde.Photos.Find(id);
                var productId = photo.ProductId;
                ocmde.Photos.Remove(photo);
                ocmde.SaveChanges();
                return RedirectToAction("Photos", "Product", new { id = productId });
            }
            catch (Exception e)
            {
                throw;
            }
        }

        [Route("editphoto/{id}")]
        [HttpGet]
        public IActionResult EditPhoto(int id)
        {
            try
            {
                var photo = ocmde.Photos.Find(id);
                ViewBag.product = photo.Product;
                return View("EditPhoto", photo);
            }
            catch (Exception e)
            {
                throw;
            }
        }

        [Route("editphoto/{id?}")]
        [HttpPost]
        public IActionResult EditPhoto(int id, Photo photo, IFormFile name)
        {
            try
            {
                if (name != null && name.Length > 0 && !name.ContentType.Contains("image"))
                {
                    ViewBag.errorPhoto = sharedLocalizer["Photo_Invalid"];
                    return View("Profile", photo);
                }
                if (ModelState.IsValid)
                {
                    var currentPhoto = ocmde.Photos.Find(photo.Id);
                    if (name != null && name.Length > 0 && name.ContentType.Contains("image"))
                    {
                        var fileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + Path.GetFileName(name.FileName);
                        var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/user/images", fileName);
                        var stream = new FileStream(path, FileMode.Create);
                        name.CopyTo(stream);
                        currentPhoto.Name = fileName;
                    }
                    currentPhoto.Main = photo.Main;
                    currentPhoto.Status = photo.Status;
                    ocmde.SaveChanges();
                    return RedirectToAction("Photos", "Product", new { id = photo.ProductId });
                }
                return View("EditPhoto", photo);
            }
            catch (Exception e)
            {
                throw;
            }
        }

        [Route("mainphoto/{id}")]
        public IActionResult MainPhoto(int id)
        {
            try
            {
                var product = ocmde.Photos.Find(id).Product;
                product.Photos.ToList().ForEach(p =>
                {
                    var photo = ocmde.Photos.Find(p.Id);
                    photo.Main = false;
                    ocmde.SaveChanges();
                });
                var mainPhoto = ocmde.Photos.Find(id);
                mainPhoto.Main = true;
                ocmde.SaveChanges();
                return RedirectToAction("Photos", "Product", new { id = product.Id });
            }
            catch (Exception e)
            {
                throw;
            }
        }

    }
}