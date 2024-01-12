using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Vic3ModManager
{
    /// <summary>
    /// Interaction logic for MusicManager.xaml
    /// </summary>


    // Whole class needs to be refactored


    public partial class MusicManager : CustomPage
    {
        private MusicAlbum? currentAlbum = null;
        private MusicAlbumControl? currentAlbumControl = null;
        private AlbumEditorPanelControl? currentAlbumEditorPanelControl = null;

        public MusicManager()
        {
            InitializeComponent();

            
            InitializeAlbums();
        }

        private void InitializeAlbums()
        {
            if (ModManager.CurrentMod != null && ModManager.CurrentMod.MusicAlbums.Count > 0)
            {
                currentAlbum = ModManager.CurrentMod.MusicAlbums.First();

                foreach (MusicAlbum musicAlbum in ModManager.CurrentMod.MusicAlbums)
                {
                    AddMusicAlbumControl(musicAlbum);
                }

                currentAlbumControl = (MusicAlbumControl)AlbumsContainer.Children[1];
                RefreshAlbumPanelStates();
            }
            else
            {
                return;
            }
        }

        private void AddNewAlbum()
        {
            MusicAlbum newAlbum = new();
            ModManager.CurrentMod.MusicAlbums.Add(newAlbum);
            currentAlbumControl = AddMusicAlbumControl(newAlbum);

            currentAlbum = ModManager.CurrentMod.MusicAlbums.Last();
            RefreshAlbumPanelStates();
        }

        private MusicAlbumControl AddMusicAlbumControl(MusicAlbum musicAlbum)
        {
            var musicAlbumControl = new MusicAlbumControl();
            musicAlbumControl.Title = $"{musicAlbum.Title}";
            musicAlbumControl.AdditionalInfo = $"Songs: {musicAlbum.Songs.Count}";
            if (!string.IsNullOrEmpty(musicAlbum.CoverImagePath))
            {
                musicAlbumControl.AlbumImagePath = musicAlbum.CoverImagePath;
            }

            musicAlbumControl.AlbumClick += AlbumPanelOnClick;
            musicAlbumControl.AlbumOnDeleteClick += AlbumPanelOnDeleteClick;

            AlbumsContainer.Children.Add(musicAlbumControl);

            OpenAlbumEditPanel(musicAlbum);

            return musicAlbumControl;
        }

        private void OpenAlbumEditPanel(MusicAlbum musicAlbum)
        {
            currentAlbumEditorPanelControl = new AlbumEditorPanelControl(musicAlbum);
            currentAlbumEditorPanelControl.OnDataUpdated += OnDataUpdated;

            AlbumEditorFrame.Navigate(currentAlbumEditorPanelControl);
        }

        private void CloseAlbumEditPanel(bool forceRenavigate)
        {
            currentAlbumEditorPanelControl.OnDataUpdated -= OnDataUpdated;

            if (!forceRenavigate) return; 

            if (ModManager.CurrentMod.MusicAlbums.Count > 0)
            {
                currentAlbum = ModManager.CurrentMod.MusicAlbums.Last();
                currentAlbumControl = (MusicAlbumControl)AlbumsContainer.Children[AlbumsContainer.Children.Count - 1];

                OpenAlbumEditPanel(currentAlbum);
            }
            else
            {
                AlbumEditorFrame.Navigate(null);
            }
            
        }

        private void UpdateUI()
        {
            if (ModManager.CurrentMod is null) return;

            currentAlbumControl.Title = currentAlbum.Title.ToString();
            currentAlbumControl.AdditionalInfo = $"Songs: {currentAlbum.Songs.Count}";
            if (!string.IsNullOrEmpty(currentAlbum.CoverImagePath))
            {
                currentAlbumControl.AlbumImagePath = currentAlbum.CoverImagePath;
            }

            // hiding Create new album label
            if (ModManager.CurrentMod.MusicAlbums.Count > 0)
            {
                AlbumsEmptyNotice.Visibility = Visibility.Hidden;
            }
            else
            {
                AlbumsEmptyNotice.Visibility = Visibility.Visible;
            }
        }

        private void DeleteAlbum(int albumIndex)
        {
            ModManager.CurrentMod.MusicAlbums.RemoveAt(albumIndex);
            AlbumsContainer.Children.RemoveAt(albumIndex + 1);

            CloseAlbumEditPanel(true);

            RefreshAlbumPanelStates();
        }

        private void RefreshAlbumPanelStates()
        {
            if (ModManager.CurrentMod is null) return;

            for (int i = 0; i < ModManager.CurrentMod.MusicAlbums.Count; i++)
            {
                if (AlbumsContainer.Children[i + 1] is not MusicAlbumControl albumPanelBorder) continue;

                albumPanelBorder.AlbumIsSelected = (i == ModManager.CurrentMod.MusicAlbums.IndexOf(currentAlbum));
            }

            UpdateUI();
        }


        private void OnDataUpdated(object sender, RoutedEventArgs e)
        {
            UpdateUI();
        }

        private void AddAlbumButton_Click(object sender, RoutedEventArgs e)
        {
            AddNewAlbum();
        }

        private void AlbumPanelOnClick(object sender, RoutedEventArgs e)
        {
            int albumIndex = AlbumsContainer.Children.IndexOf((UIElement)sender) - 1;
            if (albumIndex >= 0)
            {
                currentAlbumControl = (MusicAlbumControl)sender;
                currentAlbum = ModManager.CurrentMod.MusicAlbums[albumIndex];

                CloseAlbumEditPanel(false);
                OpenAlbumEditPanel(currentAlbum);

                RefreshAlbumPanelStates();
            }
        }

        private void AlbumPanelOnDeleteClick(object sender, RoutedEventArgs e)
        {
            int albumIndex = AlbumsContainer.Children.IndexOf((UIElement)sender) - 1;
            if (albumIndex >= 0)
            {
                DeleteAlbum(albumIndex);
            }
        }
    }
}
