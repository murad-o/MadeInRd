using ExporterWeb.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;

namespace ExporterWeb.Pages.Products
{
    public class IndexModel : BasePageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context, UserManager<User> userManager)
        {
            _context = context;
            UserManager = userManager;
        }

        public async Task OnGetAsync(int pageNumber = 1)
        {
            IQueryable<Product> products = _context.Products
                .Include(p => p.Industry)
                .Include(p => p.LanguageExporter);

            if (!IsAdminOrManager)
                products = products.Where(p => p.LanguageExporter!.Approved || p.LanguageExporterId == UserId);

            Products = await products.ToPagedListAsync(pageNumber, pageSize: 8);
        }

#nullable disable
        public IPagedList<Product> Products { get; set; }

        public bool CanCRUD(Product product) =>
            IsAdminOrManager || product.LanguageExporterId == UserId;
    }
}
