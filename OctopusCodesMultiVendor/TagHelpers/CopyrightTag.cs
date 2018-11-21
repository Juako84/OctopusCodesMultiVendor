using Microsoft.AspNetCore.Razor.TagHelpers;
using OctopusCodesMultiVendor.Models;
using System;

namespace OctopusCodesMultiVendor.TagHelpers
{
    [HtmlTargetElement("copyright", TagStructure = TagStructure.NormalOrSelfClosing)]
    public class CopyrightTag : TagHelper
    {
        private OctopusCodesMultiVendorsEntities ocmd = new OctopusCodesMultiVendorsEntities();

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "";
            output.Content.SetHtmlContent("<strong>Copyright &copy; " + DateTime.Now.Year + " OctopusCodes.</strong> All rights reserved.");
        }
    }
}
