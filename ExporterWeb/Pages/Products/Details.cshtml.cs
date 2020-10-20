using ExporterWeb.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace ExporterWeb.Pages.Products
{
    public class DetailsModel : BasePageModel
    {
        private readonly ApplicationDbContext _context;

        public DetailsModel(ApplicationDbContext context, UserManager<User> userManager)
        {
            _context = context;
            UserManager = userManager;
        }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id is null)
                return NotFound();

            IQueryable<Product> products = _context.Products
                .Include(p => p.FieldOfActivity)
                .Include(p => p.LanguageExporter);

            if (!IsAdminOrManager)
                products = products.Where(p => p.LanguageExporter!.Approved || p.LanguageExporterId == UserId);

            Product = await products
                .FirstOrDefaultAsync(m => m.Id == id);

            if (Product is null)
                return NotFound();

            return Page();
        }

#nullable disable
        public Product Product { get; set; }
    }
}
