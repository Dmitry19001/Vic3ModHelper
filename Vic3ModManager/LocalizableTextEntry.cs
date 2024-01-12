using System.Collections.Generic;

namespace Vic3ModManager
{
    public class LocalizableTextEntry
    {
        public string Key { get; set; }
        public Dictionary<int, Translation> Translations { get; set; } = [];

        public LocalizableTextEntry(string key)
        {
            Key = key;
            SetTranslation(key);
        }

        public void SetTranslation(string translation, string? language = null, int index = 0)
        {
            language ??= GameLanguages.ToString(ModManager.CurrentMod.DefaultLanguage);

            if (!Translations.ContainsKey(index))
            {
                Translations.Add(index, new Translation(language, translation));
            }
            else
            {
                Translations[index] = new Translation(language, translation);
            }
        }

        public override string ToString()
        {
            string defaultLanguage = GameLanguages.ToString(ModManager.CurrentMod.DefaultLanguage);

            foreach (var translation in Translations.Values)
            {
                if (translation.Language == defaultLanguage)
                {
                    return translation.Text;
                }
            }

            return Key;
        }

        public string ToStringByLanguage(string language)
        {
            foreach (var translation in Translations.Values)
            {
                if (translation.Language == language)
                {
                    return translation.Text;
                }
            }

            return Key;
        }
    }
}
