using System.Collections.Generic;

namespace Vic3ModManager
{
    public class LocalizableTextEntry
    {
        public string Key { get; set; }
        public Dictionary<string, string> Translations { get; set; }

        public LocalizableTextEntry(string key)
        {
            Key = key;
            Translations = [];
        }

        public override string ToString()
        {
            return Key;
        }
    }
}
