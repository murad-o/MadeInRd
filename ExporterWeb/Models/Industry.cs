using System.Collections.Generic;

namespace ExporterWeb.Models
{
    public class Industry
    {
        public Industry()
        {
            Translations = new List<IndustryTranslation>();
        }
        public int Id { get; set; }
        public ICollection<IndustryTranslation>? Translations { get; set; }
    }
}