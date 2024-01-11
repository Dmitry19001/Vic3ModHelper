using System.Collections.Generic;

namespace Vic3ModManager
{
    public class LocalizableTextEntry
    {
        public string Key { get; set; }
        public Dictionary<string, string> Translations { get; set; } = [];

        public LocalizableTextEntry(string key)
        {
            Key = key;

            SetTranslation(key);
        }

        public void SetTranslation(string translation, string? language = null)
        {
            language ??= GameLanguages.ToString(ModManager.CurrentMod.DefaultLanguage);

            if (!Translations.ContainsKey(language)) 
            { 
                Translations.Add(language, translation);
            }
            else 
            {
                Translations[language] = translation;
            }
        }

        public override string ToString()
        {
            string defaultLanguage = GameLanguages.ToString(ModManager.CurrentMod.DefaultLanguage);

            if (Translations.ContainsKey(defaultLanguage))
            {
                return Translations[defaultLanguage];
            }

            return Key;
        }

        public string ToStringByLanguage(string language)
        {
            if (Translations.ContainsKey(language))
            {
                return Translations[language];
            }

            return Key;
        }
    }
}
