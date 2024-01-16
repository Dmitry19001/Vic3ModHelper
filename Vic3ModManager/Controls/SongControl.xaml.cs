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
    /// Interaction logic for SongControl.xaml
    /// </summary>
    public partial class SongControl : UserControl
    {
        public bool IsEditing { get; set; } = false;
        public event RoutedEventHandler DeleteButtonClick;
        public event RoutedEventHandler OnSongEdited;

        private static readonly ImageSource editIcon = new BitmapImage(new Uri("pack://application:,,,/Icons/edit.png"));
        private static readonly ImageSource okIcon = new BitmapImage(new Uri("pack://application:,,,/Icons/check.png"));
        private static readonly ImageSource removeIcon = new BitmapImage(new Uri("pack://application:,,,/Icons/recycle-bin.png"));
        private static readonly ImageSource cancelIcon = new BitmapImage(new Uri("pack://application:,,,/Icons/close.png"));

        public static readonly DependencyProperty SongTitleProperty =
            DependencyProperty.Register("Title", typeof(string), typeof(SongControl), new PropertyMetadata("Untitled"));

        public static readonly DependencyProperty SongDurationProperty =
            DependencyProperty.Register("Duration", typeof(string), typeof(SongControl), new PropertyMetadata("0:00"));

        // need song extension property which is always uppercase chars
        public static readonly DependencyProperty SongExtensionProperty =
            DependencyProperty.Register("SongExtension", typeof(string), typeof(SongControl), new PropertyMetadata("OGG"));

        public SongControl(string title, string duration, string songExtension)
        {
            InitializeComponent();

            Title = title;
            Duration = duration;
            SongExtension = songExtension;
        }
        private void EditButton_Click(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton != MouseButtonState.Pressed) return;

            if (IsEditing)
            {
                Title = SongNameInput.Text;
                OnSongEdited?.Invoke(this, new RoutedEventArgs());
            }

            IsEditing = !IsEditing;

            SwitchEditMode();
        }

        private void SwitchEditMode()
        {
            Image editButtonImage = (Image)EditButton.Child;
            Image deleteButtonImage = (Image)DeleteButton.Child;

            editButtonImage.Source = IsEditing? okIcon : editIcon;
            deleteButtonImage.Source = IsEditing? cancelIcon : removeIcon;

            SongNameInput.IsReadOnly = !IsEditing;
        }

        private void DeleteButton_Click(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton != MouseButtonState.Pressed) return;

            if (IsEditing)
            {
                IsEditing = false;

                SwitchEditMode();
            }
            else
            {
                DeleteButtonClick?.Invoke(this, new RoutedEventArgs());
            }
        }

        private void DeleteButton_MouseEnter(object sender, MouseEventArgs e)
        {
            Border? border = sender as Border;
            if (border != null)
            {
                // Change to a lighter color on hover
                border.Background = new SolidColorBrush(Colors.LightGray);
            }
        }

        private void DeleteButton_MouseLeave(object sender, MouseEventArgs e)
        {
            Border? border = sender as Border;
            if (border != null)
            {
                // Revert to the original color when not hovered
                border.Background = new SolidColorBrush(Color.FromRgb(0x4C, 0x4C, 0x4C));
            }
        }

        public string Title
        {
            get { return (string)GetValue(SongTitleProperty); }
            set { SetValue(SongTitleProperty, value); }
        }

        public string Duration
        {
            get { return (string)GetValue(SongDurationProperty); }
            set { SetValue(SongDurationProperty, value); }
        }

        public string SongExtension
        {
            get { return (string)GetValue(SongExtensionProperty); }
            set
            {
                SetValue(SongExtensionProperty, value.ToUpper());
                InformUserFormat(value);
            }
        }

        private void InformUserFormat(string value)
        {
            if (value.ToUpper() == "MP3")
            {
                SongExtensionLabel.Foreground = Brushes.Yellow;
                SongExtensionLabel.ToolTip = "MP3 files are converted to OGG on mod export.";
            }
            else if (value.ToUpper() == "OGG")
            {
                SongExtensionLabel.Foreground = Brushes.LimeGreen;
                SongExtensionLabel.ToolTip = "OGG is supported.";
            }
            else
            {
                SongExtensionLabel.Foreground = Brushes.Red;
                SongExtensionLabel.ToolTip = "This format is not supported.";
            }
        }
    }
}
