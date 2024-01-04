﻿using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Vic3ModManager.Essentials;

namespace Vic3ModManager
{
    /// <summary>
    /// Interaction logic for MusicManager.xaml
    /// </summary>
    public partial class MusicManagerPage : Page
    {
        private MusicAlbum? currentAlbum = null;
        private MusicAlbumControl? currentAlbumControl = null;
        private AlbumEditorPanel? currentAlbumEditorPanel = null;

        public MusicManagerPage()
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

        private void AddAlbumButton_Click(object sender, RoutedEventArgs e)
        {
            var newAlbum = new MusicAlbum("untitled", "untitled", null, "");
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
            currentAlbumEditorPanel = new AlbumEditorPanel(musicAlbum);
            currentAlbumEditorPanel.OnDataUpdated += UpdateData;

            AlbumEditorFrame.Navigate(currentAlbumEditorPanel);
        }

        private void CloseAlbumEditPanel(bool forceRenavigate)
        {
            currentAlbumEditorPanel.OnDataUpdated -= UpdateData;

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

        private void UpdateData(object sender, RoutedEventArgs e)
        {
            currentAlbumControl.Title = $"{currentAlbum.Title}";
            currentAlbumControl.AdditionalInfo = $"Songs: {currentAlbum.Songs.Count}";
            if (!string.IsNullOrEmpty(currentAlbum.CoverImagePath))
            {
                currentAlbumControl.AlbumImagePath = currentAlbum.CoverImagePath;
            }
        }

        private void UpdateUI()
        {
            if (ModManager.CurrentMod is null) return;

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

        private void AlbumPanelOnClick(object sender, RoutedEventArgs e)
        {
            int albumIndex = AlbumsContainer.Children.IndexOf((UIElement)sender) - 1;
            if (albumIndex >= 0)
            {
                currentAlbumControl = (MusicAlbumControl)sender;
                currentAlbum = ModManager.CurrentMod.MusicAlbums[albumIndex];
                
                Debug.WriteLine($"Album {currentAlbum.Title} selected with ID: {currentAlbum.Id}");

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
                ModManager.CurrentMod.MusicAlbums.RemoveAt(albumIndex);
                AlbumsContainer.Children.RemoveAt(albumIndex + 1);

                CloseAlbumEditPanel(true);

                RefreshAlbumPanelStates();
            }
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
    }
}