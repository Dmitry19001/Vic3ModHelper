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
using Vic3ModManager;

namespace Vic3ModManager
{
    /// <summary>
    /// Interaction logic for HomePage.xaml
    /// </summary>
    public partial class HomePage : Page
    {
        public HomePage()
        {
            InitializeComponent();

            ShowCurrentModData();

            FindPreviousMods();
        }

        private void ShowCurrentModData()
        {
            if (ModManager.CurrentMod != null)
            {
                ModName.Text = ModManager.CurrentMod.Name;
                ModDescription.Text = ModManager.CurrentMod.Description;
                ModVersion.Text = ModManager.CurrentMod.Version;
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

        private void ContinueButton_Click(object sender, RoutedEventArgs e)
        {

            if(ValidateForm())
            {
                Mod mod = new Mod(ModName.Text, ModDescription.Text, ModVersion.Text);
                MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
                //// TODO: Multiple Mod support after saving system 
                //// Currently clearing all mods list and adding new one

                if (ModManager.CurrentMod != null && ModManager.CurrentMod.Name == mod.Name)
                {
                    var result = MessageBox.Show("Current action will rewrite current mod data.", "Warning", MessageBoxButton.OKCancel);
                    if (result == MessageBoxResult.Cancel) 
                    {
                        mainWindow.ChangePage("Music Manager");
                        return;
                    }
                }

                ModManager.AllMods.Clear();
                ModManager.AddMod(mod);
                ModManager.SwitchMod(mod);


                mainWindow.ChangePage("Music Manager");
            }
            else
            {
                MessageBox.Show("Please fill all the fields correctly", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LoadProjectButton_Click(object sender, RoutedEventArgs e)
        {
            ModManager.LoadMod(PreviousModsList.SelectedItem.ToString());
            ShowCurrentModData();
        }
    }
}
