using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using OctopusCodesMultiVendor.Models;

namespace OctopusCodesMultiVendor.Areas.AdminPanel.Controllers
{
    [Area("adminpanel")]
    [Route("adminpanel/account/Customer")]
    public class CustomerController : Controller
    {
        private readonly IStringLocalizer<SharedResource> sharedLocalizer;
        private OctopusCodesMultiVendorsEntities ocmde = new OctopusCodesMultiVendorsEntities();
        public CustomerController(IStringLocalizer<SharedResource> sharedLocalizer)
        {
            this.sharedLocalizer = sharedLocalizer;
        }

        [Route("index")]
        public IActionResult Index()
        {
            try
            {
                ViewBag.customers = ocmde.Customer.OrderByDescending(o => o.Id).ToList();
                return View("Index");
            }
            catch (Exception e)
            {
                throw;
            }
        }

        [Route("addCustomer")]
        [HttpGet]
        public IActionResult AddCustumer()
        {
            try
            {
                return View("AddCustomer", new Customer());
            }
            catch (Exception e)
            {
                throw;
            }
        }

        [Route("addCustomer")]
        [HttpPost]
        public IActionResult AddCustumer(Customer custommer)
        {
            try
            {
                //Valida rut 
                if (!Utils.Rut.ValidaRut(custommer.Rut, custommer.RutDv))
                {
                    ModelState.AddModelError("rut", sharedLocalizer["Rut_incorrect"]);
                }
                //Valida que no exista
                //if (addCustomer.Email != null && account.Email.Length > 0)
                //{
                //    if (Exists(account.Email))
                //    {
                //        ModelState.AddModelError("username", sharedLocalizer["Username_already_exists"]);
                //    }
                //}


                if (ModelState.IsValid)
                {
                    //account.IsAdmin = false;
                    custommer.Status = true;
                    custommer.DateCreate = DateTime.Now;


                    ocmde.Customer.Add(custommer);
                    ocmde.SaveChanges();
                    return RedirectToAction("Index", "Customer");
                }
                else
                {
                    return View("AddCustomer", custommer);
                }
            }
            catch (Exception e)
            {
                throw;
            }
        }

        #region Edit
        [Route("editCustomer")]
        public IActionResult EditCustomer(int? id)
        {
            try
            {
                int idCustomer;

                bool success = Int32.TryParse(id.ToString(), out idCustomer);
                if (success)
                {
                    var customer = ocmde.Customer.SingleOrDefault(c => c.Id.Equals(idCustomer));
                    return View("EditCustomer", customer);
                }
                

                return RedirectToAction("Index", "Customer");
            }
            catch (Exception e)
            {
                throw;
            }
        }

        [Route("editCustomer")]
        [HttpPost]
        public IActionResult EditCustomer(Customer custommer)
        {
            try
            {
                
                if (ModelState.IsValid)
                {
                    //Valida rut 
                    if (!Utils.Rut.ValidaRut(custommer.Rut, custommer.RutDv))
                    {
                        ModelState.AddModelError("rut", sharedLocalizer["Rut_incorrect"]);
                    }
                    var currentCustomer = ocmde.Customer.SingleOrDefault(c => c.Id == custommer.Id);
                    currentCustomer.Name = custommer.Name;
                    currentCustomer.Phone = custommer.Phone;
                    currentCustomer.Address = custommer.Address;
                    currentCustomer.Code = custommer.Code;
                    currentCustomer.Status = custommer.Status;
                    currentCustomer.Rut = custommer.Rut;
                    currentCustomer.RutDv = custommer.RutDv;


                    //account.IsAdmin = false;
                    //custommer.Status = true;
                    currentCustomer.DateUpdate = DateTime.Now;


                    ocmde.SaveChanges();
                    
                    return RedirectToAction("Index", "Customer");
                }
               


                return RedirectToAction("Index", "Customer");
            }
            catch (Exception e)
            {
                throw;
            }
        }
        #endregion 
    }
}