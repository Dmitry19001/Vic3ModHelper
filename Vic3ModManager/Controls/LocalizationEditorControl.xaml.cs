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
    /// Interaction logic for LocalizationEditorControl.xaml
    /// </summary>
    public partial class LocalizationEditorControl : UserControl
    {
        public static readonly DependencyProperty LocalizationDataProperty = DependencyProperty.Register("LocalizationData", typeof(LocalizableTextEntry[]), typeof(LocalizationEditorControl), new PropertyMetadata(null));

        Dictionary<string, StackPanel> TranslationsColumns = new();

        public LocalizableTextEntry[] LocalizationData
        {
            get { return (LocalizableTextEntry[])GetValue(LocalizationDataProperty); }
            set { 
                SetValue(LocalizationDataProperty, value);
                InitializeLocalizationData();
            }
        }

        private void InitializeLocalizationData()
        {
            if (LocalizationData == null) return;

            for (int i = 0; i < LocalizationData.Length; i++)
            {
                AddNewKey(LocalizationData[i].Key);
            }

            foreach (string key in LocalizationData[0].Translations.Keys)
            {
                AddNewLanguage(key);
            }
        }

        public LocalizationEditorControl()
        {
            InitializeComponent();
        }

        private void AddLanguage_Click(object sender, RoutedEventArgs e)
        {
            AddNewLanguage();
        }

        private void AddNewKey(string text)
        {
            TextBlock keyBlock = new()
            {
                Text = text
            };

            KeysListPanel.Children.Add(keyBlock);
        }

        private void AddNewLanguage(string langTitle = null)
        {
            // Dynamically add a new language column
            ColumnDefinition newColumn = new ColumnDefinition();
            newColumn.Width = new GridLength(1, GridUnitType.Star);
            MainGrid.ColumnDefinitions.Insert(MainGrid.ColumnDefinitions.Count - 1, newColumn);

            // Add grid splitter (Style is in xaml)
            // To every row
            for (int i = 0; i < MainGrid.RowDefinitions.Count; i++)
            {
                GridSplitter gridSplitter = new GridSplitter();
                Grid.SetRow(gridSplitter, i);
                Grid.SetColumn(gridSplitter, MainGrid.ColumnDefinitions.Count - 2);
                MainGrid.Children.Add(gridSplitter);
            }

            // Create a TextBox for the language title in Row 0
            TextBox languageTitleTextBox = new()
            {
                Text = langTitle?? "New Language"
            };

            Grid.SetRow(languageTitleTextBox, 0);
            Grid.SetColumn(languageTitleTextBox, MainGrid.ColumnDefinitions.Count - 2);
            MainGrid.Children.Add(languageTitleTextBox);

            // Create StackPanel for Textboxes in Row 1
            StackPanel translationsPanel = new();
            Grid.SetRow(translationsPanel, 1);
            Grid.SetColumn(translationsPanel, MainGrid.ColumnDefinitions.Count - 2);
            MainGrid.Children.Add(translationsPanel);

            // Create TextBoxes for data in stackPanel
            for (int i = 1; i <  LocalizationData.Length; i++)
            {
                string defaultText = LocalizationData[i].Translations.ContainsKey(langTitle ?? "") ? LocalizationData[i].Translations[langTitle] : LocalizationData[i].Key;

                TextBox dataTextBox = new()
                {
                    Text = defaultText
                };
                Grid.SetRow(dataTextBox, i);
                Grid.SetColumn(dataTextBox, MainGrid.ColumnDefinitions.Count - 2);
                translationsPanel.Children.Add(dataTextBox);
            }

            // Move Add button to last column
            Grid.SetColumn(AddLanguageButton, MainGrid.ColumnDefinitions.Count - 1);
        }
    }
}
