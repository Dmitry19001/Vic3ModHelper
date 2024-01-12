using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace Vic3ModManager
{
    /// <summary>
    /// Interaction logic for SettingsOverlay.xaml
    /// </summary>
    public partial class SettingsOverlay : UserControl
    {
        public SettingsOverlay()
        {
            InitializeComponent();
            IsEnabledChanged += SettingsOverlay_IsEnabledChanged;

            InitializeLanguageSelector();
            GetSettingsFromConfig();
        }

        private void GetSettingsFromConfig()
        {
            int languageIndex = (int)AppConfig.Instance.ModDefaultLanguage;

            LanguageComboBox.SelectedIndex = languageIndex;

            AskForConversionConfirm.IsChecked = AppConfig.Instance.AskForConversionConfirm;
            AutoSaveSettings.IsChecked = AppConfig.Instance.AutoSaveIsEnabled;
        }

        private void InitializeLanguageSelector()
        {
            // Loading all GameLanguages.DefaultLanguages string values
            GameLanguages.DefaultLanguages[] languages = (GameLanguages.DefaultLanguages[])Enum.GetValues(typeof(GameLanguages.DefaultLanguages));
            foreach (var language in languages)
            {
                LanguageComboBox.Items.Add(GameLanguages.ToString(language));
            }
        }

        private void SettingsOverlay_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (!(bool)e.OldValue)
            {
                Visibility = Visibility.Visible;
            }

            var storyboard = IsEnabled ? (Storyboard)Resources["FadeInStoryboard"] : (Storyboard)Resources["FadeOutStoryboard"];
            storyboard.Begin(this);
        }

        private void Animation_Completed(object sender, EventArgs e)
        {
            if (!IsEnabled)
            {
                Visibility = Visibility.Collapsed;
            }
        }
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            IsEnabled = false;
        }

        private void LanguageComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            bool settingAutosaveIsEnabled = AutoSaveSettings.IsChecked ?? false;

            if (e.AddedItems.Count > 0 && (AppConfig.Instance.AutoSaveIsEnabled || settingAutosaveIsEnabled))
            {
                SaveSettings();
            }
        }

        private void SaveSettings()
        {
            GameLanguages.DefaultLanguages language = (GameLanguages.DefaultLanguages)LanguageComboBox.SelectedIndex;
            
            AppConfig.Instance.ModDefaultLanguage = language;
            AppConfig.Instance.AutoSaveIsEnabled = AutoSaveSettings.IsChecked ?? false;
            AppConfig.Instance.AskForConversionConfirm = AskForConversionConfirm.IsChecked ?? false;

            AppConfig.Instance.Save();
        }

        private void CheckBox_click(object sender, RoutedEventArgs e)
        {
            bool settingAutosaveIsEnabled = AutoSaveSettings.IsChecked ?? false;

            if (AppConfig.Instance.AutoSaveIsEnabled || settingAutosaveIsEnabled)
            {
                SaveSettings();
            }
        }
    }
}
