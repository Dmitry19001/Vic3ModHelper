using System;
using System.Collections.Generic;

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

        public static Mod? CurrentMod { get; private set; }

        public static List<Mod> AllMods { get; private set; } = new();
    }

}
