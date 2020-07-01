using ExporterWeb.Areas.Identity.Authorization;
using ExporterWeb.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ExporterWeb
{
    public static class FixtureUtils
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage(
            "CodeQuality", "IDE0051:Remove unused private members", Justification = "<Pending>")]
        public static void PopulateDatabase(IHost host)
        {
            using var scope = host.Services.CreateScope();
            var services = scope.ServiceProvider;
            try
            {
                var context = services.GetService<ApplicationDbContext>();
                context.Database.Migrate();

                var config = host.Services.GetRequiredService<IConfiguration>();
                //string password = config["AdministratorPassword"];
                // TODO: fetch password from some secret after
                // investigation of the best way to store them
                string password = "1234qwE!";

                SeedData.Initialize(services, password).Wait();
            }
            catch (Exception ex)
            {
                var logger = services.GetService<ILogger<Program>>();
                logger.LogError(ex, "An error occurred seeding the DB.");
                throw;
            }
        }
    }

    static class SeedData
    {
        public static async Task Initialize(IServiceProvider serviceProvider, string password)
        {
            var options = serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>();
            using var context = new ApplicationDbContext(options);

            var admin = await EnsureUser(serviceProvider, "admin@example.com", password);
            await EnsureRole(serviceProvider, Constants.AdministratorsRole, admin);

            var manager = await EnsureUser(serviceProvider, "manager@example.com", password);
            await EnsureRole(serviceProvider, Constants.ManagersRole, manager);

            await EnsureRole(serviceProvider, Constants.ExportersRole, null);
        }

        private static async Task<User> EnsureUser(IServiceProvider serviceProvider, string email, string password)
        {
            var userManager = serviceProvider.GetService<UserManager<User>>();

            var user = await userManager.FindByEmailAsync(email);
            if (user == null)
            {
                user = new User(email)
                {
                    Email = email,
                    EmailConfirmed = true
                };
                var result = await userManager.CreateAsync(user, password);
                if (!result.Succeeded)
                {
                    var message = string.Join(", ", result.Errors.Select(e => e.Code + " " + e.Description));
                    throw new Exception($"Errors when creating a user {email}: {message}");
                }
            }

            return user;
        }

        private static async Task EnsureRole(IServiceProvider serviceProvider, string role, User? user)
        {
            var roleManager = serviceProvider.GetService<RoleManager<IdentityRole>>();

            if (roleManager == null)
            {
                throw new Exception("roleManager is null");
            }

            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }

            if (user is { })
            {
                var userManager = serviceProvider.GetService<UserManager<User>>();
                await userManager.AddToRoleAsync(user, role);
            }
        }
    }
}
