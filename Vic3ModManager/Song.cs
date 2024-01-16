using Newtonsoft.Json;
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
        private LocalizableTextEntry? title;
        private string? originalPath;
        private int duration = 0;
        private int pauseFactor;
        private bool canBeInterrupted;
        private bool triggerPrioOverride;
        private bool mood;

        /// <summary>
        /// Initializes a new instance of the <see cref="Song"/> class.
        /// </summary>
        /// <param name="title">The title of the song.</param>
        /// <param name="originalPath">The original file path of the song.</param>
        /// <param name="duration">The duration of the song.</param>
        /// <param name="pauseFactor">The pause factor of the song.</param>
        /// <param name="canBeInterrupted">Indicates if the song can be interrupted.</param>
        /// <param name="triggerPrioOverride">Indicates if the song can trigger priority override.</param>
        /// <param name="mood">
        /// Indicates whether the song is associated with a specific mood or condition in the game.
        /// When set to true (yes), the song might be played under certain conditions or trigger specific reactions.
        /// The exact mechanics of how this influences the game's behavior are not fully understood.
        /// </param>

        public Song(LocalizableTextEntry? title,
            string? originalPath,
            int duration = 0,
            int pauseFactor = 75,
            bool canBeInterrupted = false,
            bool triggerPrioOverride = false,
            bool mood = true)
        {
            this.Title = title ?? new LocalizableTextEntry(string.Empty);
            this.OriginalPath = originalPath;
            this.Duration = duration;

            this.PauseFactor = pauseFactor;
            this.CanBeInterrupted = canBeInterrupted;
            this.TriggerPrioOverride = triggerPrioOverride;
            this.Mood = mood;
        }

        public LocalizableTextEntry? Title { get => title; set => title = value; }
        public string? OriginalPath { get => originalPath; set => originalPath = value; }
        public int PauseFactor { get => pauseFactor; set => pauseFactor = value; }
        public bool CanBeInterrupted { get => canBeInterrupted; set => canBeInterrupted = value; }
        public bool TriggerPrioOverride { get => triggerPrioOverride; set => triggerPrioOverride = value; }
        public bool Mood { get => mood; set => mood = value; }
        public int Duration { get => duration; set => duration = value; }
        
        public string DurationToString()
        {
            int minutes = Duration / 60;
            int seconds = Duration % 60;

            return $"{minutes}:{(seconds < 10 ? "0" : "")}{seconds}";
        }
    }
}
