using ExporterWeb.Areas.Identity.Authorization;
using ExporterWeb.Helpers;
using ExporterWeb.Helpers.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace ExporterWeb
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.ConfigureDatabase(Configuration);

            services.ConfigureIdentity();

            services.ConfigureRazorPages();
            
            services.AddControllers();
            
            services.ConfigureAuthorization();

            services.ConfigureLocalization();

            services.ConfigureRouteOptions();

            services.ConfigureRequestLocalizationOptions();

            services.AddScoped<IAuthorizationHandler, ExporterOwnerAuthorizationHandler>();
            services.AddScoped<IAuthorizationHandler, ProductOwnerAuthorizationHandler>();
            services.AddSingleton<CommonLocalizationService>();
            services.AddSingleton<ErrorsLocalizationService>();
            services.AddSingleton<ImageService>();
            services.AddSingleton<ImageResizeService>();
            services.AddSingleton<IAuthorizationHandler, ManagerAuthorizationHandler>();
            services.AddSingleton<IAuthorizationHandler, AdministratorAuthorizationHandler>();
            services.AddTransient<IEmailSender, EmailSender>();
            services.AddTransient<RazorPartialToStringRenderer>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();

            var localizationOptions = app.ApplicationServices.GetService<IOptions<RequestLocalizationOptions>>().Value;
            app.UseRequestLocalization(localizationOptions);

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllers();
            });
        }
    }
}
