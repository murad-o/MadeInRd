using ExporterWeb.Models;

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
                languageExporter.DirectorFirstName,
                languageExporter.DirectorSecondName,
                languageExporter.DirectorPatronymic);
        }

        internal static string ConstructFullName(string firstName, string secondName, string? patronymic)
        {
            string patronymicSpaced = patronymic is null
                ? string.Empty
                : " " + patronymic;
            return $"{firstName} {secondName}{patronymicSpaced}";
        }
    }
}
