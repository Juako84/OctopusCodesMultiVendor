using Microsoft.AspNetCore.Mvc;
using OctopusCodesMultiVendor.Models;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Http;
using System;

namespace OctopusCodesMultiVendor.ViewComponents
{
    [ViewComponent(Name = "CategoriesVendor")]
    public class CategoriesVendorViewComponent : ViewComponent
    {
        private OctopusCodesMultiVendorsEntities ocmde = new OctopusCodesMultiVendorsEntities();

        private readonly IHttpContextAccessor _contextAccessor;

        public CategoriesVendorViewComponent(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var uri = GetUri(_contextAccessor.HttpContext.Request);
            var isVendor = uri.Segments.Length >= 2 
                            && uri.Segments[1].ToString().Trim().ToLower().StartsWith("vendors".ToLower()) 
                            && !uri.Segments[2].ToString().Trim().ToLower().StartsWith("MemberShip".ToLower())
                            && !uri.Segments[2].ToString().Trim().ToLower().StartsWith("Register".ToLower())
                            && !uri.Segments[2].ToString().Trim().ToLower().StartsWith("Success".ToLower())
                            && !uri.Segments[2].ToString().Trim().ToLower().StartsWith("Expires".ToLower());
            ViewBag.categories = ocmde.Categories.Where(c => c.Status).ToList();
            ViewBag.isVendor = isVendor;
            return View("Index");
        }

        private Uri GetUri(HttpRequest request)
        {
            var hostComponents = request.Host.ToUriComponent().Split(':');

            var builder = new UriBuilder
            {
                Scheme = request.Scheme,
                Host = hostComponents[0],
                Path = request.Path,
                Query = request.QueryString.ToUriComponent()
            };

            if (hostComponents.Length == 2)
            {
                builder.Port = Convert.ToInt32(hostComponents[1]);
            }

            return builder.Uri;
        }

    }
}
