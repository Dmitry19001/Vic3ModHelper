using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Vic3ModManager
{
    /// <summary>
    /// Interaction logic for Home.xaml
    /// </summary>
    public partial class Home : CustomPage
    {
        public Home()
        {
            InitializeComponent();

            InitializeDefaultLanguageComboBox();

            ShowCurrentModData();

            FindPreviousMods();
        }

        private void InitializeDefaultLanguageComboBox()
        {
            // Loading all GameLanguages.DefaultLanguages string values 
            GameLanguages.DefaultLanguages[] languages = (GameLanguages.DefaultLanguages[])Enum.GetValues(typeof(GameLanguages.DefaultLanguages));

            // Adding all GameLanguages.DefaultLanguages string values to the combobox
            for (int i = 0; i < languages.Length; i++)
            {
                DefaultLangugeSelector.Items.Add(GameLanguages.ToString(languages[i]));
            }

            DefaultLangugeSelector.SelectedIndex = 1;
        }

        private void ShowCurrentModData()
        {
            UpdateModDataButton.Visibility = Visibility.Collapsed;

            if (ModManager.CurrentMod != null)
            {
                ModName.Text = ModManager.CurrentMod.Name;
                ModDescription.Text = ModManager.CurrentMod.Description;
                ModVersion.Text = ModManager.CurrentMod.Version;
                DefaultLangugeSelector.SelectedIndex = (int)ModManager.CurrentMod.DefaultLanguage;

                UpdateModDataButton.Visibility = Visibility.Visible;
            }
        }

        private void FindPreviousMods()
        {
            string[] modList = ModManager.GetAvailableMods();

            if (modList.Length > 0)
            {
                PreviousModsList.ItemsSource = modList;
            }
            else
            {
                PreviousModsList.Visibility = Visibility.Hidden;
            }
        }

        private bool ValidateForm()
        {
            bool result = true;

            if (ModName.InputTextBox.Text.ToString().Length < 3)
            {
                ModName.BorderBrush = Brushes.Red;
                result = false;
            }
            else
            {
                ModName.BorderBrush = Brushes.White;
            }

            if (ModDescription.Text.ToString().Length < 3)
            {
                ModDescription.BorderBrush = Brushes.Red;
                result |= false;
            }
            else
            {           
                ModDescription.BorderBrush = Brushes.White;
            }

            if (ModVersion.Text.ToString().Length < 1)
            {
                ModVersion.Text = "1.0";
            }

            return result;
        }


        private void ProcessNewMod()
        {
            if (ValidateForm())
            {
                var defLanguage = (GameLanguages.DefaultLanguages)DefaultLangugeSelector.SelectedIndex;

                Mod mod = new(ModName.Text, ModDescription.Text, ModVersion.Text, defaultLanguage: defLanguage);

                if (ModManager.CurrentMod != null && ModManager.CurrentMod.Name == mod.Name)
                {
                    var result = MessageBox.Show("Current action will rewrite current mod data.", "Warning", MessageBoxButton.OKCancel);
                    if (result == MessageBoxResult.Cancel)
                    {
                        GotoMusicManagerPage();
                        return;
                    }
                }

                ModManager.AddMod(mod);
                ModManager.SwitchMod(ModManager.AllMods.Last());

                GotoMusicManagerPage();
            }
            else
            {
                MessageBox.Show("Please fill all the fields correctly", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void GotoMusicManagerPage()
        {
            OnRequestPageChange(nameof(MusicManager));
        }


        private void UpdateCurrentModData()
        {
            if (ValidateForm())
            {
                var defLanguage = (GameLanguages.DefaultLanguages)DefaultLangugeSelector.SelectedIndex;
                ModManager.UpdateCurrentMod(ModName.Text, ModDescription.Text, ModVersion.Text, defLanguage);

                MessageBox.Show("Mod data updated successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Please fill all the fields correctly", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ContinueButton_Click(object sender, RoutedEventArgs e)
        {
            ProcessNewMod();
        }

        private void LoadProjectButton_Click(object sender, RoutedEventArgs e)
        {
            bool success = ModManager.LoadMod(PreviousModsList.SelectedItem.ToString());

            if (success) GotoMusicManagerPage();
        }

        private void BrowseProjectButton_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog openFileDialog = new()
            {
                Filter = "Json file (*.json)|*.json"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                // getting song name from file
                string filePath = openFileDialog.FileName;

                bool success = ModManager.LoadMod(filePath, true);
                if (success) GotoMusicManagerPage();
            }
        }

        private void UpdateModDataButton_Click(object sender, RoutedEventArgs e)
        {
            UpdateCurrentModData();
        }
    }
}
