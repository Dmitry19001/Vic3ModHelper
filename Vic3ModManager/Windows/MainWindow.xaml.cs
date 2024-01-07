using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Vic3ModManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly Dictionary<string, Func<Page>> pages = new();
        private int currentPage = 0;

        public MainWindow()
        {
            InitializeComponent();

            // if debigging, load the test mod
            if (System.Diagnostics.Debugger.IsAttached)
            {
                ModManager.AddMod(new Mod("test_mod", "test_desc", "0.1"));
                ModManager.SwitchMod(ModManager.AllMods[0]);
            }

            InitializePages();

            Closing += MainWindow_Closing;
        }

        private void InitializePages()
        {
            pages.Add("Home", () => new HomePage());
            pages.Add("Music Manager", () => new MusicManagerPage());
            pages.Add("Export", () => new ExportPage());

            GenerateNavigationButtons();

            ChangePage("Home");
        }

        private void MainWindow_Closing(object sender, CancelEventArgs e)
        {
            AppConfig.Instance.Save();

            if (ModManager.CurrentMod != null)
            {
                var result = MessageBox.Show("Do you want to save the current mod before exiting?", "Save mod?", MessageBoxButton.YesNoCancel);

                if (result == MessageBoxResult.Yes)
                {
                    ModManager.SaveCurrentMod();
                }
                else if (result == MessageBoxResult.Cancel)
                {
                    e.Cancel = true;
                }
            }
        }

        private void GenerateNavigationButtons()
        {
            foreach (KeyValuePair<string, Func<Page>> page in pages)
            {
                Button navButton = new();
                navButton.Content = page.Key;
                navButton.Click += NavButton_Click;
                navButton.Style = (Style)FindResource("NavigationButtonStyle");
                Navigations.Children.Add(navButton);
            }
        }

        private void NavButton_Click(object sender, RoutedEventArgs e)
        {
            Button? button = sender as Button;
            string? pageKey = button?.Content.ToString();
            
            if ( pageKey!= null) ChangePage(pageKey);
        }

        private void RefreshNavigationButtons()
        {
            for (int i = 0;  i < Navigations.Children.Count; i++)
            {
                // Skipping if unable to find a button
                if (Navigations.Children[i] is not Button navButton) continue;

                // Disabling navigation if there are no mods
                // To avoid crashes
                if (ModManager.AllMods.Count == 0)
                {
                    navButton.IsEnabled = false;
                    continue;
                }

                if (i == currentPage)
                {
                    navButton.IsEnabled = false;
                }
                else
                {
                    navButton.IsEnabled |= true;
                }
            }
        }

        private int GetPageIndex(string key)
        {
            var keys = pages.Keys.ToList();
            return keys.IndexOf(key);
        }


        public void ChangePage(string key)
        {
            // Cleanup the previous page
            if (ContentFrame.Content is IDisposable disposable)
            {
                disposable.Dispose();
            }

            ContentFrame.Content = pages[key](); // Create a new instance
            currentPage = GetPageIndex(key);
        }

        private void ContentFrame_ContentRendered(object sender, EventArgs e)
        {
            RefreshNavigationButtons();

            SwitchProjectSaveButtonVisibility();
        }

        private void SwitchProjectSaveButtonVisibility()
        {
            if (ModManager.CurrentMod == null)
            {
                SaveProjectButton.Visibility = Visibility.Hidden;
            }
            else
            {
                SaveProjectButton.Visibility = Visibility.Visible;
            }
        }

        private void SaveProjectButton_Click(object sender, RoutedEventArgs e)
        {
            ModManager.SaveCurrentMod();
        }
    }
}
