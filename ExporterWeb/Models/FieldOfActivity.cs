using ExporterWeb.Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExporterWeb.Models
{
    public class FieldOfActivity
    {
        [Key]
        public int Id { get; set; }

        private CustomLanguage? custom;
        [NotMapped]
        public CustomLanguage Name
        {
            get
            {
                if (custom is null)
                {
                    custom = new CustomLanguage(SerializedNames);
                    custom.UpdateValue += serializedString
                        => SerializedNames = serializedString;
                }
                return custom;
            }
        }

        [Required(AllowEmptyStrings = false)]
        [Column(nameof(Name))]
        public string SerializedNames { get; set; } = "{}";
    }

    public class CustomLanguage
    {
        private readonly Dictionary<string, string> _names;
        public CustomLanguage(string json)
        {
            _names = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
        }

        public string this[string language]
        {
            get
            {
                _names.TryGetValue(language, out string? name);
                return name ?? _names[Languages.DefaultLanguage];
            }
            set
            {
                _names[language] = value;
                string json = JsonConvert.SerializeObject(_names);
                UpdateValue?.Invoke(json);
            }
        }

        public event Action<string>? UpdateValue;
    }
}
