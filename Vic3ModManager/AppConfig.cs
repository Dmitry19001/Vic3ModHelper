using Newtonsoft.Json;
using System.IO;

namespace Vic3ModManager
{
    public class AppConfig
    {
        private static AppConfig _instance;

        public string ConfigVersion { get; set; } = "0.1";
        public bool AskForConversionConfirm { get; set; } = true;

        private const string configPath = "./config.json";

        // Private constructor to prevent external instantiation
        private AppConfig() { }

        // Public static method to get the instance
        public static AppConfig Instance
        {
            get
            {
                // Load the config when accessed for the first time
                _instance ??= Load();
                return _instance;
            }
        }

        // Load method as before
        private static AppConfig Load()
        {
            if (!File.Exists(configPath))
            {
                return new AppConfig(); // Return default config if file doesn't exist
            }

            string json = File.ReadAllText(configPath);
            AppConfig config = JsonConvert.DeserializeObject<AppConfig>(json) ?? new AppConfig(); // Return default config if deserialization fails

            // TODO: Version check and migration logic will be added here 

            return config;
        }

        // Save method as before
        public void Save()
        {
            string json = JsonConvert.SerializeObject(this, Formatting.Indented);
            File.WriteAllText(configPath, json);
        }
    }

}
