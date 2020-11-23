using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata.Ecma335;

namespace ExporterWeb.Models
{
    public class IndustryTranslation
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Это обязательное поле")]
        public string Name { get; set; } = "";

        [Required(ErrorMessage = "Это обязательное поле")]
        public string Description { get; set; } = "";

        public string? Image { get; set; }

        [Required]
        public string Language { get; set; } = "";
        public int Order { get; set; }

        public int IndustryId { get; set; }
        public Industry? Industry { get; set; }
    }
}