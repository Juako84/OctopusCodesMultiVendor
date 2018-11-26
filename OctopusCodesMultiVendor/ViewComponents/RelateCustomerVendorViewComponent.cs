using Microsoft.AspNetCore.Mvc;
using OctopusCodesMultiVendor.Models;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace OctopusCodesMultiVendor.ViewComponents
{
    [ViewComponent(Name = "RelateCustomerVendor")]
    public class RelateCustomerVendorViewComponent : ViewComponent
    {
        private OctopusCodesMultiVendorsEntities ocmde = new OctopusCodesMultiVendorsEntities();
        public async Task<IViewComponentResult> InvokeAsync(int customerId)
        {

            var vendorsAvaible = ocmde.Vendors.Where(v => v.Status).ToList();            
            var vendorRelated = ocmde.CustomerVendor.Where(c => c.CustomerId.Equals(customerId)).Select(x => x.Vendor).ToList();

            ViewBag.VendorAvailable = vendorsAvaible.Except(vendorRelated).ToList();
            ViewBag.VendorRelated = vendorRelated;
            ViewBag.CustomerId = customerId;

            return View("Index");
        }
    }
}
