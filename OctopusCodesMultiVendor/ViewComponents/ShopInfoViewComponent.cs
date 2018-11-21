using Microsoft.AspNetCore.Mvc;
using OctopusCodesMultiVendor.Models;
using System.Threading.Tasks;
using System.Linq;

namespace OctopusCodesMultiVendor.ViewComponents
{
    [ViewComponent(Name = "ShopInfo")]
    public class ShopInfoViewComponent : ViewComponent
    {
        private OctopusCodesMultiVendorsEntities ocmde = new OctopusCodesMultiVendorsEntities();

        public async Task<IViewComponentResult> InvokeAsync()
        {
            ViewBag.shopInfo = ocmde.Settings.Find(5).Value;
            return View("Index");
        }

    }
}
