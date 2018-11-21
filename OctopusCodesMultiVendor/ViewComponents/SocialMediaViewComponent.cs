using Microsoft.AspNetCore.Mvc;
using OctopusCodesMultiVendor.Models;
using System.Threading.Tasks;

namespace OctopusCodesMultiVendor.ViewComponents
{
    [ViewComponent(Name = "SocialMedia")]
    public class SocialMediaViewComponent : ViewComponent
    {
        private OctopusCodesMultiVendorsEntities ocmde = new OctopusCodesMultiVendorsEntities();

        public async Task<IViewComponentResult> InvokeAsync()
        {
            ViewBag.facebookUrl = ocmde.Settings.Find(1).Value;
            ViewBag.youtubeUrl = ocmde.Settings.Find(2).Value;
            ViewBag.twitterUrl = ocmde.Settings.Find(3).Value;
            return View("Index");
        }

    }
}
