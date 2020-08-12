using ExporterWeb.Areas.Identity.Authorization;
using ExporterWeb.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExporterWeb
{
    public static class FixtureUtils
    {
        private const string DbKeyConfig = "db";

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
                var dbSection = config.GetSection(DbKeyConfig);
                SeedData.Initialize(services, dbSection).Wait();
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
        private const string AdministratorsKeyConfig = "administrators";
        private const string ManagersKeyConfig = "managers";
        private const string AnalystsKeyConfig = "analysts";

        public static async Task Initialize(IServiceProvider serviceProvider, IConfigurationSection dbConfig)
        {
            var administrators = dbConfig.GetSection(AdministratorsKeyConfig).GetChildren();
            var managers = dbConfig.GetSection(ManagersKeyConfig).GetChildren();
            var analysts = dbConfig.GetSection(AnalystsKeyConfig).GetChildren();

            var options = serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>();
            await using var context = new ApplicationDbContext(options);

            await EnsureRole(serviceProvider, Constants.AdministratorsRole);
            await EnsureRole(serviceProvider, Constants.ManagersRole);
            await EnsureRole(serviceProvider, Constants.AnalystsRole);
            await EnsureRole(serviceProvider, Constants.ExportersRole);

            await EnsureUsersAreInRole(serviceProvider, administrators, Constants.AdministratorsRole);
            await EnsureUsersAreInRole(serviceProvider, managers, Constants.ManagersRole);
            await EnsureUsersAreInRole(serviceProvider, analysts, Constants.AnalystsRole);
        }

        private static async Task<User> EnsureUser(IServiceProvider serviceProvider, string email, string password)
        {
            var userManager = serviceProvider.GetService<UserManager<User>>();

            var user = await userManager.FindByEmailAsync(email);

            if (user is {})
                return user;

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

            return user;
        }

        private static async Task EnsureRole(IServiceProvider serviceProvider, string role)
        {
            var roleManager = serviceProvider.GetService<RoleManager<IdentityRole>>();
            if (!await roleManager.RoleExistsAsync(role))
                await roleManager.CreateAsync(new IdentityRole(role));
        }

        private static async Task EnsureUsersAreInRole(
            IServiceProvider serviceProvider,
            IEnumerable<IConfigurationSection> users,
            string role)
        {
            foreach (var data in users)
            {
                var email = data.Key;
                var password = data.Value;
                var user = await EnsureUser(serviceProvider, email, password);
                await EnsureUserIsInRole(serviceProvider, user, role);
            }
        }

        private static async Task EnsureUserIsInRole(IServiceProvider serviceProvider, User user, string role)
        {
            var userManager = serviceProvider.GetService<UserManager<User>>();
            var usersInThatRole = await userManager.GetUsersInRoleAsync(role);
            if (!usersInThatRole.Contains(user))
                await userManager.AddToRoleAsync(user, role);
        }
    }
}
