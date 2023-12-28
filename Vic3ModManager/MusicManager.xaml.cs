using System;
using System.Collections.Generic;
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

namespace Vic3ModManager
{
    /// <summary>
    /// Interaction logic for MusicManager.xaml
    /// </summary>
    public partial class MusicManager : Page
    {
        private MusicAlbum currentAlbum;
        private List<MusicAlbum> musicAlbums = new List<MusicAlbum>();

        private readonly SolidColorBrush defaultAlbumBorderBrush = (SolidColorBrush)new BrushConverter().ConvertFrom("#404040");

        public MusicManager()
        {
            InitializeComponent();

            musicAlbums.Add(new MusicAlbum("untitled", "untitled", null, ""));
            currentAlbum = musicAlbums[0];

            AddMusicAlbumControl(currentAlbum);

            RefreshAlbumPanelStates();
        }

        private void AddMusicAlbumControl(MusicAlbum musicAlbum)
        {
            var musicAlbumControl = new MusicAlbumControl();
            musicAlbumControl.Title = $"{musicAlbum.Title}";
            musicAlbumControl.AdditonalInfo = $"Songs: {musicAlbum.Songs.Count}";
            if (!string.IsNullOrEmpty(musicAlbum.CoverImagePath))
            {
                musicAlbumControl.AlbumImage = new BitmapImage(new Uri(musicAlbum.CoverImagePath, UriKind.RelativeOrAbsolute));
            }

            musicAlbumControl.AlbumClick += AlbumPanelOnClick;

            AlbumsContainer.Children.Add(musicAlbumControl);
        }

        private void AlbumPanelOnClick(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("button pressed huraay");
        }

        private void RefreshAlbumPanelStates()
        {
            for (int i = 0; i < musicAlbums.Count; i++)
            {
                Border? albumPanelBorder = AlbumsContainer.Children[i + 1] as Border;

                if (albumPanelBorder is not Border) continue;

                if (i == musicAlbums.IndexOf(currentAlbum))
                {
                    albumPanelBorder.BorderBrush = Brushes.LimeGreen;
                }
                else
                {
                    albumPanelBorder.BorderBrush = defaultAlbumBorderBrush;
                }
            }
        }
    }
}
