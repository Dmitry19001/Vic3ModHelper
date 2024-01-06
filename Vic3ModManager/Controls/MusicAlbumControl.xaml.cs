using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Vic3ModManager.Essentials;

namespace Vic3ModManager
{
    /// <summary>
    /// Interaction logic for MusicAlbumControl.xaml
    /// </summary>
    public partial class MusicAlbumControl : UserControl
    {
        public event RoutedEventHandler AlbumClick;

        public event RoutedEventHandler AlbumOnDeleteClick;
        
        private event RoutedEventHandler AlbumImagePathChanged;

        public static readonly ImageSource DefaultAlbumImage = new BitmapImage(new Uri("pack://application:,,,/Icons/vinyl-disc.png"));

        public static readonly DependencyProperty AlbumTitleProperty =
            DependencyProperty.Register("Title", typeof(string), typeof(MusicAlbumControl), new PropertyMetadata("Untitled"));

        public static readonly DependencyProperty AlbumSongCountProperty =
            DependencyProperty.Register("AdditionalInfo", typeof(string), typeof(MusicAlbumControl), new PropertyMetadata("Songs: 0"));

        public static readonly DependencyProperty AlbumImageProperty =
            DependencyProperty.Register("AlbumImage", typeof(ImageSource), typeof(MusicAlbumControl), new PropertyMetadata(DefaultAlbumImage));

        public static readonly DependencyProperty AlbumIsSelectedProperty =
            DependencyProperty.Register("AlbumIsSelected", typeof(bool), typeof(MusicAlbumControl), new PropertyMetadata(false));

        public static readonly DependencyProperty AlbumImagePathProperty = 
            DependencyProperty.Register("AlbumImagePath", typeof(string), typeof(MusicAlbumControl), new PropertyMetadata("pack://application:,,,/Icons/vinyl-disc.png"));

        public MusicAlbumControl()
        {
            InitializeComponent();

            this.MouseDown += OnAlbumMouseDown;
            this.MouseEnter += OnAlbumMouseEnter;
            this.MouseLeave += OnAlbumMouseLeave;
            this.AlbumImagePathChanged += OnAlbumImagePathChanged;
        }

        private void OnAlbumImagePathChanged(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(AlbumImagePath))
            {
                AlbumImage = DefaultAlbumImage;
                return;
            }
            // Handling .dds files
            if (System.IO.Path.GetExtension(AlbumImagePath) == ".dds")
            {
                AlbumImage = ImageHelpers.BitmapImageFromDDS(AlbumImagePath) ?? DefaultAlbumImage;
            }
            else // Handling .png and other image files
            {
                AlbumImage = new BitmapImage(new Uri(AlbumImagePath, UriKind.RelativeOrAbsolute)) ?? DefaultAlbumImage;
            }
        }

        private void OnAlbumMouseLeave(object sender, MouseEventArgs e)
        {
            if (!AlbumIsSelected)
            {
                AlbumControlBorder.BorderBrush = new BrushConverter().ConvertFrom("#404040") as SolidColorBrush;           
            }
            this.Cursor = Cursors.Arrow;
        }

        private void OnAlbumMouseEnter(object sender, MouseEventArgs e)
        {
            if (!AlbumIsSelected)
            {
                AlbumControlBorder.BorderBrush = new BrushConverter().ConvertFrom("#ffffff") as SolidColorBrush;
                this.Cursor = Cursors.Hand;
            }
        }

        // Internal event handler that raises the AlbumClick event
        private void OnAlbumMouseDown(object sender, MouseButtonEventArgs e)
        {
            // Check if there are any subscribers to the event
            if (!AlbumIsSelected)
            {
                AlbumClick?.Invoke(this, new RoutedEventArgs());
            }
        }

        public string Title
        {
            get { return (string)GetValue(AlbumTitleProperty); }
            set { SetValue(AlbumTitleProperty, value); }
        }

        public string AdditionalInfo
        {
            get { return (string)GetValue(AlbumSongCountProperty); }
            set { SetValue(AlbumSongCountProperty, value); }
        }

        public ImageSource AlbumImage
        {
            get { return (ImageSource)GetValue(AlbumImageProperty); }
            private set { SetValue(AlbumImageProperty, value); }
        }

        public String AlbumImagePath
        {
            get { return (string)GetValue(AlbumImagePathProperty); }
            set { 
                SetValue(AlbumImagePathProperty, value); 
                AlbumImagePathChanged?.Invoke(this, new RoutedEventArgs()); 
            }
        }

        public bool AlbumIsSelected
        {
            get { return (bool)GetValue(AlbumIsSelectedProperty); }
            set { 
                SetValue(AlbumIsSelectedProperty, value); 
                if (value)
                {
                    AlbumControlBorder.BorderBrush = new BrushConverter().ConvertFrom("#48f542") as SolidColorBrush;
                }
                else
                {
                    AlbumControlBorder.BorderBrush = new BrushConverter().ConvertFrom("#404040") as SolidColorBrush;
                }
            }
        }

        private void DeleteAlbumButton_MouseDown(object sender, MouseButtonEventArgs e)
        {
            AlbumOnDeleteClick?.Invoke(this, new RoutedEventArgs());
        }
    }
}
