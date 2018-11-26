using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using OctopusCodesMultiVendor.Helpers;
using OctopusCodesMultiVendor.Models;
using System;
using System.Linq;

namespace OctopusCodesMultiVendor.Areas.CustomerPanel.Controllers
{
    [Area("customerpanel")]
    [Route("customerpanel/login")]
    public class LoginController : Controller
    {
        private readonly IStringLocalizer<SharedResource> sharedLocalizer;

        private OctopusCodesMultiVendorsEntities ocmde = new OctopusCodesMultiVendorsEntities();

        public LoginController(IStringLocalizer<SharedResource> sharedLocalizer)
        {
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
                var account = login(email, password);
                if (account == null)
                {
                    ViewBag.error = sharedLocalizer["Invalid_Account"];
                    return View("Index");
                }
                else
                {
                    HttpContext.Session.SetString("email_customer", account.Email);
                    return RedirectToAction("Index", "Orders");
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
                HttpContext.Session.Remove("email_customer");
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
                var account = ocmde.AccountCustomer.SingleOrDefault(a => a.Email.Equals(HttpContext.Session.GetString("email_customer")));
                return View("Profile", account);
            }
            catch (Exception e)
            {
                throw;
            }
        }

        [Route("profile")]
        [HttpPost]
        public IActionResult Profile(AccountCustomer account)
        {
            try
            {
                var loginedAccount = ocmde.AccountCustomer.SingleOrDefault(a => a.Email.Equals(HttpContext.Session.GetString("email_customer")));

                var currentAccount = ocmde.AccountCustomer.SingleOrDefault(a => a.Id == account.Id);

                if (account.Email != null && account.Email.Length > 0 && loginedAccount.Email != account.Email)
                {
                    if (Exists(account.Email))
                    {
                        ModelState.AddModelError("username", sharedLocalizer["Username_already_exists"]);
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
                    //currentAccount.Username = account.Username;
                    ocmde.SaveChanges();
                    HttpContext.Session.SetString("email_customer", account.Email);
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

        private bool Exists(string email)
        {
            return ocmde.AccountCustomer.Count(a => a.Email.Equals(email)) > 0;
        }

        private AccountCustomer login(string email, string password)
        {
            try
            {
                var account = ocmde.AccountCustomer.SingleOrDefault(a => a.Email.Equals(email));
                if (account != null)
                {
                    if (BCrypt.Net.BCrypt.Verify(password, account.Password) && account.Status)
                    {
                        return account;
                    }
                }
                return null;
            }
            catch(Exception e)
            {
                return null;
            }
        }
    }
}