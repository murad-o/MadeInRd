using ExporterWeb.Helpers;
using ExporterWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace ExporterWeb.Pages.Admin.FieldsOfActivity
{
    public class BasePageModel : PageModel
    {
        public BasePageModel()
        {
            LocalizedNames = Languages.WhiteList
                .Select(language => new LocalizedNameInput { Language = language })
                .ToImmutableList();
        }

        [BindProperty]
        public IList<LocalizedNameInput> LocalizedNames { get; }

        private protected void FillFieldOfActivityNames(FieldOfActivity fieldOfActivity)
        {
            foreach (var localizedName in LocalizedNames)
            {
                localizedName.Name = fieldOfActivity.Name[localizedName.Language];
            }
        }

        public class LocalizedNameInput
        {
            public string Name { get; set; } = "";
            public string Language { get; set; } = "";
        }
    }
}
