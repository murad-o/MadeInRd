using System.Globalization;
using System.Linq;
using System.Reflection;
using ExporterWeb.Areas.Identity.Authorization;
using ExporterWeb.Helpers.RouteModelConventions;
using ExporterWeb.Models;
using ExporterWeb.Resources;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Localization.Routing;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ExporterWeb.Helpers
{
    public static class ServicesExtensions
    {
        public static void ConfigureDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            const string dbConnectionString = "ExportersDbConnection";
            var connectionString = configuration.GetConnectionString(dbConnectionString);
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(connectionString));
        }

        public static void ConfigureIdentity(this IServiceCollection services)
        {
            services.AddIdentity<User, IdentityRole>(options =>
                {
                    options.SignIn.RequireConfirmedAccount = true;
                })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders()
                .AddErrorDescriber<MultiLanguageIdentityErrorDescriber>();

            services.ConfigureApplicationCookie(o => o.LoginPath = "/Identity/Account/Login");
        }

        public static void ConfigureRequestLocalizationOptions(this IServiceCollection services)
        {
            services.Configure<RequestLocalizationOptions>(options =>
            {
                var supportedCultures = Languages.WhiteList.Select(lang => new CultureInfo(lang)).ToList();
                options.DefaultRequestCulture = new RequestCulture(Languages.DefaultLanguage);
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;
                options.RequestCultureProviders.Insert(0, new RouteDataRequestCultureProvider { Options = options });
            });
        }

        public static void ConfigureLocalization(this IServiceCollection services)
        {
            services.AddLocalization(options => options.ResourcesPath = "Resources");

            services.AddMvc()
                .AddViewLocalization()
                .AddDataAnnotationsLocalization(options =>
                {
                    options.DataAnnotationLocalizerProvider = (type, factory) => factory.Create(nameof(ErrorsResources),Assembly.GetExecutingAssembly().GetName().FullName);
                });
        }

        private const string RequireAdministratorRole = "RequireAdministratorRole";
        public static void ConfigureAuthorization(this IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                options.AddPolicy(RequireAdministratorRole,
                    policy => policy.RequireRole(Constants.AdministratorsRole));
            });
        }

        public static void ConfigureRazorPages(this IServiceCollection services)
        {
            services.AddRazorPages(options => {
                options.Conventions.Add(new CultureTemplatePageRouteModelConvention());
                options.Conventions.AuthorizeFolder("/Admin/", RequireAdministratorRole);
            });
        }

        public static void ConfigureRouteOptions(this IServiceCollection services)
        {
            services.Configure<RouteOptions>(options =>
            {
                options.LowercaseUrls = true;
            });
        }
    }
}