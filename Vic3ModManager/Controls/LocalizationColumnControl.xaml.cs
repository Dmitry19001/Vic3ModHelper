using System;
using System.Windows;
using System.Windows.Controls;

namespace Vic3ModManager
{
    /// <summary>
    /// Interaction logic for LocalizationColumnControl.xaml
    /// </summary>
    public partial class LocalizationColumnControl : UserControl
    {
        //dependency property for localization title LanguageTitle
        public RoutedEventHandler? OnDataChanged;
        public static readonly DependencyProperty LanguageTitleProperty = DependencyProperty.Register(
            "LanguageTitle", typeof(string), typeof(LocalizationColumnControl), new PropertyMetadata(string.Empty, OnTextChanged));

        public string LanguageTitle
        {
            get { return (string)GetValue(LanguageTitleProperty); }
            set { SetValue(LanguageTitleProperty, value); }
        }
        public LocalizableTextEntry[] LocalizationValues { get; private set; } = [];

        public LocalizationColumnControl(string title, LocalizableTextEntry[] values)
        {
            InitializeComponent();

            LanguageTitle = title;
            LocalizationValues = [.. values];

            GenerateLayout();
        }

        private void GenerateLayout()
        {
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

        private void TextChanged_handler(object sender, TextChangedEventArgs e)
        {
            if (LocalizationValues == null) return;

            TextBox textBox = (TextBox)sender;
            int i = TranslationsList.Children.IndexOf(textBox);

            LocalizableTextEntry localizableTextEntry = LocalizationValues[i + 1];

            localizableTextEntry.SetTranslation(textBox.Text, LanguageTitle);
            OnDataChanged?.Invoke(this, new RoutedEventArgs());
        }
        private static void OnTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is LocalizationColumnControl control && e.NewValue is string newText)
            {
                control.LanguageTitleBox.Text = newText;
            }
        }
    }
}
