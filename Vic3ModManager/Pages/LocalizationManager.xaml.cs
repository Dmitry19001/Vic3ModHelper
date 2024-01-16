using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.ConstrainedExecution;
using System.Windows;
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
        private Style fileNameBlockDefaultStyle;
        private Style fileNameBlockActiveStyle;

        public LocalizationManager()
        {
            InitializeComponent();

            InitializeStyles(); 
            LoadMusicData();
            LocalizationEditor.OnDataChanged += DataChangedHandler;
        }

        private void InitializeStyles()
        {
            fileNameBlockActiveStyle = (Style)FindResource("FileNameTextBlockActiveStyle");
            fileNameBlockDefaultStyle = (Style)FindResource("FileNameTextBlockStyle");
        }

        private void LoadMusicData()
        {
            if (ModManager.CurrentMod == null) return;
            if (ModManager.CurrentMod.MusicAlbums.Count < 1) return;

            CreateLocalizationFileEntry();

            List<LocalizableTextEntry> localizableTextEntries = CreateLocalizationData();

            LocalizationEditor.LocalizationData = localizableTextEntries;
        }

        private List<LocalizableTextEntry> CreateLocalizationData()
        {
            List<LocalizableTextEntry> localizableTextEntries = [];

            for (int i = 0; i < ModManager.CurrentMod.MusicAlbums.Count; i++)
            {
                MusicAlbum musicAlbum = ModManager.CurrentMod.MusicAlbums[i];

                if (musicAlbum.Title.Translations.Count < 1)
                {
                    musicAlbum.Title.SetTranslation(musicAlbum.Title.Key);
                }

                localizableTextEntries.Add(musicAlbum.Title);

                for (int j = 0; j < musicAlbum.Songs.Count; j++)
                {
                    Song song = musicAlbum.Songs[j];

                    if (song.Title.Translations.Count < 1)
                    {
                        song.Title.SetTranslation(song.Title.Key);
                    }

                    localizableTextEntries.Add(song.Title);
                }
            }

            return localizableTextEntries;
        }

        private void CreateLocalizationFileEntry()
        {
            string modName = StringHelpers.FormatString(ModManager.CurrentMod.Name);

            TextBlock textBlock = new()
            {
                Text = $"{modName}_Music",
                Style = fileNameBlockActiveStyle,
                ToolTip = $"File name on export: {modName}_Music.txt"
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

            if (fileNameBlock.Style != fileNameBlockActiveStyle)
            {
                fileNameBlock.Background = Brushes.DarkGray;
            }
        }
        private void FileNameTextBlock_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            TextBlock fileNameBlock = (TextBlock)sender;

            if (fileNameBlock.Style != fileNameBlockActiveStyle)
            {
                fileNameBlock.Background = Brushes.Transparent;
            }
        }

        private void DataChangedHandler(object sender, RoutedEventArgs e)
        {
            if (e is not LocalizationDataChangedEventArgs args){ return; }

            if (args.IsLanguageDeleted)
            {
                OnRequestPageChange(nameof(LocalizationManager));
            }
            else
            {

            }
        }
    }
}
