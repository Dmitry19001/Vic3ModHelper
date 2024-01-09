using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using Vic3ModManager.Essentials;

namespace Vic3ModManager
{
    /// <summary>
    /// Interaction logic for LocalizationManager.xaml
    /// </summary>
    public partial class LocalizationManager : Page
    {
        public LocalizationManager()
        {
            InitializeComponent();

            // Dynamically add language columns here
            //AddLanguageColumns(new List<string> { "English" }); // Example languages
            LoadMusicData(); // Load your data here
        }

        private void AddLanguageColumns(List<string> languages)
        {
            if (ModManager.CurrentMod.MusicAlbums.Count < 1) return;

            foreach (var language in languages)
            {
                var column = new DataGridTextColumn
                {
                    Header = language,
                    Binding = new Binding($"Translations[{language}]")
                };
                //LocalizationDataGrid.Columns.Add(column);
            }
        }

        private void LoadMusicData()
        {
            // Load your data into the DataGrid
            // Example: LocalizationDataGrid.ItemsSource = yourDataSource;

            if (ModManager.CurrentMod.MusicAlbums.Count < 1) return;

            List<LocalizableTextEntry> localizableTextEntries = new();

            string modName = StringHelpers.FormatString(ModManager.CurrentMod.Name);

            TextBlock textBlock = new()
            {
                Text = $"{modName}_Music",
                Style = (System.Windows.Style)FindResource("FileNameTextBlockStyle"),
                IsEnabled = false
            };

            LocFilesPanel.Children.Add(textBlock);

            for (int i = 0; i < ModManager.CurrentMod.MusicAlbums.Count; i++)
            {
                MusicAlbum musicAlbum = ModManager.CurrentMod.MusicAlbums[i];

                if (!musicAlbum.Title.Translations.ContainsKey("English"))
                    musicAlbum.Title.Translations.Add("English", $"{musicAlbum.Title.Key}");

                localizableTextEntries.Add(musicAlbum.Title);

                for (int j = 0; j < musicAlbum.Songs.Count; j++)
                {
                    Song song = musicAlbum.Songs[j];

                    if (!song.Title.Translations.ContainsKey("English"))
                        song.Title.Translations.Add("English", $"{song.Title.Key}");

                    localizableTextEntries.Add(song.Title);
                }
            }

            LocalizationEditor.LocalizationData = [.. localizableTextEntries];

            //LocalizationDataGrid.ItemsSource = localizableTextEntries;
        }

        private void LocalizationDataGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            if (e.Row.DataContext is LocalizableTextEntry entry)
            {
                e.Row.Header = entry.Key;
            }
        }

        private void FileNameTextBlock_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            TextBlock fileNameBlock = (TextBlock)sender;

            if (fileNameBlock.IsEnabled)
            {
                fileNameBlock.Background = Brushes.DarkGray;
            }
        }
        private void FileNameTextBlock_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            TextBlock fileNameBlock = (TextBlock)sender;

            if (fileNameBlock.IsEnabled)
            {
                fileNameBlock.Background = Brushes.Transparent;
            }
        }
    }
}
