using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text;


namespace Vic3ModManager.Essentials
{
    public class ModExporter
    {
        private string modDirectory;

        public ModExporter(string modDirectory)
        {
            this.modDirectory = modDirectory;
        }


        public void CopyMusicFiles()
        {
            // Doing simple copy for now
            // TODO: Add support for converting to .ogg
            // Skipping mp3, only ogg is supported

            foreach (MusicAlbum album in ModManager.CurrentMod.MusicAlbums)
            {
                string albumTitle = StringHelpers.ReplaceSpaces(album.Title);
                albumTitle = StringHelpers.TransliterateCyrillicToLatin(albumTitle);

                foreach (Song song in album.Songs)
                {
                    if (System.IO.Path.GetExtension(song.OriginalPath) != ".ogg")
                    {
                        continue;
                    }

                    string songName = StringHelpers.ReplaceSpaces(song.Title);
                    songName = StringHelpers.TransliterateCyrillicToLatin(songName);

                    string destinationPath = System.IO.Path.Combine(modDirectory, "music", albumTitle, $"{songName}.ogg");

                    System.IO.File.Copy(song.OriginalPath, destinationPath, true);
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

                string albumId = StringHelpers.ReplaceSpaces(album.Id);
                albumId = StringHelpers.TransliterateCyrillicToLatin(albumId);

                string destinationPath = System.IO.Path.Combine(modDirectory, "gfx", "interface", "illustrations", "music_player", $"{albumId}.dds");

                if (System.IO.Path.GetExtension(album.CoverImagePath) != ".dds")
                {
                    ImageConverters.ConvertToDDS(album.CoverImagePath, destinationPath);

                    continue;
                }

                System.IO.File.Copy(album.CoverImagePath, destinationPath, true);
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

            string fileName = $"{StringHelpers.ReplaceSpaces(ModManager.CurrentMod.Name)}_music.txt";
            string filePath = System.IO.Path.Combine(modDirectory, "music", fileName);

            StringBuilder stringBuilder = new StringBuilder();

            foreach (MusicAlbum album in ModManager.CurrentMod.MusicAlbums)
            {
                string albumTitle = StringHelpers.ReplaceSpaces(album.Title);
                albumTitle = StringHelpers.TransliterateCyrillicToLatin(albumTitle);

                stringBuilder.AppendLine($"\n##### {albumTitle}");

                foreach (Song song in album.Songs)
                {
                    string songName = StringHelpers.ReplaceSpaces(song.Title);
                    songName = StringHelpers.TransliterateCyrillicToLatin(songName);

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

            System.IO.File.WriteAllText(filePath, stringBuilder.ToString());
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

            string fileName = $"{StringHelpers.ReplaceSpaces(ModManager.CurrentMod.Name)}_categories.txt";
            string filePath = System.IO.Path.Combine(modDirectory, "music", "music_player_categories", fileName);

            StringBuilder stringBuilder = new StringBuilder();

            foreach (MusicAlbum album in ModManager.CurrentMod.MusicAlbums)
            {
                string albumTitle = StringHelpers.ReplaceSpaces(album.Title);
                albumTitle = StringHelpers.TransliterateCyrillicToLatin(albumTitle);

                stringBuilder.AppendLine($"category = {{");
                stringBuilder.AppendLine($"    id = \"{album.Id}\"");
                stringBuilder.AppendLine($"    name = \"{albumTitle}\"");
                stringBuilder.AppendLine($"    tracks = {{");

                foreach (Song song in album.Songs)
                {
                    string songName = StringHelpers.ReplaceSpaces(song.Title);
                    songName = StringHelpers.TransliterateCyrillicToLatin(songName);

                    stringBuilder.AppendLine($"        \"{songName}\"");
                }

                stringBuilder.AppendLine($"    }}");
                stringBuilder.AppendLine($"}}");
            }

            System.IO.File.WriteAllText(filePath, stringBuilder.ToString(), Encoding.UTF8);
        }


        public void CreateModMetaFile()
        {
            // file name metadata.json
            // file content

            ModData modData = new ModData
            {
                name = ModManager.CurrentMod.Name,
                id = "",
                version = ModManager.CurrentMod.Version,
                supported_game_version = "",
                short_description = ModManager.CurrentMod.Description,
                tags = new List<string> { "sound" },
                game_custom_data = new GameCustomData { multiplayer_synchronized = false }
            };

            string jsonString = JsonConvert.SerializeObject(modData, Formatting.Indented);
            string filePath = System.IO.Path.Combine(modDirectory, ".metadata", "metadata.json");

            System.IO.File.WriteAllText(filePath, jsonString, Encoding.UTF8);
        }

        public void CreateDirectories()
        {
            // Create the mod directory
            string modFolderName = StringHelpers.ReplaceSpaces(ModManager.CurrentMod.Name);
            modFolderName = StringHelpers.TransliterateCyrillicToLatin(modFolderName);

            modDirectory = System.IO.Path.Combine(modDirectory, modFolderName);
            System.IO.Directory.CreateDirectory(modDirectory);

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

            for (int x = 0; x < albumCount; x++)
            {
                MusicAlbum album = ModManager.CurrentMod.MusicAlbums[x];

                string albumTitle = StringHelpers.ReplaceSpaces(album.Title);
                albumTitle = StringHelpers.TransliterateCyrillicToLatin(albumTitle);

                directories.Add($"music/{albumTitle}");
            }


            // Create the directories if not exists
            foreach (string directory in directories)
            {
                System.IO.Directory.CreateDirectory(System.IO.Path.Combine(modDirectory, directory));
            }
        }
    }
}
