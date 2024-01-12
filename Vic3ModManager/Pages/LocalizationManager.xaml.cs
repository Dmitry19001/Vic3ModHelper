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
    public partial class LocalizationManager : CustomPage
    {
        public LocalizationManager()
        {
            InitializeComponent();

            LoadMusicData(); // Load your data here
        }

        private void LoadMusicData()
        {

            if (ModManager.CurrentMod.MusicAlbums.Count < 1) return;

            List<LocalizableTextEntry> localizableTextEntries = new();

            CreateLocalizationFileEntry();

            for (int i = 0; i < ModManager.CurrentMod.MusicAlbums.Count; i++)
            {
                MusicAlbum musicAlbum = ModManager.CurrentMod.MusicAlbums[i];

                localizableTextEntries.Add(musicAlbum.Title);

                for (int j = 0; j < musicAlbum.Songs.Count; j++)
                {
                    Song song = musicAlbum.Songs[j];

                    localizableTextEntries.Add(song.Title);
                }
            }

            LocalizationEditor.LocalizationData = [.. localizableTextEntries];

            //LocalizationDataGrid.ItemsSource = localizableTextEntries;
        }

        private void CreateLocalizationFileEntry()
        {
            string modName = StringHelpers.FormatString(ModManager.CurrentMod.Name);

            TextBlock textBlock = new()
            {
                Text = $"{modName}_Music",
                Style = (System.Windows.Style)FindResource("FileNameTextBlockStyle"),
                IsEnabled = false
            };

            LocFilesPanel.Children.Add(textBlock);
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
