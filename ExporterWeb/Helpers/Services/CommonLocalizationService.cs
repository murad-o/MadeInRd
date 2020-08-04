using ExporterWeb.Resources;
using Microsoft.Extensions.Localization;
using System.Reflection;

namespace ExporterWeb.Helpers.Services
{
    public class CommonLocalizationService
    {
        private readonly IStringLocalizer localizer;
        public CommonLocalizationService(IStringLocalizerFactory factory)
        {
            localizer = factory.Create(nameof(CommonResources), Assembly.GetExecutingAssembly().GetName().Name);
        }

        public string this[string key]
        {
            get => localizer[key];
        }
    }
}
