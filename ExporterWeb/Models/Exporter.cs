using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExporterWeb.Models
{
    public class CommonExporter
    {
        [Key]
        [Required]
        public string UserId { get; set; } = "";
        [ForeignKey(nameof(UserId))]
        public virtual User? User { get; set; }

        [Required]
        [MaxLength(12)]
        public string INN { get; set; } = "";

        [Required]
        [MaxLength(15)]
        public string OGRN_IP { get; set; } = "";

        public string? LogoPath { get; set; }

        [Required]
        public int FieldOfActivityId { get; set; }
        public FieldOfActivity? FieldOfActivity { get; set; }
    }

    // Composite primary key is set through Fluent API
    public class LanguageExporter
    {
        [Required]
        public string CommonExporterId { get; set; } = "";
        [ForeignKey(nameof(CommonExporterId))]
        public virtual CommonExporter? CommonExporter { get; set; }

        [Required]
        public string Language { get; set; } = "";

        [Required]
        public string Name { get; set; } = "";

        public string? Description { get; set; }

        [Required]
        public string ContactPersonFirstName { get; set; } = "";
        [Required]
        public string ContactPersonSecondName { get; set; } = "";
        public string? ContactPersonPatronymic { get; set; }

        [Required]
        public string DirectorFirstName { get; set; } = "";
        [Required]
        public string DirectorSecondName { get; set; } = "";
        public string? DirectorPatronymic { get; set; }

        public string? WorkingTime { get; set; }

        public string? Address { get; set; }

        public string? Website { get; set; }

        [Required]
        public bool Approved { get; set; }
    }
}
