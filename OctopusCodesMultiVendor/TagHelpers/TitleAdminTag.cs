using Microsoft.AspNetCore.Razor.TagHelpers;
using OctopusCodesMultiVendor.Models;

namespace OctopusCodesMultiVendor.TagHelpers
{
    [HtmlTargetElement("titleAdmin", TagStructure = TagStructure.NormalOrSelfClosing)]
    public class TitleAdminTag : TagHelper
    {
        private OctopusCodesMultiVendorsEntities ocmd = new OctopusCodesMultiVendorsEntities();

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "";
            output.Content.SetContent(ocmd.Settings.Find(4).Value);
        }
    }
}
