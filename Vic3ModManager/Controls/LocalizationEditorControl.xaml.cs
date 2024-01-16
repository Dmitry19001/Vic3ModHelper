using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;


namespace Vic3ModManager
{
    /// <summary>
    /// Interaction logic for LocalizationEditorControl.xaml
    /// </summary>

    public partial class LocalizationEditorControl : UserControl
    {
        public static readonly DependencyProperty LocalizationDataProperty = DependencyProperty.Register("LocalizationData", typeof(List<LocalizableTextEntry>), typeof(LocalizationEditorControl), new PropertyMetadata(null));
        private Style addButtonStyle;

        // TODO event OnDataChanged with arg bool isLanguageDeleted
        public event RoutedEventHandler? OnDataChanged;


        public List<LocalizableTextEntry> LocalizationData
        {
            get { return (List<LocalizableTextEntry>)GetValue(LocalizationDataProperty); }
            set { 
                SetValue(LocalizationDataProperty, value);
                InitializeLocalizationData();
            }
        }

        private void InitializeLocalizationData()
        {
            if (LocalizationData == null) return;

            for (int i = 0; i < LocalizationData.Count; i++)
            {
                AddNewKey(LocalizationData[i].Key);
            }

            // Generating columns for each language + 1 for placeholder
            for (int x = 0; x < LocalizationData[0].Translations.Count + 1; x++)
            {
                if (x == LocalizationData[0].Translations.Count)
                {
                    // Adding placeholder column
                    AddNewLanguage("New language", x, true);
                    continue;
                }

                Translation translation = LocalizationData[0].Translations[x];
                AddNewLanguage(translation.Language, x);
            }

        }

        public LocalizationEditorControl()
        {
            InitializeComponent();
            addButtonStyle = (Style)FindResource("AddButtonStyle");
        }

        private void AddNewKey(string text)
        {
            TextBlock keyBlock = new()
            {
                Text = text
            };

            KeysListPanel.Children.Add(keyBlock);
        }

        private void AddNewLanguage(string langTitle, int translationsId, bool isPlaceHolder = false)
        {
            // adding to main grid LocalizationColumnControl
                     
            LocalizationColumnControl localizationColumnControl = new(
                langTitle,
                [.. LocalizationData],
                translationsId,
                isPlaceHolder);

            if (!isPlaceHolder)
            {
                localizationColumnControl.OnDataChanged += OnColumnDataChanged;
                localizationColumnControl.OnDataDeleted += OnColumnDataDeleted;
            }
            else
            {
                localizationColumnControl.OnAddNewColumnClicked += OnAddNewColumnHandler;
            }

            MainGrid.Children.Add(localizationColumnControl);

            // Scroll to end 
            Dispatcher.InvokeAsync(() =>
            {
                MainScrollViewer.ScrollToRightEnd();
            }, DispatcherPriority.Background);
        }

        private void OnAddNewColumnHandler(object sender, RoutedEventArgs e)
        {
            // Setting up events for last column before adding new
            LocalizationColumnControl lastColumn = (LocalizationColumnControl)MainGrid.Children[MainGrid.Children.Count - 1];
            lastColumn.OnDataChanged += OnColumnDataChanged;
            lastColumn.OnDataDeleted += OnColumnDataDeleted;
            lastColumn.OnAddNewColumnClicked -= OnAddNewColumnHandler;

            // Adding new placeholder column
            AddNewLanguage("New language", lastColumn.TranslationId + 1, true);
        }

        private void OnColumnDataDeleted(object sender, RoutedEventArgs e)
        {
            // Deleteting translations from all keys
            LocalizationColumnControl localizationColumnControl = (LocalizationColumnControl)sender;
            LocalizationData.RemoveAt(localizationColumnControl.TranslationId);

            // Reload current page
            OnDataChanged?.Invoke(this, new LocalizationDataChangedEventArgs(null, true));
        }

        private void OnColumnDataChanged(object sender, RoutedEventArgs e)
        {
            OnDataChanged?.Invoke(this, new LocalizationDataChangedEventArgs(null, false));
        }
    }
}
