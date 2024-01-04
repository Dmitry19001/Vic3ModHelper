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
using Vic3ModManager.Essentials;

namespace Vic3ModManager
{
    /// <summary>
    /// Interaction logic for AlbumEditorPanel.xaml
    /// </summary>
    public partial class AlbumEditorPanel : UserControl
    {
        public RoutedEventHandler? OnDataUpdated;

        public MusicAlbum currentAlbum;
        private BitmapImage defaultAlbumCover;

        public AlbumEditorPanel(MusicAlbum album)
        {
            InitializeComponent();

            currentAlbum = album;

            InitializeDefaultAlbumCover();
            LoadDataToUI();
        }

        private void LoadDataToUI()
        {
            AlbumTitleInput.Text = currentAlbum.Title;
            AlbumIdInput.Text = currentAlbum.Id;

            AlbumTitleInput.TextChanged += AlbumInput_TextChanged;
            AlbumIdInput.TextChanged += AlbumInput_TextChanged;

            UpdateCoverImage();

            if (currentAlbum.Songs.Count > 0)
            {
                foreach (Song song in currentAlbum.Songs)
                {
                    AddSongControl(song);
                }
            }
        }

        private void UpdateCoverImage()
        {
            //setting album cover image
            //need to check if path is not empty otherwise use default image
            if (!string.IsNullOrEmpty(currentAlbum.CoverImagePath))
            {
                if (currentAlbum.CoverImagePath.EndsWith(".dds"))
                    AlbumCoverImage.Source = ImageHelpers.BitmapImageFromDDS(currentAlbum.CoverImagePath);
                else
                    AlbumCoverImage.Source = new BitmapImage(new Uri(currentAlbum.CoverImagePath, UriKind.RelativeOrAbsolute));
            }
            else
            {
                AlbumCoverImage.Source = defaultAlbumCover;
            }
        }

        private void InitializeDefaultAlbumCover()
        {
            defaultAlbumCover = new BitmapImage();
            defaultAlbumCover.BeginInit();
            defaultAlbumCover.UriSource = new Uri("pack://application:,,,/vinyl-disc.png");
            defaultAlbumCover.CacheOption = BitmapCacheOption.OnLoad;
            defaultAlbumCover.EndInit();
        }

        private void RefreshAlbumClassData()
        {
            currentAlbum.Title = AlbumTitleInput.Text;
            //currentAlbum.Id = AlbumIdInput.Text;

            string id = StringHelpers.ReplaceSpaces(AlbumTitleInput.Text);
            id = StringHelpers.TransliterateCyrillicToLatin(id);
            currentAlbum.Id = id;

            //Debug.WriteLine($"Refreshed MusicAlbum: {currentAlbum}");
        }

        private void UpdateSongsEmptyLabel()
        {
            // hiding "Song list is empty" label
            if (currentAlbum.Songs.Count > 0)
            {
                SongsEmptyLabel.Visibility = Visibility.Hidden;
            }
            else
            {
                SongsEmptyLabel.Visibility |= Visibility.Visible;
            }

        }

        private void AddSongButton_Click(object sender, RoutedEventArgs e)
        {

            // calling file dialog
            Microsoft.Win32.OpenFileDialog openFileDialog = new()
            {
                Multiselect = true,
                Filter = "Audio files (*.ogg, *.mp3)|*.ogg;*.mp3"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                // getting song name from file
                string[] filePaths = openFileDialog.FileNames;
                foreach (string filePath in filePaths)
                {
                    string fileName = System.IO.Path.GetFileNameWithoutExtension(filePath);
                    // removing spaces from file name
                    fileName = StringHelpers.ReplaceSpaces(fileName);
                    // replacing non-latin characters from file name
                    fileName = StringHelpers.TransliterateCyrillicToLatin(fileName);
                    // getting song duration from file
                    TagLib.File file = TagLib.File.Create(filePath);
                    int duration = (int)file.Properties.Duration.TotalSeconds;
                    // adding song to current album
                    currentAlbum.AddSong(new Song(fileName, filePath, duration));
                    // adding song to UI
                    AddSongControl(currentAlbum.Songs.Last());
                }
                OnDataUpdated?.Invoke(this, new RoutedEventArgs());
            }
        }

        private void AddSongControl(Song song)
        {
            SongControl songControl = new()
            {
                Title = $"{song.Name}",
                Duration = $"{song.DurationToString()}"
            };

            songControl.DeleteButtonClick += SongDeleteOnClick;

            SongsContainer.Children.Add(songControl);
            
            UpdateSongsEmptyLabel();

            OnDataUpdated?.Invoke(this, new RoutedEventArgs());
        }

        private void SongDeleteOnClick(object sender, RoutedEventArgs e)
        {
            int songIndex = SongsContainer.Children.IndexOf((UIElement)sender);
            if (songIndex >= 0)
            {
                currentAlbum.RemoveSong(currentAlbum.Songs[songIndex]);
                UpdateSongsEmptyLabel();
                OnDataUpdated?.Invoke(this, new RoutedEventArgs());
            }
        }

        private void AlbumCoverImage_MouseDown(object sender, MouseButtonEventArgs e)
        {
            // Here is code to choose image from file
            // Supported formats: .png, .jpg, .jpeg, dds

            // calling file dialog
            Microsoft.Win32.OpenFileDialog openFileDialog = new()
            {
                Multiselect = false,
                Filter = "Image files (*.png, *.jpg, *.jpeg, *.dds)|*.png;*.jpg;*.jpeg;*.dds"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                // getting image path from file
                string imagePath = openFileDialog.FileName;
                // adding image to current album
                currentAlbum.CoverImagePath = imagePath;

                UpdateCoverImage();

                OnDataUpdated?.Invoke(this, new RoutedEventArgs());
            }
        }


        private void AlbumInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            RefreshAlbumClassData();
            OnDataUpdated?.Invoke(this, new RoutedEventArgs());
        }
    }
}
