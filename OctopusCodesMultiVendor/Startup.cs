using System.Globalization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.DependencyInjection;
using OctopusCodesMultiVendor.Middlewares;

namespace OctopusCodesMultiVendor
{
    public class Startup
    {
        private const string enUSCulture = "es-CL";

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddLocalization(options => options.ResourcesPath = "Resources");

            services.Configure<RequestLocalizationOptions>(options =>
            {
                var supportedCultures = new[]
                {
                    new CultureInfo(enUSCulture)
                    //new CultureInfo("es-CL")
                };
                options.DefaultRequestCulture = new RequestCulture(culture: enUSCulture, uiCulture: enUSCulture);
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;
            });

            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();

            services.AddSession();

            services.AddMvc()
                    .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
                    .AddDataAnnotationsLocalization();

        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            var supportedCultures = new[]
            {
                //new CultureInfo(enUSCulture),
                //new CultureInfo("en-AU"),
                //new CultureInfo("en-GB"),
                //new CultureInfo("en"),
                //new CultureInfo("es"),
                new CultureInfo("es-CL")
            };

            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture(enUSCulture),
                SupportedCultures = supportedCultures,
                SupportedUICultures = supportedCultures
            });

            app.UseSession();

            app.UseMiddleware<AuthenticationMiddleware>();

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                // need route and attribute on controller: [Area("Blogs")]
                routes.MapRoute(name: "areaRoute",
                                template: "{area:exists}/{controller=Home}/{action=Index}");
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");

               
            });

        }
    }
}
