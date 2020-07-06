using ExporterWeb.Areas.Identity.Authorization;
using ExporterWeb.Helpers;
using ExporterWeb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExporterWeb.Pages.Products
{
    public class CreateModel : BasePageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly ILogger<CreateModel> _logger;
        private readonly IAuthorizationService _authorizationService;

        public CreateModel(ApplicationDbContext context,
            UserManager<User> userManager,
            ILogger<CreateModel> logger,
            IAuthorizationService authorizationService)
        {
            _context = context;
            _userManager = userManager;
            _logger = logger;
            _authorizationService = authorizationService;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            Init(_userManager);

            var exporters = _context.CommonExporters.Include(nameof(CommonExporter.User));
            if (!IsAdminOrManager)
                exporters = exporters.Where(e => e.UserId == UserId);

            var items = await exporters.ToListAsync();
            ViewData["CommonExporterList"] = new SelectList(items, "UserId", "User.Email");
            ViewData["WhiteListLanguages"] = new SelectList(Languages.WhiteList);
            var localizedFieldsOfActivity = await _context.FieldsOfActivity
                .Select(f => new { f.Id, Name = f.Name[Language!] })
                .ToListAsync();
            ViewData["FieldOfActivityId"] = new SelectList(localizedFieldsOfActivity, "Id", "Name");

            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            Init(_userManager);

            if (!ModelState.IsValid)
                return Page();

            if (!await IsAuthorized(Product))
            {
                string message = $"User {UserId} tries to create product with LanguageExporterId {Product.LanguageExporterId}";
                _logger.LogWarning(message);
                return Forbid();
            }

            Product.CreatedAt = DateTime.Now;
            // TODO: Disapprove if !IsAdminOrManager
            _context.Products!.Add(Product);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }

        private async Task<bool> IsAuthorized(Product product)
        {
            var result = await _authorizationService.AuthorizeAsync(
                User, product, AuthorizationOperations.Create);
            return result.Succeeded;
        }

#nullable disable
        [BindProperty]
        public Product Product { get; set; }
    }
}
