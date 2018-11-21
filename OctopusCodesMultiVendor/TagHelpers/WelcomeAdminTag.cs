using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace OctopusCodesMultiVendor.TagHelpers
{   
    [HtmlTargetElement("welcomeAdmin", TagStructure = TagStructure.NormalOrSelfClosing)]
    public class WelcomeAdminTag : TagHelper
    {
        private readonly IHttpContextAccessor _contextAccessor;

        public WelcomeAdminTag(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "";
            var session = _contextAccessor.HttpContext.Session;
            if(session.GetString("username_customer") != null)
            {
                output.Content.SetContent("Welcome " + session.GetString("username_customer"));
            }
            if (session.GetString("username_vendor") != null)
            {
                output.Content.SetContent("Welcome " + session.GetString("username_vendor"));
            }
            if (session.GetString("username_admin") != null)
            {
                output.Content.SetContent("Welcome " + session.GetString("username_admin"));
            }
        }
    }
}
