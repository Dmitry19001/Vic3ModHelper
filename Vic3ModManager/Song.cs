using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vic3ModManager
{
    /// <summary>
    /// Represents a song with properties defining its behavior in the mod manager.
    /// </summary>
    public class Song
    {
        private string? name;
        private string? originalPath;

        private int pauseFactor;
        private bool canBeInterrupted;
        private bool triggerPrioOverride;
        private bool mood;

        /// <summary>
        /// Initializes a new instance of the <see cref="Song"/> class.
        /// </summary>
        /// <param name="name">The name of the song.</param>
        /// <param name="originalPath">The original file path of the song.</param>
        /// <param name="pauseFactor">The pause factor of the song.</param>
        /// <param name="canBeInterrupted">Indicates if the song can be interrupted.</param>
        /// <param name="triggerPrioOverride">Indicates if the song can trigger priority override.</param>
        /// <param name="mood">Indicates the mood of the song.</param>

        public Song(string? name,
                    string? originalPath,
                    int pauseFactor = 75,
                    bool canBeInterrupted = false,
                    bool triggerPrioOverride = false,
                    bool mood = true)
        {
            this.Name = name;
            this.OriginalPath = originalPath;

            this.PauseFactor = pauseFactor;
            this.CanBeInterrupted = canBeInterrupted;
            this.TriggerPrioOverride = triggerPrioOverride;
            this.Mood = mood;
        }

        public string? Name { get => name; set => name = value; }
        public string? OriginalPath { get => originalPath; set => originalPath = value; }
        public int PauseFactor { get => pauseFactor; set => pauseFactor = value; }
        public bool CanBeInterrupted { get => canBeInterrupted; set => canBeInterrupted = value; }
        public bool TriggerPrioOverride { get => triggerPrioOverride; set => triggerPrioOverride = value; }
        public bool Mood { get => mood; set => mood = value; }
    }
}
