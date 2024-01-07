using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using System.Windows;
using Vic3ModManager.Essentials;
using System.Text.Json.Serialization;

namespace Vic3ModManager
{
    public static class ModManager
    {
        public static void AddMod(Mod mod)
        {
            AllMods.Add(mod);
        }

        public static void RemoveMod(Mod mod)
        {
            AllMods.Remove(mod);
        }

        public static void SwitchMod(Mod mod)
        {
            if (AllMods.Contains(mod))
            {
                CurrentMod = mod;
            }
        }

        public static string[] GetAvailableMods()
        {
            if (!Directory.Exists("./Mods"))
            {
                return [];
            }

            string[] mods = Directory.GetFiles("./Mods", "*.json");

            for (int i = 0; i < mods.Length; i++)
            {
                mods[i] = Path.GetFileNameWithoutExtension(mods[i]);
            }

            return mods;
        }

        public static void SaveCurrentMod()
        {
            if (!Directory.Exists("./Mods"))
            {
                Directory.CreateDirectory("./Mods");
            }

            string fileName = StringHelpers.FormatString(CurrentMod.Name);

            TextWriter textWriter = new StreamWriter($"./Mods/{fileName}.json");
            JsonSerializer jsonSerializer = new();

            jsonSerializer.Serialize(textWriter, CurrentMod, typeof(Mod));
            //File.WriteAllText($"./Mods/{fileName}.json", currentModJson, Encoding.UTF8);
        }

        public static void LoadMod(string fileName)
        {
            string filePath = Path.Combine("./Mods", $"{fileName}.json");
            JsonReader jsonReader = new JsonTextReader(new StreamReader(filePath));

            JsonSerializer jsonSerializer = new JsonSerializer();

            Mod loadedMod = jsonSerializer.Deserialize<Mod>(jsonReader);

            if (loadedMod != null)
            {
                AddMod(loadedMod);
                SwitchMod(loadedMod);
            }
            else
            {
                MessageBox.Show("Unable to load mod project, file is possibly corrupted!");
            }
        }   

        public static Mod? CurrentMod { get; private set; }

        public static List<Mod> AllMods { get; private set; } = new();
    }

}
