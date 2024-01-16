using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using static Vic3ModManager.GameLanguages;
using System.Windows;


namespace Vic3ModManager.Essentials
{
    public class ModExporter
    {
        private readonly Encoding noBomUtf8 = new UTF8Encoding(false);
        private string modDirectory;
        public bool musicNeedsConversion;

        public ModExporter(string modDirectory)
        {
            this.modDirectory = modDirectory;

            CheckIfMusicNeedsConversion();
        }

        private void CheckIfMusicNeedsConversion()
        {
            musicNeedsConversion = false;

            foreach (MusicAlbum album in ModManager.CurrentMod.MusicAlbums)
            {
                foreach (Song song in album.Songs)
                {
                    if (System.IO.Path.GetExtension(song.OriginalPath) != ".ogg")
                    {
                        musicNeedsConversion = true;
                        return;
                    }
                }
            }
        }

        public void CopyMusicFiles(bool convertionIsAllowed)
        {
            AudioConverter audioConverter = new AudioConverter();

            if (convertionIsAllowed) audioConverter.FindFFMpeg();

            foreach (MusicAlbum album in ModManager.CurrentMod.MusicAlbums)
            {
                string albumTitle = StringHelpers.FormatString(album.Title.Key);

                foreach (Song song in album.Songs)
                {
                    string songName = StringHelpers.FormatString(song.Title.Key);

                    string destinationPath = Path.Combine(modDirectory, "music", albumTitle, $"{songName}.ogg");

                    if (Path.GetExtension(song.OriginalPath) != ".ogg")
                    {
                        if (convertionIsAllowed)
                        {
                            audioConverter.ConvertToOgg(song.OriginalPath, destinationPath);
                        }
                        continue;
                    }

                    File.Copy(song.OriginalPath, destinationPath, true);
                }
            }
        }

        public void CopyAlbumCovers()
        {
            // TODO: Add covers only if copied album contains songs

            foreach (MusicAlbum album in ModManager.CurrentMod.MusicAlbums)
            {
                if (string.IsNullOrEmpty(album.CoverImagePath))
                {
                    continue;
                }

                string albumId = StringHelpers.FormatString(album.Id);

                string destinationPath = Path.Combine(modDirectory, "gfx", "interface", "illustrations", "music_player", $"{albumId}.dds");

                if (Path.GetExtension(album.CoverImagePath) != ".dds")
                {
                    ImageConverters.ConvertToDDS(album.CoverImagePath, destinationPath);

                    continue;
                }

                File.Copy(album.CoverImagePath, destinationPath, true);
            }
        }

        public void CreateMusicFile()
        {
            // file name {mod_name}_music.txt
            // file content
            // for each album and song
            // {song_name} = {
            //     music = "file:/music/{album_title}/{song_name}.ogg"
            //     name = "{song_name}"
            //     pause_factor = {song_pause_factor}
            //     can_be_interrupted = {song_can_be_inerrupted? yes : no}
            //     trigger_prio_override = {song_trigger_prio_override? yes : no}
            //     mood = {song_mood? yes : no}
            // }

            string fileName = $"{StringHelpers.FormatString(ModManager.CurrentMod.Name)}_music.txt";
            string filePath = Path.Combine(modDirectory, "music", fileName);

            StringBuilder stringBuilder = new StringBuilder();

            foreach (MusicAlbum album in ModManager.CurrentMod.MusicAlbums)
            {
                string albumTitle = StringHelpers.FormatString(album.Title.Key);

                stringBuilder.AppendLine($"\n##### {albumTitle}");

                foreach (Song song in album.Songs)
                {
                    string songName = StringHelpers.FormatString(song.Title.Key);

                    stringBuilder.AppendLine($"{songName} = {{");
                    stringBuilder.AppendLine($"    music = \"file:/music/{albumTitle}/{songName}.ogg\"");
                    stringBuilder.AppendLine($"    name = \"{songName}\"");
                    stringBuilder.AppendLine($"    pause_factor = {song.PauseFactor}");
                    stringBuilder.AppendLine($"    can_be_interrupted = {(song.CanBeInterrupted ? "yes" : "no")}");
                    stringBuilder.AppendLine($"    trigger_prio_override = {(song.TriggerPrioOverride ? "yes" : "no")}");
                    stringBuilder.AppendLine($"    mood = {(song.Mood ? "yes" : "no")}");
                    stringBuilder.AppendLine($"}}");
                }
            }
            stringBuilder.AppendLine($"\n### GENERATED BY VIC3MODMANAGER ###");

            File.WriteAllText(filePath, stringBuilder.ToString());
        }

