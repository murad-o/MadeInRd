using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExporterWeb.Models
{
    public class Exporter
    {
        [Required]
        public string Name { get; set; } = "";

        [Required]
        public string Description { get; set; } = "";

        [Required]
        public string Firstname { get; set; } = "";

        [Required]
        public string SecondName { get; set; } = "";

        public string? Patronymic { get; set; }
        public string? Website { get; set; }

        [Required]
        [MaxLength(12)]
        public string INN { get; set; } = "";

        [Required]
        [MaxLength(15)]
        public string OGRN_IP { get; set; } = "";

        [Key]
        [Required]
        [ForeignKey(nameof(User))]
        public string UserId { get; set; } = "";
        public User? User { get; set; }

        [Required]
        public int FieldOfActivityId { get; set; }
        public FieldOfActivity? FieldOfActivity { get; set; }
    }
}
