using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Vic3ModManager
{
    /// <summary>
    /// Interaction logic for MusicAlbumControl.xaml
    /// </summary>
    public partial class MusicAlbumControl : UserControl
    {
        public event RoutedEventHandler AlbumClick;

        private static readonly ImageSource DefaultAlbumImage = new BitmapImage(new Uri("pack://application:,,,/vinyl-disc.png"));

        public static readonly DependencyProperty AlbumTitleProperty =
            DependencyProperty.Register("Title", typeof(string), typeof(MusicAlbumControl), new PropertyMetadata("Untitled"));

        public static readonly DependencyProperty AlbumSongCountProperty =
            DependencyProperty.Register("AdditonalInfo", typeof(string), typeof(MusicAlbumControl), new PropertyMetadata("Songs: 0"));

        public static readonly DependencyProperty AlbumImageProperty =
            DependencyProperty.Register("AlbumImage", typeof(ImageSource), typeof(MusicAlbumControl), new PropertyMetadata(DefaultAlbumImage));


        public MusicAlbumControl()
        {
            InitializeComponent();

            this.MouseDown += OnAlbumMouseDown;
        }

        // Internal event handler that raises the AlbumClick event
        private void OnAlbumMouseDown(object sender, MouseButtonEventArgs e)
        {
            // Check if there are any subscribers to the event
            AlbumClick?.Invoke(this, new RoutedEventArgs());
        }

        public string Title
        {
            get { return (string)GetValue(AlbumTitleProperty); }
            set { SetValue(AlbumTitleProperty, value); }
        }

        public string AdditonalInfo
        {
            get { return (string)GetValue(AlbumSongCountProperty); }
            set { SetValue(AlbumSongCountProperty, value); }
        }

        public ImageSource AlbumImage
        {
            get { return (ImageSource)GetValue(AlbumImageProperty); }
            set { SetValue(AlbumImageProperty, value); }
        }
    }
}
