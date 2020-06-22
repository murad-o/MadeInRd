using Microsoft.EntityFrameworkCore;

namespace ExporterWeb.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Dummy>? Dummies { get; set; }
    }
}
