using System;
using System.Windows;
using System.Windows.Controls;
using Vic3ModManager.Essentials;
using Vic3ModManager.Windows;
using Ookii.Dialogs.Wpf;

namespace Vic3ModManager
{
    /// <summary>
    /// Interaction logic for Export.xaml
    /// </summary>
    public partial class Export : CustomPage
    {
        private string modDirectory;
        private bool exportIsCanceled = false;

        public Export()
        {
            InitializeComponent();

            InitilizeModFolder();
        }

        private void InitilizeModFolder()
        {
            // Set the default export path
            ExportPathTextBox.Text = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Paradox Interactive", "Victoria 3", "mod");

            // Scroll to the end of the textbox
            // Need dispatcher because the textbox is not rendered yet
            Dispatcher.BeginInvoke(new Action(() => ExportPathTextBox.ScrollToEnd()));
        }

        private void BrowseDirectoryButton_Click(object sender, RoutedEventArgs e)
        {
            // Show the folder browser dialog wpf

            var dialog = new VistaFolderBrowserDialog();
            if (dialog.ShowDialog() == true)
            {
                string selectedPath = dialog.SelectedPath;
                ExportPathTextBox.Text = selectedPath;
            }
        }

        private void BeginExportButton_Click(object sender, RoutedEventArgs e)
        {
            exportIsCanceled = false;

            ModExporter modExporter = new(ExportPathTextBox.Text);

            // Currently only music mod is generated

            modExporter.CreateDirectories();

            // Create the mod meta file dir {mod}/.metadata
            modExporter.CreateModMetaFile();

            if (ModManager.CurrentMod.MusicAlbums.Count > 0)
            {
                // Create the music player categories file dir {mod}/music/music_player_categories/{mod_name}_categories.txt
                modExporter.CreateMusicPlayerCategoriesFile();

                // Copy the music files dir {mod}/music/{album}/{song_name}.ogg
                bool isConversionAllowed = IsMusicConversionAllowed(modExporter.musicNeedsConversion);

                if (exportIsCanceled) {
                    modExporter.DeleteModFolder();

                    MessageBox.Show("Export canceled!");

                    return;
                }

                modExporter.CopyMusicFiles(isConversionAllowed);

                // Create the music file dir {mod}/music/{mod_name}_music.txt
                modExporter.CreateMusicFile();

                // Copy the album covers dir {mod}/gfx/interface/illustrations/music_player/{album_title}.dds
                modExporter.CopyAlbumCovers();

                // Create the localizations dir {mod}/localisation/{language}/{mod_name}_Music_l_{language}.yml
                modExporter.CreateLocalizations();

                MessageBox.Show("Export complete!");
            }
        }

        private bool IsMusicConversionAllowed(bool musicNeedsConversion)
        {
            // TODO: Ask user to download ffmpeg if not found
            // TODO: Options for download: Auto or Manual (if user don't trust author approach)
            if (musicNeedsConversion && AppConfig.Instance.AskForConversionConfirm)
            {
                var customMessageBox = new CustomMessageBox("External .exe needed",
                                            "Warning! Some music needs conversion to .ogg. "
                                            + "FFMPEG can be used to perform the conversion. "
                                            + "FFMPEG is executable that is developed by other developers, so it will be used for your own risk. "
                                            + "Do you want to continue?");

                customMessageBox.ShowDialog();

                switch (customMessageBox.MessageBoxResult)
                {
                    case CustomMessageBoxResult.YesDontAskAgain:
                        AppConfig.Instance.AskForConversionConfirm = false;
                        AppConfig.Instance.Save();
                        break;
                    case CustomMessageBoxResult.Yes:
                        break;
                    case CustomMessageBoxResult.ContinueWithoutConversion:
                        return false;
                    case CustomMessageBoxResult.CancelExport:
                        exportIsCanceled = true;
                        return false;
                }
            }
            return true;
        }
        
    }
}
