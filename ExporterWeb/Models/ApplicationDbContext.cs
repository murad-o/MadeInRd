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
            // Add composite Primary Key for LanguageExporter(CommonExporterId, Language)
            builder.Entity<LanguageExporter>(langExpTable =>
                langExpTable.HasKey(k => new { k.CommonExporterId, k.Language }));

            // Add composite Foreign Key from Product(LanguageExporterId, Language)
            // to LanguageExporter(LanguageExporterId, Language)
            // Cascade not delete due to "... constraint ... on table 'Products' may cause cycles or multiple cascade paths"
            builder.Entity<Product>()
                .HasOne(productTable => productTable.LanguageExporter)
                .WithMany(languageExporter => languageExporter!.Products)
                .HasForeignKey(productTable => new { productTable.LanguageExporterId, productTable.Language })
                .OnDelete(DeleteBehavior.ClientCascade);
            base.OnModelCreating(builder);
        }

        public DbSet<Dummy>? Dummies { get; set; }
        public DbSet<FieldOfActivity>? FieldsOfActivity { get; set; }
        public DbSet<CommonExporter>? CommonExporters { get; set; }
        public DbSet<LanguageExporter>? LanguageExporters { get; set; }
        public DbSet<Product>? Products { get; set; }
        public DbSet<NewsModel> News { get; set; }
    }
}
