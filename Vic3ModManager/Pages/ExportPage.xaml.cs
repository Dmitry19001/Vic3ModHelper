using System;
using System.Windows;
using System.Windows.Controls;
using Vic3ModManager.Essentials;
using Vic3ModManager.Windows;
using Ookii.Dialogs.Wpf;

namespace Vic3ModManager
{
    /// <summary>
    /// Interaction logic for ExportPage.xaml
    /// </summary>
    public partial class ExportPage : Page
    {
        private string modDirectory;
        private bool exportIsCanceled = false;

        public ExportPage()
        {
            InitializeComponent();

            InitilizeModFolder();
        }

        private void InitilizeModFolder()
        {
            // Set the default export path
            ExportPathTextBox.Text = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Paradox Interactive", "Victoria 3", "mod");
            
            // Scroll to the end of the textbox
            ExportPathTextBox.CaretIndex = ExportPathTextBox.Text.Length;
            ExportPathTextBox.ScrollToEnd();
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

                MessageBox.Show("Export complete!");
            }
        }

        private bool IsMusicConversionAllowed(bool musicNeedsConversion)
        {
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
