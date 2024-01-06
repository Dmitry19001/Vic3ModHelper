using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Linq;
using Vic3ModManager.Essentials;

namespace Vic3ModManager
{
    /// <summary>
    /// Interaction logic for ExportPage.xaml
    /// </summary>
    public partial class ExportPage : Page
    {
        private string modDirectory;

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

        private void BeginExportButton_Click(object sender, RoutedEventArgs e)
        {
            ModExporter modExporter = new ModExporter(ExportPathTextBox.Text);

            // Currently only music mod is generated

            modExporter.CreateDirectories();

            // Create the mod meta file dir {mod}/.metadata
            modExporter.CreateModMetaFile();

            if (ModManager.CurrentMod.MusicAlbums.Count > 0)
            {
                // Create the music player categories file dir {mod}/music/music_player_categories/{mod_name}_categories.txt
                modExporter.CreateMusicPlayerCategoriesFile();

                // Create the music file dir {mod}/music/{mod_name}_music.txt
                modExporter.CreateMusicFile();

                // Copy the music files dir {mod}/music/{album}/{song_name}.ogg
                modExporter.CopyMusicFiles();

                // Copy the album covers dir {mod}/gfx/interface/illustrations/music_player/{album_title}.dds
                modExporter.CopyAlbumCovers();

                MessageBox.Show("Export complete!");
            }
        }

        
    }
}
