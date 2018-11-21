using System;
using System.Linq;
using OctopusCodesMultiVendor.Models;
using OctopusCodesMultiVendor.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;

namespace OctopusCodesMultiVendor.Areas.AdminPanel.Controllers
{
    [Area("adminpanel")]
    [Route("adminpanel/login")]
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
                string username = fc["username"];
                string password = fc["password"];
                var account = login(username, password);
                if (account == null)
                {
                    ViewBag.error = sharedLocalizer["Invalid_Account"];
                    return View("Index");
                }
                else
                {
                    HttpContext.Session.SetString("username_admin", account.Username);
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
                HttpContext.Session.Remove("username_admin");
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
                var account = ocmde.Users.Single(a => a.Username.Equals(HttpContext.Session.GetString("username_admin")));
                return View("Profile", account);
            }
            catch (Exception e)
            {
                throw;
            }
        }

        [Route("profile")]
        [HttpPost]
        public IActionResult Profile(Users account)
        {
            try
            {
                var loginedAccount = ocmde.Users.Single(a => a.Username.Equals(HttpContext.Session.GetString("username_admin")));

                var currentAccount = ocmde.Users.SingleOrDefault(a => a.Id == account.Id);

                if (account.Username != null && account.Username.Length > 0 && loginedAccount.Username != account.Username)
                {
                    if (Exists(account.Username))
                    {
                        ModelState.AddModelError("Username", sharedLocalizer["Username_already_exists"]);
                    }
                }

                if (account.Password != null && account.Password.Length != 0 && !PasswordHelper.IsValidPassword(account.Password))
                {
                    ModelState.AddModelError("Password", sharedLocalizer["Password_validate_message"]);
                }

                if (ModelState.IsValid)
                {
                    if (account.Password != null && account.Password.Length != 0)
                    {
                        currentAccount.Password = BCrypt.Net.BCrypt.HashPassword(account.Password);
                    }
                    currentAccount.Email = account.Email;
                    currentAccount.FullName = account.FullName;
                    currentAccount.Phone = account.Phone;
                    currentAccount.Username = account.Username;
                    ocmde.SaveChanges();
                    HttpContext.Session.SetString("username_admin", account.Username);
                    TempData["msg"] = sharedLocalizer["Update_Success"];
                    return RedirectToAction("Profile", "Login");
                }
                else
                {
                    return View("Profile", account);
                }
            }
            catch (Exception e)
            {
                throw;
            }
        }

        private bool Exists(string username)
        {
            return ocmde.Users.Count(a => a.Username.Equals(username)) > 0;
        }

        private Users login(string username, string password)
        {
            try
            {
                var account = ocmde.Users.SingleOrDefault(a => a.Username.Equals(username));
                if (account != null)
                {
                    if (BCrypt.Net.BCrypt.Verify(password, account.Password))
                    {
                        return account;
                    }
                }
                return null;
            }
            catch (Exception e)
            {
                return null;
            }
        }

    }
}