        // TODO: REMAKE WHOLE LOGIC FOR CREATING MUSIC PLAYER CATEGORIES FILE
        // TODO: CREATE MUSIC PLAYER CATEGORIES FILE AFTER MUSIC FILE IS COPIED TO AVOID ADDING SONGS THAT ARE NOT COPIED
        public void CreateMusicPlayerCategoriesFile()
        {
            // file name {mod_name}_categories.txt
            // file content
            // category = {
            //     id = "{album_id}"
            //     name = "{album_title}"
            //     tracks = {
            //         "{song_name}"
            //     }
            // }
            // repeat for each album

            string fileName = $"{StringHelpers.FormatString(ModManager.CurrentMod.Name)}_categories.txt";
            string filePath = Path.Combine(modDirectory, "music", "music_player_categories", fileName);

            StringBuilder stringBuilder = new StringBuilder();

            foreach (MusicAlbum album in ModManager.CurrentMod.MusicAlbums)
            {
                string albumTitle = StringHelpers.FormatString(album.Title.Key);

                stringBuilder.AppendLine($"category = {{");
                stringBuilder.AppendLine($"    id = \"{album.Id}\"");
                stringBuilder.AppendLine($"    name = \"{albumTitle}\"");
                stringBuilder.AppendLine($"    tracks = {{");

                foreach (Song song in album.Songs)
                {
                    string songName = StringHelpers.FormatString(song.Title.Key);

                    stringBuilder.AppendLine($"        \"{songName}\"");
                }

                stringBuilder.AppendLine($"    }}");
                stringBuilder.AppendLine($"}}");
            }
            stringBuilder.AppendLine($"\n### GENERATED BY VIC3MODMANAGER ###");
            File.WriteAllText(filePath, stringBuilder.ToString(), noBomUtf8);
        }


        public void CreateModMetaFile()
        {
            // file name metadata.json
            // file content
            // Warning: name can't contain spaces

            ModData modData = new ModData
            {
                name = ModManager.CurrentMod.Name.Replace(" ", ""),
                id = "",
                version = ModManager.CurrentMod.Version,
                supported_game_version = "",
                short_description = ModManager.CurrentMod.Description,
                tags = ["sound"],
                game_custom_data = new GameCustomData { multiplayer_synchronized = false }
            };

            string jsonString = JsonConvert.SerializeObject(modData, Formatting.Indented);
            string filePath = Path.Combine(modDirectory, ".metadata", "metadata.json");

            File.WriteAllText(filePath, jsonString, noBomUtf8);
        }

        public void CreateDirectories()
        {
            // Create the mod directory
            string modFolderName = StringHelpers.FormatString(ModManager.CurrentMod.Name);

            modDirectory = Path.Combine(modDirectory, modFolderName);
            Directory.CreateDirectory(modDirectory);

            // Needed directories: localization, music, gfx, .metadata, music/music_player_categories, gfx/interface/illustrations/music_player
            // Additional directories = foreach musicalbum in musicalbums: music/musicalbum
            List<string> directories = new List<string>
            {
                "localization",
                "music",
                "gfx",
                ".metadata",
                "music/music_player_categories",
                "gfx/interface/illustrations/music_player"
            };

            int albumCount = ModManager.CurrentMod.MusicAlbums.Count;

            // Adding music/{album_title} directories
            for (int x = 0; x < albumCount; x++)
            {
                MusicAlbum album = ModManager.CurrentMod.MusicAlbums[x];

                string albumTitle = StringHelpers.FormatString(album.Title.Key);

                directories.Add($"music/{albumTitle}");
            }

            // Adding localization/{language} directories
            // Using only language that are used in mod
            // Music album title should contain everything needed
            MusicAlbum mAlbum = ModManager.CurrentMod.MusicAlbums[0];
            foreach (Translation translation in mAlbum.Title.Translations)
            {
                directories.Add($"localization/{translation.Language.ToLowerInvariant()}");
            }


            // Create the directories if not exists
            foreach (string directory in directories)
            {
                Directory.CreateDirectory(Path.Combine(modDirectory, directory));
            }
        }

        public void CreateLocalizations()
        {
            // Create the localizations dir {mod}/localisation/{language}/{mod_name}_Music_l_{language}.yml

            // File content example:
            // l_{language}:
            //     {song_title_key}: "{song_title.{language}.translation}"

            Mod currentMod = ModManager.CurrentMod;

            StringBuilder stringBuilder = new();

            for (int i = 0; i < currentMod.MusicAlbums[0].Title.Translations.Count;  i++)
            {
                string language = currentMod.MusicAlbums[0].Title.Translations[i].Language;
                DefaultLanguages gameLanguage = ToDefaultLanguage(language);

                if (gameLanguage != DefaultLanguages.Custom)
                {
                    stringBuilder.AppendLine($"{gameLanguage.ToIngameId()}:");
                }
                else
                {
                    stringBuilder.AppendLine($"l_{language.ToLowerInvariant()}:");
                }

                for (int x = 0; x < currentMod.MusicAlbums.Count; x++)
                {
                    MusicAlbum album = currentMod.MusicAlbums[x];

                    stringBuilder.AppendLine($"  #{album.Title.Translations[i].Text}");
                    stringBuilder.AppendLine($"  {album.Title.Key}: \"{album.Title.Translations[i].Text}\"");

                    foreach (Song song in album.Songs)
                    {
                        stringBuilder.AppendLine($"  {song.Title.Key}: \"{song.Title.Translations[i].Text}\"");
                    }
                    stringBuilder.AppendLine();
                }
                // removing last \n
                stringBuilder.Append("### Generated by Vic3ModManager ###");

                string fileName = $"{StringHelpers.FormatString(currentMod.Name)}_Music_l_{language.ToLowerInvariant()}.yml";
                string filePath = Path.Combine(modDirectory, "localization", language.ToLowerInvariant(), fileName);

                try
                {
                    File.WriteAllText(filePath, stringBuilder.ToString(), noBomUtf8);
                } catch
                {
                    MessageBox.Show($"Unable to create file at: {filePath}");
                }

                stringBuilder.Clear();
            }

        }

        public void DeleteModFolder()
        {
            try
            {
                Directory.Delete(modDirectory, true);
            }
            catch (Exception)
            {
                
            }
        }
    }
}
