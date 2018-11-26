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
        [Route("editCustomer/{id}")]
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

        [Route("editCustomer/{id}")]
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

        [Route("statuscustomer/{id}")]
        public IActionResult StatusCustomer(int id)
        {
            try
            {
                var customer = ocmde.Customer.SingleOrDefault(a => a.Id == id);
                customer.Status = !customer.Status;
                ocmde.SaveChanges();
                return RedirectToAction("Index", "Customer");
            }
            catch (Exception e)
            {
                throw;
            }
        }

        [Route("RelateCustomerVendor/{id}")]
        [HttpPost]
        public JsonResult RelateCustomerVendor([FromBody] dynamic id)
        {
            try
            {
                int idCustomer = Convert.ToInt32(id.idCustomer.Value);
                int idVendor = Convert.ToInt32(id.idVendor.Value);
               
                var customerVendor = ocmde.CustomerVendor.SingleOrDefault(cv => cv.CustomerId.Equals(idCustomer) && cv.VendorId.Equals(idVendor));
                if (customerVendor != null)
                {
                    //Delete relationship
                    ocmde.CustomerVendor.Remove(customerVendor);


                }
                else
                {
                    customerVendor = new CustomerVendor()
                    {
                        CustomerId = idCustomer,
                        VendorId = idVendor,
                        DateCreate = DateTime.Now

                    };
                    ocmde.CustomerVendor.Add(customerVendor);
                }

                ocmde.SaveChanges();
                return Json(new { success =true });
                // return RedirectToAction("EditCustomer", "Customer", new { id = idCustomer});
            }
            catch (Exception e)
            {
                throw;
            }
        }
        #endregion

        #region Delete

        #endregion
    }
}