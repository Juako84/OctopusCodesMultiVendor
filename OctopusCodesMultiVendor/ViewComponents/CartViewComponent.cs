using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using OctopusCodesMultiVendor.Helpers;
using OctopusCodesMultiVendor.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace OctopusCodesMultiVendor.ViewComponents
{
    [ViewComponent(Name = "Cart")]
    public class CartViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            decimal total = 0;
            int totalItem = 0;
            var cart = SessionHelper.GetObjectFromJson<List<Item>>(HttpContext.Session, "cart");
            if (cart != null)
            {
                total = cart.Sum(i => i.Price * i.Quantity);
                totalItem = cart.Count;
            }
            ViewBag.total = total;
            ViewBag.totalItem = totalItem;
            return View("Index");
        }

    }
}
