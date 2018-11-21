using System;
using System.Linq;
using OctopusCodesMultiVendor.Models;
using System.IO;
using OctopusCodesMultiVendor.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;

namespace OctopusCodesMultiVendor.Areas.VendorPanel.Controllers
{
    [Area("vendorpanel")]
    [Route("vendorpanel/login")]
    public class LoginController : Controller
    {
        private readonly IStringLocalizer<LoginController> localizer;

        private readonly IStringLocalizer<SharedResource> sharedLocalizer;

        private OctopusCodesMultiVendorsEntities ocmde = new OctopusCodesMultiVendorsEntities();

        public LoginController(IStringLocalizer<LoginController> localizer, IStringLocalizer<SharedResource> sharedLocalizer)
        {
            this.localizer = localizer;
            this.sharedLocalizer = sharedLocalizer;
        }

        [Route("index")]
        public IActionResult Index()
        {
            return View();
        }

        [Route("process")]
        [HttpPost]
        public IActionResult Process(IFormCollection fc)
        {
            try
            {
                string email = fc["email"];
                string password = fc["password"];
                var vendor = login(email, password);
                if (vendor == null)
                {
                    ViewBag.error = sharedLocalizer["Invalid_Account"];
                    return View("Index");
                }
                else
                {
                    HttpContext.Session.SetString("email_vendor", email);
                    return RedirectToAction("Index", "Category");
                }
            }
            catch (Exception e)
            {
                throw;
            }
        }

        [Route("signout")]
        public IActionResult SignOut()
        {
            try
            {
                HttpContext.Session.Remove("email_vendor");
                return RedirectToAction("Index", "Login");
            }
            catch (Exception e)
            {
                throw;
            }
        }

        [Route("profile")]
        [HttpGet]
        public IActionResult Profile()
        {
            try
            {
                var AccountVendor = ocmde.AccountVendors.SingleOrDefault(v => v.Email.Equals(HttpContext.Session.GetString("email_vendor")));
                return View("Profile", AccountVendor);
            }
            catch (Exception e)
            {
                throw;
            }
        }

        [Route("profile")]
        [HttpPost]
        public IActionResult Profile(OctopusCodesMultiVendor.Models.AccountVendor AccountVendor, IFormFile logo)
        {
            try
            {
                var loginedAccount = ocmde.AccountVendors.SingleOrDefault(v => v.Email.Equals(HttpContext.Session.GetString("email_vendor")));

                var currentVendor = ocmde.AccountVendors.SingleOrDefault(a => a.Id == AccountVendor.Id);

                if (AccountVendor.Email != null && AccountVendor.Email.Length > 0 && loginedAccount.Email != AccountVendor.Email)
                {
                    if (Exists(AccountVendor.Email))
                    {
                        ModelState.AddModelError("username", sharedLocalizer["Username_already_exists"]);
                    }
                }

                if (AccountVendor.Password != null && AccountVendor.Password.Length != 0 && !PasswordHelper.IsValidPassword(AccountVendor.Password))
                {
                    ModelState.AddModelError("Password", sharedLocalizer["Password_validate_message"]);
                }

                if (logo != null && logo.Length > 0 && !logo.ContentType.Contains("image"))
                {
                    ViewBag.errorPhoto = sharedLocalizer["Photo_Invalid"];
                    return View("Profile", loginedAccount);
                }

                if (ModelState.IsValid)
                {
                    if (AccountVendor.Password != null && AccountVendor.Password.Length != 0)
                    {
                        currentVendor.Password = BCrypt.Net.BCrypt.HashPassword(AccountVendor.Password);
                    }

                    //if (logo != null && logo.Length > 0 && logo.ContentType.Contains("image"))
                    //{
                    //    var fileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + Path.GetFileName(logo.FileName);
                    //    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/user/images", fileName);
                    //    var stream = new FileStream(path, FileMode.Create);
                    //    currentVendor.Logo = Path.GetFileName(logo.FileName);
                    //}
                    currentVendor.Email = AccountVendor.Email;
                    currentVendor.FullName = AccountVendor.FullName;
                    currentVendor.Phone = AccountVendor.Phone;
                    currentVendor.Address = AccountVendor.Address;
                    //currentVendor.Username = AccountVendor.Username;
                    ocmde.SaveChanges();
                    HttpContext.Session.SetString("email_vendor", AccountVendor.Email);
                    return RedirectToAction("Profile", "Login");
                }
                else
                {
                    return View("Profile", AccountVendor);
                }
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
                return ocmde.Vendors.Count(a => a.Email.Equals(email)) > 0;
            }
            catch
            {
                return false;
            }
        }

        private OctopusCodesMultiVendor.Models.AccountVendor login(string email, string password)
        {
            try
            {
                var AccountVendor = ocmde.AccountVendors.SingleOrDefault(v => v.Email.Equals(email));
                if (AccountVendor != null)
                {
                    if (BCrypt.Net.BCrypt.Verify(password, AccountVendor.Password) && AccountVendor.Status && VendorHelper.checkExpires(AccountVendor.Id))
                    {
                        return AccountVendor;
                    }
                }
                return null;
            }
            catch
            {
                return null;
            }
        }
    }
}