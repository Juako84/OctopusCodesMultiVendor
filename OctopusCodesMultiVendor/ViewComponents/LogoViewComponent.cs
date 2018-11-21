using Microsoft.AspNetCore.Mvc;
using OctopusCodesMultiVendor.Models;
using System.Threading.Tasks;

namespace OctopusCodesMultiVendor.ViewComponents
{
    [ViewComponent(Name = "Logo")]
    public class LogoViewComponent : ViewComponent
    {
        private OctopusCodesMultiVendorsEntities ocmde = new OctopusCodesMultiVendorsEntities();

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var logo = ocmde.Settings.Find(10).Value;
            var websiteName = ocmde.Settings.Find(4).Value;
            ViewBag.logo = logo;
            ViewBag.websiteName = websiteName;
            return View("Index");
        }

    }
}
