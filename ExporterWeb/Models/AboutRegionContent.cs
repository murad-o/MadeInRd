using System.ComponentModel.DataAnnotations;

namespace ExporterWeb.Models
{
    public class AboutRegionModel
    {
        [Key]
        public string Lang { get; set; } = "";

        [Required]
        public string Content { get; set; } = "";
    }
}