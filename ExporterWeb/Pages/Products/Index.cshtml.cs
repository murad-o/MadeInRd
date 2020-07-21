using ExporterWeb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExporterWeb.Pages.Products
{
    [AllowAnonymous]
    public class IndexModel : BasePageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context, UserManager<User> userManager)
        {
            _context = context;
            UserManager = userManager;
        }

        public async Task OnGetAsync()
        {
            IQueryable<Product> products = _context.Products
                .Include(p => p.FieldOfActivity)
                .Include(p => p.LanguageExporter);

            if (!IsAdminOrManager)
                products = products.Where(p => p.LanguageExporter!.Approved || p.LanguageExporterId == UserId);

            Products = await products.ToListAsync();
        }

#nullable disable
        public IList<Product> Products { get; set; } = Array.Empty<Product>();

        public bool CanCRUD(Product product) =>
            IsAdminOrManager || product.LanguageExporterId == UserId;
    }
}
