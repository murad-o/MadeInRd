using System.Reflection;
using ExporterWeb.Resources;
using Microsoft.Extensions.Localization;

namespace ExporterWeb.Helpers.Services
{
    public class ErrorsLocalizationService
    {
        private readonly IStringLocalizer _localizer;
        public ErrorsLocalizationService(IStringLocalizerFactory factory)
        {
            _localizer = factory.Create(nameof(ErrorsResources), Assembly.GetExecutingAssembly().GetName().Name);
        }
        
        public string this[string key] => _localizer[key];
    }
}