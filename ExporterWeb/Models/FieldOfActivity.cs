using System.ComponentModel.DataAnnotations;

namespace ExporterWeb.Models
{
    public class FieldOfActivity
    {
        [Key]
        public int Id { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string Name { get; set; } = "";
    }
}
