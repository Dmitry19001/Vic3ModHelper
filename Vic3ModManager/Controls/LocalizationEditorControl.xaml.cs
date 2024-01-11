using System;
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
        public static readonly DependencyProperty LocalizationDataProperty = DependencyProperty.Register("LocalizationData", typeof(LocalizableTextEntry[]), typeof(LocalizationEditorControl), new PropertyMetadata(null));
        private Style addButtonStyle;
        //readonly List<LocalizationColumn> TranslationsColumns = [];

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
            addButtonStyle = (Style)FindResource("AddButtonStyle");
        }

        private void AddLanguage_Click(object sender, RoutedEventArgs e)
        {
            AddNewLanguage("New language");
        }

        private void AddNewKey(string text)
        {
            TextBlock keyBlock = new()
            {
                Text = text
            };

            KeysListPanel.Children.Add(keyBlock);
        }

        private void AddNewLanguage(string langTitle)
        {
            // CODE IS SHIT
            // TODO REMAKE

            // adding to main grid LocalizationColumnControl
            
            // Deleting old add button if exists by simple math
            if (MainGrid.Children.Count > 1)
            {
                Button addbutton = (Button)MainGrid.Children[MainGrid.Children.Count - 1];
                addbutton.Click -= AddLanguage_Click;

                MainGrid.Children.RemoveAt(MainGrid.Children.Count - 1);
            }            

            LocalizationColumnControl localizationColumnControl = new(
                langTitle,
                LocalizationData);

            localizationColumnControl.OnDataChanged += OnColumnDataChanged;

            MainGrid.Children.Add(localizationColumnControl);

            // creating new add button
            // <Button x:Name="AddLanguageButton" Style="{StaticResource AddButtonStyle}" Content="+" Click="AddLanguage_Click"/>

            Button button = new()
            {
                Style = addButtonStyle,
                Content = "+"
            };
            button.Click += AddLanguage_Click;

            MainGrid.Children.Add(button);

            // Scroll to end 
            Dispatcher.InvokeAsync(() =>
            {
                MainScrollViewer.ScrollToRightEnd();
            }, DispatcherPriority.Background);
        }

        private void OnColumnDataChanged(object sender, RoutedEventArgs e)
        {
            LocalizationColumnControl localizationColumnControl = (LocalizationColumnControl)sender;
            Debug.WriteLine($"Data from {localizationColumnControl.LanguageTitle}");
            Debug.WriteLine($"ID: {localizationColumnControl.LocalizationValues[0].Key}");

            string keys = String.Join(",", localizationColumnControl.LocalizationValues[0].Translations.Keys);
            string values = String.Join(",", localizationColumnControl.LocalizationValues[0].Translations.Values);

            Debug.WriteLine($"Langs: {keys}");
            Debug.WriteLine($"Values: {values}");
        }
    }
}
