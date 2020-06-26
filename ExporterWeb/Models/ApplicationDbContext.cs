using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ExporterWeb.Models
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<LanguageExporter>(langExpTable =>
                langExpTable.HasKey(k => new { k.CommonExporterId, k.Language }));
            base.OnModelCreating(builder);
        }

        public DbSet<Dummy>? Dummies { get; set; }
        public DbSet<FieldOfActivity>? FieldsOfActivity { get; set; }
        public DbSet<CommonExporter>? CommonExporters { get; set; }
        public DbSet<LanguageExporter>? LanguageExporters { get; set; }
    }
}
