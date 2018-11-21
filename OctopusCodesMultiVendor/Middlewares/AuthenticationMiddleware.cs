using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace OctopusCodesMultiVendor.Middlewares
{
    public class AuthenticationMiddleware
    {
        private readonly RequestDelegate _next;

        public AuthenticationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            var path = httpContext.Request.Path;
            if (path.HasValue && path.Value.StartsWith("/customerpanel") && !path.Value.StartsWith("/customerpanel/login/index") && !path.Value.StartsWith("/customerpanel/login/process"))
            {
                if (httpContext.Session.GetString("username_customer") == null)
                {
                    httpContext.Response.Redirect("/customerpanel/login/index");
                    return;
                }
            }
            if (path.HasValue && path.Value.StartsWith("/vendorpanel") && !path.Value.StartsWith("/vendorpanel/login/index") && !path.Value.StartsWith("/vendorpanel/login/process"))
            {
                if (httpContext.Session.GetString("username_vendor") == null)
                {
                    httpContext.Response.Redirect("/vendorpanel/login/index");
                    return;
                }
            }
            if (path.HasValue && path.Value.StartsWith("/adminpanel") && !path.Value.StartsWith("/adminpanel/login/index") && !path.Value.StartsWith("/adminpanel/login/process"))
            {
                if (httpContext.Session.GetString("username_admin") == null)
                {
                    httpContext.Response.Redirect("/adminpanel/login/index");
                    return;
                }
            }
            await _next(httpContext);
        }
    }

    public static class AuthenticationMiddlewareExtensions
    {
        public static IApplicationBuilder UseAuthenticationMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<AuthenticationMiddleware>();
        }
    }
}
