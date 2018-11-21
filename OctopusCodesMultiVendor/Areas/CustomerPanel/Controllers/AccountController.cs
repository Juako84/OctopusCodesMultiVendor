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
    [Route("customerpanel/Account")]
    public class AccountController : Controller
    {
        //public IActionResult Index()
        //{
        //    return View();
        private readonly IStringLocalizer<SharedResource> sharedLocalizer;

        private OctopusCodesMultiVendorsEntities ocmde = new OctopusCodesMultiVendorsEntities();

       

        [Route("register")]
        [HttpPost]
        public IActionResult Register(AccountCustomer account)
        {
            try
            {
                if (account.Email != null && account.Email.Length > 0)
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
                    //account.IsAdmin = false;
                    account.Status = true;
                    account.Password = BCrypt.Net.BCrypt.HashPassword(account.Password);
                    ocmde.AccountCustomer.Add(account);
                    ocmde.SaveChanges();
                    return RedirectToAction("Index", "Login", new { Area = "customerpanel" });
                }
                else
                {
                    return View("Register", account);
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
    }
}