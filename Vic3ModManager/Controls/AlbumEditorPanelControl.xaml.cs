using Microsoft.Win32;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Vic3ModManager.Essentials;

namespace Vic3ModManager
{
    /// <summary>
    /// Interaction logic for AlbumEditorPanel.xaml
    /// </summary>
    public partial class AlbumEditorPanelControl : UserControl
    {
        public RoutedEventHandler? OnDataUpdated;

        public MusicAlbum currentAlbum;
        private BitmapImage defaultAlbumCover;

        public AlbumEditorPanelControl(MusicAlbum album)
        {
            InitializeComponent();

            currentAlbum = album;

            InitializeDefaultAlbumCover();
            LoadDataToUI();
        }

        private void LoadDataToUI()
        {
            string defaultLanguage = GameLanguages.ToString(ModManager.CurrentMod.DefaultLanguage);

            AlbumTitleInput.Text = currentAlbum.Title.ToString();
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
            defaultAlbumCover.UriSource = new Uri("pack://application:,,,/Icons/vinyl-disc.png");
            defaultAlbumCover.CacheOption = BitmapCacheOption.OnLoad;
            defaultAlbumCover.EndInit();
        }

        private void RefreshAlbumClassData()
        {
            string id = StringHelpers.FormatString(AlbumTitleInput.Text);
            currentAlbum.Id = id;

            currentAlbum.Title.Key = id;

            currentAlbum.Title.SetTranslation(AlbumTitleInput.Text);
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

        private void UpdateSongData(object sender)
        {
            // getting song index
            int songIndex = SongsContainer.Children.IndexOf((UIElement)sender);

            if (songIndex < 0)
            {
                return;
            }

            // getting song control
            SongControl songControl = (SongControl)sender;
            // getting song from album
            Song song = currentAlbum.Songs[songIndex];
            // updating song name
            song.Title.SetTranslation(songControl.Title);

            currentAlbum.Songs[songIndex] = song;
        }

        private void AddSong(OpenFileDialog openFileDialog)
        {
            // getting song name from file
            string[] filePaths = openFileDialog.FileNames;
            foreach (string filePath in filePaths)
            {
                string fileName = System.IO.Path.GetFileNameWithoutExtension(filePath);

                fileName = StringHelpers.FormatString(fileName);

                // getting song duration from file
                TagLib.File file = TagLib.File.Create(filePath);
                int duration = (int)file.Properties.Duration.TotalSeconds;
                // adding song to current album
                currentAlbum.AddSong(new Song(new LocalizableTextEntry(fileName), filePath, duration));
                // adding song to UI
                AddSongControl(currentAlbum.Songs.Last());
            }
            OnDataUpdated?.Invoke(this, new RoutedEventArgs());
        }

        private void AddSongControl(Song song)
        {
            string fileExtension = System.IO.Path.GetExtension(song.OriginalPath).Remove(0,1);
            SongControl songControl = new(song.Title.ToString(), song.DurationToString(), fileExtension);

            songControl.OnSongEdited += SongControl_OnSongEdited;
            songControl.DeleteButtonClick += SongDeleteOnClick;

            SongsContainer.Children.Add(songControl);
            
            UpdateSongsEmptyLabel();

            OnDataUpdated?.Invoke(this, new RoutedEventArgs());
        }

        private void DeleteSong(int songIndex)
        {
            if (songIndex < 0)
            {
                return;
            }
            currentAlbum.RemoveSongAt(songIndex);
            SongsContainer.Children.RemoveAt(songIndex);
            UpdateSongsEmptyLabel();
            OnDataUpdated?.Invoke(this, new RoutedEventArgs());
        }

        private void AddSongButton_Click(object sender, RoutedEventArgs e)
        {
            // calling file dialog
            OpenFileDialog openFileDialog = new()
            {
                Multiselect = true,
                Filter = "Audio files (*.ogg, *.mp3)|*.ogg;*.mp3"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                AddSong(openFileDialog);
            }
        }

        private void SongControl_OnSongEdited(object sender, RoutedEventArgs e)
        {
            UpdateSongData(sender);
        }

        private void SongDeleteOnClick(object sender, RoutedEventArgs e)
        {
            int songIndex = SongsContainer.Children.IndexOf((UIElement)sender);
            DeleteSong(songIndex);
        }

        private void AlbumCoverImage_MouseDown(object sender, MouseButtonEventArgs e)
        {
            // Here is code to choose image from file
            // Supported formats: .png, .jpg, .jpeg, .dds

            // calling file dialog
            OpenFileDialog openFileDialog = new()
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
