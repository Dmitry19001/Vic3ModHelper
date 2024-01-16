using System.Collections.Generic;

namespace Vic3ModManager
{
    public class LocalizableTextEntry
    {
        public string Key { get; set; }
        public List<Translation> Translations { get; set; } = [];

        public LocalizableTextEntry(string key)
        {
            Key = key;

            SetTranslation(key);
        }

        public void SetTranslation(string translation, string? language = null, int index = 0)
        {
            if (ModManager.CurrentMod != null)
            {
                language ??= GameLanguages.ToString(ModManager.CurrentMod.DefaultLanguage);
            }
            else
            {
                language ??= GameLanguages.ToString(GameLanguages.DefaultLanguages.English);
            }

            if (index >= Translations.Count)
            {
                Translations.Add(new Translation(language, translation));
            }
            else
            {
                Translations[index] = new Translation(language, translation);
            }
        }

        public override string ToString()
        {
            string defaultLanguage = GameLanguages.ToString(ModManager.CurrentMod.DefaultLanguage);

            foreach (var translation in Translations)
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
            foreach (var translation in Translations)
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
