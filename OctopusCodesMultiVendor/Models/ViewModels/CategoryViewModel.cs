using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace OctopusCodesMultiVendor.Models.ViewModels
{
    public class CategoryViewModel
    {
        public Category category { get; set; }

        public List<SelectListItem> Parent { get; set; }
    }
}