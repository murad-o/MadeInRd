using System.ComponentModel.DataAnnotations;

namespace ExporterWeb.Models
{
    public class Dummy
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string? X { get; set; }

        public override string ToString()
        {
            return $"Dummy{{Id: {Id}, X: {X}}}";
        }
    }
}
