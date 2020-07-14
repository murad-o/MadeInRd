using System;
using System.ComponentModel.DataAnnotations;

namespace ExporterWeb.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Name"), Required(ErrorMessage = "Name is required")]
        public string Name { get; set; } = "";

        public string? Description { get; set; }

        // Foreign key from Fluent API
        public string Language { get; set; } = "";

        public string LanguageExporterId { get; set; } = "";
        public LanguageExporter? LanguageExporter { get; set; }

        [Required]
        public int FieldOfActivityId { get; set; }
        public FieldOfActivity? FieldOfActivity { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
