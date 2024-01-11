namespace Vic3ModManager
{
    // Need to convert DefaultLanguages to ingame id for localization.
    // English: "l_english" etc.
    public static class GameLanguages
    {
        // Here are all the languages that are supported by the game.
        // l_english: "English"
        // l_braz_por: "Português do Brasil"
        // l_french: "Français"
        // l_german: "Deutsch"
        // l_polish: "Polski"
        // l_russian: "Русский"
        // l_spanish: "Español"
        // l_japanese: "日本語"
        // l_simp_chinese: "中文"
        // l_korean: "한국어"
        // l_turkish: "Türkçe"
        public enum DefaultLanguages
        {
            Custom,
            English,
            BrazilianPortuguese,
            French,
            German,
            Polish,
            Russian,
            Spanish,
            Japanese,
            SimplifiedChinese,
            Korean,
            Turkish
        }

        public static string? ToIngameId(this DefaultLanguages language)
        {
            return language switch
            {
                DefaultLanguages.English => "l_english",
                DefaultLanguages.BrazilianPortuguese => "l_braz_por",
                DefaultLanguages.French => "l_french",
                DefaultLanguages.German => "l_german",
                DefaultLanguages.Polish => "l_polish",
                DefaultLanguages.Russian => "l_russian",
                DefaultLanguages.Spanish => "l_spanish",
                DefaultLanguages.Japanese => "l_japanese",
                DefaultLanguages.SimplifiedChinese => "l_simp_chinese",
                DefaultLanguages.Korean => "l_korean",
                DefaultLanguages.Turkish => "l_turkish",
                _ => null,
            };
        }

        public static string CustomToIngameId(string customLang)
        {
            if (customLang.Length < 1) return "l_english";

            if (customLang.StartsWith("l_")) return customLang;

            return $"l_{customLang.ToLower()}";
        }

        // Simple method to convert the enum to a string.
        // When app localization is implemented, this will be redone.
        public static string ToString(this DefaultLanguages language)
        {
            return language switch
            {
                DefaultLanguages.Custom => "Custom",
                DefaultLanguages.English => "English",
                DefaultLanguages.BrazilianPortuguese => "Brazilian Portuguese",
                DefaultLanguages.French => "French",
                DefaultLanguages.German => "German",
                DefaultLanguages.Polish => "Polish",
                DefaultLanguages.Russian => "Russian",
                DefaultLanguages.Spanish => "Spanish",
                DefaultLanguages.Japanese => "Japanese",
                DefaultLanguages.SimplifiedChinese => "Simplified Chinese",
                DefaultLanguages.Korean => "Korean",
                DefaultLanguages.Turkish => "Turkish",
                _ => language.ToString(),// Return the enum name for unknown values
            };
        }
    }   


}
