using Microsoft.AspNetCore.Mvc;
using OctopusCodesMultiVendor.Models;
using System.Threading.Tasks;
using System.Linq;

namespace OctopusCodesMultiVendor.ViewComponents
{
    [ViewComponent(Name = "ProductSearch")]
    public class ProductSearchViewComponent : ViewComponent
    {
        private OctopusCodesMultiVendorsEntities ocmde = new OctopusCodesMultiVendorsEntities();

        public async Task<IViewComponentResult> InvokeAsync()
        {
            ViewBag.categories = ocmde.Categories.Where(c => c.VendorId == null && c.Status && c.ParentId == null).ToList();
            return View("Index");
        }

    }
}
