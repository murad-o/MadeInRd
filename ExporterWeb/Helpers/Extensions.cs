using ExporterWeb.Helpers.Services;
using ExporterWeb.Models;
using System.IO;

namespace ExporterWeb.Helpers
{
    public static class Extensions
    {
        public static string ContactPersonFullName(this LanguageExporter languageExporter)
        {
            return ConstructFullName(
                languageExporter.ContactPersonFirstName,
                languageExporter.ContactPersonSecondName,
                languageExporter.ContactPersonPatronymic);
        }

        public static string DirectorFullName(this LanguageExporter languageExporter)
        {
            return ConstructFullName(
                languageExporter.DirectorFirstName!,
                languageExporter.DirectorSecondName!,
                languageExporter.DirectorPatronymic);
        }

        private static string ConstructFullName(string firstName, string secondName, string? patronymic)
        {
            string patronymicSpaced = patronymic is null
                ? string.Empty
                : " " + patronymic;
            return $"{firstName} {secondName}{patronymicSpaced}";
        }

        public static string LogoPath(this NewsModel news)
        {
            if (news.Logo is null)
                return "/img/news-icon.png";
            return Path.Combine("/", ImageService.GetWebRelativePath(ImageTypes.NewsLogo), news.Logo).Replace('\\', '/');
        }

        public static string LogoPath(this Event @event)
        {
            if (@event.Logo is null)
                return "/img/news-icon.png";
            return Path.Combine("/", ImageService.GetWebRelativePath(ImageTypes.EventLogo), @event.Logo).Replace('\\', '/');
        }

        public static string LogoPath(this LanguageExporter exporter)
        {
            if (exporter.Logo is null)
                return "/img/news-icon.png";
            return Path.Combine("/", ImageService.GetWebRelativePath(ImageTypes.ExporterLogo),
                exporter.Logo).Replace('\\', '/');
        }

        public static string LogoPath(this Product product)
        {
            if (product.Logo is null)
                return "/img/news-icon.png";
            return Path.Combine("/", ImageService.GetWebRelativePath(ImageTypes.ProductLogo),
                product.Logo).Replace('\\', '/');
        }
        
        public static string LogoPath(this IndustryTranslation industry)
        {
            if (industry.Image is null)
                return "";
            return Path.Combine("/", ImageService.GetWebRelativePath(ImageTypes.IndustryImage),
                industry.Image!).Replace('\\', '/');
        }
    }
}
