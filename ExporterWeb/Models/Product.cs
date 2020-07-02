using System;
using System.ComponentModel.DataAnnotations;

namespace ExporterWeb.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = "";

        public string? Description { get; set; }

        public string LanguageExporterId { get; set; } = "";
        public string Language { get; set; } = "";
        [Required]
        // Foreign key from Fluent API
        public LanguageExporter? LanguageExporter { get; set; }

        [Required]
        public int FieldOfActivityId { get; set; }
        public FieldOfActivity? FieldOfActivity { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
