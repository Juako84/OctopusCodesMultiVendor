using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using OctopusCodesMultiVendor.Helpers;
using OctopusCodesMultiVendor.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace OctopusCodesMultiVendor.ViewComponents
{
    [ViewComponent(Name = "LastVendors")]
    public class LastVendorsViewComponent : ViewComponent
    {
        private OctopusCodesMultiVendorsEntities ocmde = new OctopusCodesMultiVendorsEntities();

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var lastVendors = new List<Vendor>();
            ocmde.Vendors.Where(v => v.Status).ToList().ForEach(v =>
            {
                if (VendorHelper.checkExpires(v.Id))
                {
                    lastVendors.Add(v);
                }
            });
            ViewBag.lastVendors = lastVendors;
            return View("Index");
        }

    }
}
