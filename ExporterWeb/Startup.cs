using ExporterWeb.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.IO;

namespace ExporterWeb
{
    public class Startup
    {
        private const string DbConnectionString = "ExportersDbConnection";
        private const string PathToAppSettingsSecret = "appsettings.Secrets.json";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            IConfigurationRoot secretConfig = GetSecretConfig();

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(secretConfig.GetConnectionString(DbConnectionString)));

            services.AddDefaultIdentity<IdentityUser>(options => 
                options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<ApplicationDbContext>();
            services.AddRazorPages();
        }

        private static IConfigurationRoot GetSecretConfig()
        {
            try
            {
                return new ConfigurationBuilder()
                    .AddJsonFile(PathToAppSettingsSecret)
                    .Build();
            }
            catch (FileNotFoundException e)
            {
                throw new FileNotFoundException(
                    $"Please create \"{PathToAppSettingsSecret}\". In README.md read the setup section for help",
                    e
                );
            }
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

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });
        }
    }
}
