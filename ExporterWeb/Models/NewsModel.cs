using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

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
    }
}
