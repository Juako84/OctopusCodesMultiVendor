using OctopusCodesMultiVendor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using OctopusCodesMultiVendor.Helpers;
using Microsoft.AspNetCore.Mvc;
using PagedList.Core;
using Microsoft.AspNetCore.Http;
using OctopusCodesMultiVendor.Paypal;
using Microsoft.Extensions.Localization;
using System.Threading.Tasks;

namespace OctopusCodesMultiVendor.Controllers
{
    [Route("vendors")]
    public class VendorsController : Controller
    {
        private readonly IStringLocalizer<SharedResource> sharedLocalizer;

        private OctopusCodesMultiVendorsEntities ocmde = new OctopusCodesMultiVendorsEntities();

        public VendorsController(IStringLocalizer<SharedResource> sharedLocalizer)
        {
            this.sharedLocalizer = sharedLocalizer;
        }

        [Route("expires")]
        public IActionResult Expires()
        {
            return View("Expires");
        }

        [Route("Detail/{id}")]
        public IActionResult Detail(int id, int page = 1, int pageSize = 9)
        {
            try
            {
                if (!VendorHelper.checkExpires(id))
                {
                    return RedirectToAction("Expires", "Vendors");
                }
                else
                {
                    pageSize = int.Parse(ocmde.Settings.Find(9).Value);
                    var vendor = ocmde.Vendors.Find(id);
                    List<Product> listProducts = vendor.Products.Where(p => p.Status).ToList();
                    PagedList<Product> model = new PagedList<Product>(listProducts.AsQueryable(), page, pageSize);
                    ViewBag.vendor = vendor;
                    ViewBag.comments = vendor.Reviews.Where(r => r.VendorId == id).OrderByDescending(r => r.Id).ToList();
                    return View("Index", model);
                }
            }
            catch (Exception e)
            {
                throw;
            }
        }

        [Route("SendMessage/{id?}")]
        [HttpGet]
        public IActionResult SendMessage(int id)
        {
            try
            {
                if (!VendorHelper.checkExpires(id))
                {
                    return RedirectToAction("Expires", "Vendors");
                }
                else
                {
                    if (HttpContext.Session.GetString("username_customer") == null)
                    {
                        return RedirectToAction("Index", "Login", new { Area = "CustomerPanel" });
                    }
                    else
                    {
                        ViewBag.vendor = ocmde.Vendors.Find(id);
                        return View("SendMessage", new Message() { VendorId = id });
                    }
                }
            }
            catch (Exception e)
            {
                throw;
            }
        }

        [Route("SendMessage/{id?}")]
        [HttpPost]
        public IActionResult SendMessage(int id, Message message)
        {
            try
            {
                var customer = ocmde.AccountCustomer.SingleOrDefault(a => a.Email.Equals(HttpContext.Session.GetString("email_customer")));
                var newMessage = new Message() {
                    DateCreation = DateTime.Now,
                    Status = false,
                    CustomerId = customer.Id,
                    Title = message.Title,
                    Body = message.Body,
                    VendorId = id
                };
                ocmde.Messages.Add(newMessage);
                ocmde.SaveChanges();
                ViewBag.vendor = ocmde.Vendors.Find(id);
                ViewBag.Result = sharedLocalizer["messages_sent_success"];
                return View("SendMessage", new Message() { VendorId = id });
            }
            catch (Exception e)
            {
                throw;
            }
        }

