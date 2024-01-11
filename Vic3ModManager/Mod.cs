using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Vic3ModManager
{
    /// <summary>
    /// Represents a mod.
    /// </summary>


    public class Mod
    {
        public const string MOD_STUCTURE_ITERATION = "3";

        private string name;
        private string description;
        private string version;
        private List<MusicAlbum> musicAlbums;
        private GameLanguages.DefaultLanguages defaultLanguage;

        /// <summary>
        /// Initializes a new instance of the <see cref="Mod"/> class.
        /// </summary>
        /// <param name="name">The name of the mod.</param>
        /// <param name="description">The description of the mod.</param>
        /// <param name="musicAlbums">The music albums of the mod.</param>
        /// <param name="version">The version of the mod.</param>
        
        public Mod(string name,
                   string description,
                   string version,
                   MusicAlbum[]? musicAlbums = null,
                   GameLanguages.DefaultLanguages? defaultLanguage = null)
        {
            Name = name;
            Description = description;
            Version = version;
            MusicAlbums = new List<MusicAlbum>(musicAlbums ?? []);
            DefaultLanguage = defaultLanguage ?? GameLanguages.DefaultLanguages.English;
        }

        public string ModStructureIteration { get; set; } = MOD_STUCTURE_ITERATION;

        public string Name
        {
            get => name;
            set => name = value ?? throw new ArgumentNullException(nameof(value));
        }
        public string Description {
            get => description;
            set => description = value ?? string.Empty;
        }
        public string Version {
            get => version;
            set => version = value ?? "1.0";
        }
        public List<MusicAlbum> MusicAlbums {
            get => musicAlbums;
            set => musicAlbums = value;
        }

        public GameLanguages.DefaultLanguages DefaultLanguage
        {
            get => defaultLanguage;
            set => defaultLanguage = value;
        }

        public override string ToString()
        {
            return $"Name: {Name}, Description: {Description}, Version: {Version}";
        }
    }
}
