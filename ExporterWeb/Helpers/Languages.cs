using System.Collections.Generic;

namespace ExporterWeb.Helpers
{
    public static class Languages
    {
        public static HashSet<string> WhiteList => new HashSet<string>
        {
            "ru",
            "en",
            "az",
            "tr",
            "ar",
            "fa"
        };

        public const string DefaultLanguage = "ru";
    }
}
