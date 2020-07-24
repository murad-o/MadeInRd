using System;
using System.ComponentModel.DataAnnotations;

namespace ExporterWeb.Models
{
    public class NewsModel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = "";

        [Required]
        public string Description { get; set; } = "";

        [Required]
        public string Language { get; set; } = "";

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [Required]
        public string UserNameOwner { get; set; } = "";

        public string? Logo { get; set; }
    }
}