        [Route("GiveReview/{id}")]
        [HttpGet]
        public IActionResult GiveReview(int id)
        {
            try
            {
                if (!VendorHelper.checkExpires(id))
                {
                    return RedirectToAction("Expires", "Vendors");
                }
                else
                {
                    if (HttpContext.Session.GetString("email_customer") == null)
                    {
                        return RedirectToAction("Index", "Login", new { Area = "CustomerPanel" });
                    }
                    else
                    {
                        var customer = ocmde.AccountCustomer.SingleOrDefault(a => a.Email.Equals(HttpContext.Session.GetString("email_customer")));
                        if (!checkCustomerOfVendor(customer.Id, id))
                        {
                            return RedirectToAction("Index", "Login", new { Area = "CustomerPanel" });
                        }
                        else
                        {
                            ViewBag.vendor = ocmde.Vendors.Find(id);
                            return View("GiveReview", new Review() { VendorId = id });
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw;
            }
        }

        [Route("GiveReview/{id?}")]
        [HttpPost]
        public IActionResult GiveReview(int id, Review review)
        {
            try
            {
                var accountCustomer = ocmde.AccountCustomer.SingleOrDefault(a => a.Email.Equals(HttpContext.Session.GetString("email_customer")));
                var newReview = new Review() {
                    CustomerId = accountCustomer.CustomerId,
                    DatePost = DateTime.Now,
                    Detail = review.Detail,
                    VendorId = id
                };
                ocmde.Reviews.Add(newReview);
                ocmde.SaveChanges();
                ViewBag.Result = sharedLocalizer["Your_review_sent_to_vendor"];
                ViewBag.vendor = ocmde.Vendors.Find(id);
                return View("GiveReview");
            }
            catch (Exception e)
            {
                throw;
            }
        }

        private bool checkCustomerOfVendor(int customerId, int vendorId)
        {
            return ocmde.Orderss.Count(o => o.VendorId == vendorId && o.CustomerId == customerId) > 0;
        }

        [Route("Category/{id}/{vendorId}")]
        public IActionResult Category(int id, int vendorId, int page = 1, int pageSize = 9)
        {
            try
            {
                if (!VendorHelper.checkExpires(vendorId))
                {
                    return RedirectToAction("Expires", "Vendors");
                }
                else
                {
                    pageSize = int.Parse(ocmde.Settings.Find(9).Value);
                    List<Product> listProducts = ocmde.Products.Where(p => p.CategoryId == id && p.Status).ToList();
                    PagedList<Product> model = new PagedList<Product>(listProducts.AsQueryable(), page, pageSize);
                    ViewBag.category = ocmde.Categories.Find(id);
                    TempData["categoryVendorSelected"] = ocmde.Categories.Find(id).Parent.Id;
                    ViewBag.vendor = ocmde.Vendors.Find(vendorId);
                    return View("Category", model);
                }
            }
            catch (Exception e)
            {
                throw;
            }
        }

        [Route("MemberShip")]
        public IActionResult MemberShip()
        {
            try
            {
                var email_vendor = HttpContext.Session.GetString("email_vendor");
                ViewBag.vendor_logged = ocmde.Vendors.SingleOrDefault(a => a.Email.Equals(email_vendor));
                ViewBag.memberships = ocmde.MemberShips.Where(m => m.Status).ToList();
                ViewBag.PayPalSubmitUrl = ocmde.Settings.Find(11).Value;
                ViewBag.PayPalUsername = ocmde.Settings.Find(12).Value;
                ViewBag.ReturnUrl = ocmde.Settings.Find(13).Value;
                return View("MemberShip");
            }
            catch (Exception e)
            {
                throw;
            }
        }

        [Route("Register")]
        [HttpGet]
        public IActionResult Register()
        {
            try
            {
                return View("Register", new Vendor());
            }
            catch (Exception e)
            {
                throw;
            }
        }

        [Route("Register")]
        [HttpPost]
        public IActionResult Register(AccountVendor accountVendor, IFormFile logo)
        {
            try
            {
                
                if (accountVendor.Email != null && Exists(accountVendor.Email))
                {
                    ModelState.AddModelError("Username", sharedLocalizer["Username_already_exists"]);
                }

                if (accountVendor.Password != null && !PasswordHelper.IsValidPassword(accountVendor.Password))
                {
                    ModelState.AddModelError("Password", "Invalid Password");
                }

                if (logo != null && logo.Length > 0 && !logo.ContentType.Contains("image"))
                {
                    ViewBag.errorPhoto = sharedLocalizer["Photo_Invalid"];
                    return View("Register", accountVendor);
                }

                if (ModelState.IsValid)
                {
                    accountVendor.Password = BCrypt.Net.BCrypt.HashPassword(accountVendor.Password);
                    accountVendor.Status = true;
                    //if (logo != null && logo.Length > 0 && logo.ContentType.Contains("image"))
                    //{
                    //    var fileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + Path.GetFileName(logo.FileName);
                    //    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/user/images", fileName);
                    //    var stream = new FileStream(path, FileMode.Create);
                    //    logo.CopyTo(stream);
                    //    accountVendor.Logo = fileName;
                    //}
                    //else
                    //{
                    //    accountVendor.Logo = "no-logo.jpg";
                    //}
                    ocmde.AccountVendors.Add(accountVendor);

                    // Add Package Trial to new vendor
                    //var membership = ocmde.MemberShips.Find(MemberShipHelper.TrialPackage);
                    //MemberShipVendor memberShipVendor = new MemberShipVendor()
                    //{
                    //    MemerShipId = membership.Id,
                    //    VendorId = vendor.Id,
                    //    Price = membership.Price,
                    //    StartDate = DateTime.Now,
                    //    EndDate = DateTime.Now.AddMonths(membership.Month)
                    //};
                    //ocmde.MemberShipVendors.Add(memberShipVendor);
                    ocmde.SaveChanges();

                    ocmde.SaveChanges();
                    return RedirectToAction("Index", "Login", new { Area = "vendorpanel" });
                }
                return View("Register", accountVendor);
            }
            catch (Exception e)
            {
                throw;
            }
        }

        private bool Exists(string email)
        {
            try
            {
                return ocmde.AccountVendors.Count(a => a.Email.Equals(email)) > 0;
            }
            catch
            {
                return false;
            }
        }

        [Route("Success")]
        public IActionResult Success()
        {
            try
            {
                var result = PDTHolder.Success(Request.Query["tx"]);
                if (result != null)
                {
                    var membership = ocmde.MemberShips.Find(result.MemberShipId);
                    var accountVendor = ocmde.AccountVendors.SingleOrDefault(v => v.Email.Equals(HttpContext.Session.GetString("email_customer")));
                    MemberShipVendor memberShipVendor = new MemberShipVendor()
                    {
                        MemerShipId = membership.Id,
                        VendorId = accountVendor.VendorId,
                        Price = membership.Price,
                        StartDate = DateTime.Now,
                        EndDate = DateTime.Now.AddMonths(membership.Month)
                    };
                    ocmde.MemberShipVendors.Add(memberShipVendor);
                    ocmde.SaveChanges();
                    ViewBag.msg = "Success";
                }
                else
                {
                    ViewBag.msg = "Error";
                }
                return View("Success");
            }
            catch (Exception e)
            {
                throw;
            }
        }

    }
}