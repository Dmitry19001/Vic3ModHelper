using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Windows.Input;
using TagLib.Ape;

namespace Vic3ModManager
{
    /// <summary>
    /// Interaction logic for LocalizationColumnControl.xaml
    /// </summary>
    public partial class LocalizationColumnControl : UserControl
    {
        public RoutedEventHandler? OnDataChanged;
        public RoutedEventHandler? OnDataDeleted;
        public RoutedEventHandler? OnAddNewColumnClicked;

        public static readonly DependencyProperty LanguageTitleProperty = DependencyProperty.Register(
            "LanguageTitle", typeof(string), typeof(LocalizationColumnControl), new PropertyMetadata(string.Empty, OnTextChanged));


        private static readonly ImageSource editIcon = new BitmapImage(new Uri("pack://application:,,,/Icons/edit.png"));
        private static readonly ImageSource okIcon = new BitmapImage(new Uri("pack://application:,,,/Icons/check.png"));
        private static readonly ImageSource removeIcon = new BitmapImage(new Uri("pack://application:,,,/Icons/recycle-bin.png"));
        private static readonly ImageSource cancelIcon = new BitmapImage(new Uri("pack://application:,,,/Icons/close.png"));

        public string LanguageTitle
        {
            get { return (string)GetValue(LanguageTitleProperty); }
            set { SetValue(LanguageTitleProperty, value); }
        }

        public LocalizableTextEntry[] LocalizationValues { get; private set; } = [];
        public int TranslationId { get; set; }

        public bool IsPlaceholder { get; set; }

        private bool isEditLangMode = false;


        public LocalizationColumnControl(string title, LocalizableTextEntry[] values, int translationId, bool isPlaceholder = false)
        {
            InitializeComponent();

            LanguageTitle = title;
            LocalizationValues = [.. values];
            TranslationId = translationId;
            IsPlaceholder = isPlaceholder;

            if (!isPlaceholder)
            {
                GenerateLayout();
            }
            else
            {
                AddNewColumnButton.Visibility = Visibility.Visible;
            }
        }


        private void GenerateLayout()
        {
            AddNewColumnButton.Visibility = Visibility.Collapsed;

            for (int i = 0; i < LocalizationValues.Length; i++)
            {
                TextBox textBox = new()
                {
                    Text = LocalizationValues[i].ToStringByLanguage(LanguageTitle)
                };

                textBox.TextChanged += TextChanged_handler;

                TranslationsList.Children.Add(textBox);
            }
        }


        private void SwitchEditMode()
        {
            isEditLangMode = !isEditLangMode;

            Image editButtonImage = (Image)EditLanguageTitleButton.Child;
            Image deleteButtonImage = (Image)DeleteButton.Child;

            editButtonImage.Source = isEditLangMode ? okIcon : editIcon;
            deleteButtonImage.Source = isEditLangMode ? cancelIcon : removeIcon;

            LanguageTitleBox.IsReadOnly = !isEditLangMode;

            if (isEditLangMode)
            {
                LanguageTitleBox.Focus();
            }
        }

        private void TextChanged_handler(object sender, TextChangedEventArgs e)
        {
            if (LocalizationValues == null || LocalizationValues.Length == 0) return;

            TextBox textBox = (TextBox)sender;
            int i = TranslationsList.Children.IndexOf(textBox);

            SaveChanges(textBox.Text, i);
        }

        private void ChangeLanguage()
        {
            for (int i = 0; i < LocalizationValues.Length; i++)
            {
                TextBox textBox = (TextBox)TranslationsList.Children[i];

                // Rewriting each translation with new language title
                SaveChanges(textBox.Text, i);
            }
        }

        private void SaveChanges(string input, int i)
        {
            LocalizableTextEntry localizableTextEntry = LocalizationValues[i];

            localizableTextEntry.SetTranslation(input, LanguageTitle, TranslationId);

            OnDataChanged?.Invoke(this, new RoutedEventArgs());
        }

        private static void OnTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is LocalizationColumnControl control && e.NewValue is string newText)
            {
                control.LanguageTitleBox.Text = newText;
            }
        }

        private void DeleteButton_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton != MouseButtonState.Pressed) return;

            if (isEditLangMode)
            {
                SwitchEditMode();
            }
            else
            {
                OnDataDeleted?.Invoke(this, new RoutedEventArgs());
            }
        }

        private void EditLanguageTitleButton_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton != MouseButtonState.Pressed) return;

            if (isEditLangMode)
            {
                LanguageTitle = LanguageTitleBox.Text;
                SwitchEditMode();

                ChangeLanguage();

                OnDataChanged?.Invoke(this, new RoutedEventArgs());
            }
            else
            {
                SwitchEditMode();
            }
        }

        private void Button_MouseEnter(object sender, MouseEventArgs e)
        {
            if (sender is Border border)
            {
                // Revert to the original color when not hovered
                border.Background = new SolidColorBrush(Colors.LightGray);
            }
        }

        private void Button_MouseLeave(object sender, MouseEventArgs e)
        {
            if (sender is Border border)
            {
                // Revert to the original color when not hovered
                border.Background = new SolidColorBrush(Color.FromRgb(0x4C, 0x4C, 0x4C));
            }
        }

        private void AddNewColumnButton_Click(object sender, RoutedEventArgs e)
        {
            GenerateLayout();

            SwitchEditMode();

            OnAddNewColumnClicked?.Invoke(this, new RoutedEventArgs());
        }
    }
}
