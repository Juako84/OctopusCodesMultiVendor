using Microsoft.AspNetCore.Mvc;
using OctopusCodesMultiVendor.Models;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace OctopusCodesMultiVendor.ViewComponents
{
    [ViewComponent(Name = "MenuTop")]
    public class MenuTopViewComponent : ViewComponent
    {
        private OctopusCodesMultiVendorsEntities ocmde = new OctopusCodesMultiVendorsEntities();

        private readonly IHttpContextAccessor _contextAccessor;

        public MenuTopViewComponent(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            // Get Customer Info
            var email_customer = _contextAccessor.HttpContext.Session.GetString("email_customer");
            if (email_customer != null)
            {
                ViewBag.customer_logged = ocmde.AccountCustomer.SingleOrDefault(a => a.Email.Equals(email_customer));
            }

            // Get Vendor Info
            var email_vendor = _contextAccessor.HttpContext.Session.GetString("email_vendor");
            if (email_vendor != null)
            {
                ViewBag.vendor_logged = ocmde.Vendors.SingleOrDefault(a => a.Email.Equals(email_vendor));
            }
            return View("Index");
        }


    }
}
