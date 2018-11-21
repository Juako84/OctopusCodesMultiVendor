using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace OctopusCodesMultiVendor.Models.ViewModels
{
    public class ProductViewModel
    {
        public Product product { get; set; }

        public List<SelectListItem> Categories { get; set; }
    }
}