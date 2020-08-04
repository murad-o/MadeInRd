using ExporterWeb.Areas.Identity.Authorization;
using ExporterWeb.Helpers;
using ExporterWeb.Helpers.Services;
using ExporterWeb.Models;
using ExporterWeb.Resources;
using ExporterWeb.Helpers.RouteModelConventions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Localization.Routing;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace ExporterWeb
{
    public class Startup
    {
        private const string DbConnectionString = "ExportersDbConnection";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var connectionString = Configuration.GetConnectionString(DbConnectionString);
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(connectionString));

            services.AddDefaultIdentity<User>(options =>
                    options.SignIn.RequireConfirmedAccount = true)
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            string requireAdministratorRole = "RequireAdministratorRole";
            services.AddRazorPages(options => {
                options.Conventions.Add(new CultureTemplatePageRouteModelConvention());
                options.Conventions.AuthorizeFolder("/Admin/Users", requireAdministratorRole);
            });

            services.AddAuthorization(options =>
            {
                options.AddPolicy(requireAdministratorRole,
                     policy => policy.RequireRole(Constants.AdministratorsRole));
            });

            services.AddLocalization(options => options.ResourcesPath = "Resources");

            services.AddMvc()
                .AddViewLocalization()
                .AddDataAnnotationsLocalization(options =>
                {
                    options.DataAnnotationLocalizerProvider = (type, factory) =>
                    {
                        return factory.Create(nameof(CommonResources), Assembly.GetExecutingAssembly().GetName().Name);
                    };
                });

            services.Configure<RouteOptions>(options =>
            {
                options.LowercaseUrls = true;
            });

            services.AddControllers(config =>
            {
                var policy = new AuthorizationPolicyBuilder()
                                 .RequireAuthenticatedUser()
                                 .Build();
                config.Filters.Add(new AuthorizeFilter(policy));
            });

            services.Configure<RequestLocalizationOptions>(options =>
            {
                var supportedCultures = Languages.WhiteList.Select(lang => new CultureInfo(lang)).ToList();
                options.DefaultRequestCulture = new RequestCulture(Languages.DefaultLanguage);
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;
                options.RequestCultureProviders.Insert(0, new RouteDataRequestCultureProvider { Options = options });
            });

            services.AddScoped<IAuthorizationHandler, ExporterOwnerAuthorizationHandler>();
            services.AddScoped<IAuthorizationHandler, ProductOwnerAuthorizationHandler>();
            services.AddSingleton<CommonLocalizationService>();
            services.AddSingleton<ImageService>();
            services.AddSingleton<IAuthorizationHandler, ManagerAuthorizationHandler>();
            services.AddSingleton<IAuthorizationHandler, AdministratorAuthorizationHandler>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
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
            });
        }
    }
}
