using System;
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

        public static string ToRussian(string language) => language switch
        {
            "ru" => "русский",
            "en" => "английский",
            "az" => "азербайджанский",
            "tr" => "турецкий",
            "ar" => "арабский",
            "fa" => "персидский",
            _ => throw new ArgumentException("Несуществующий язык")
        };
    }
}
