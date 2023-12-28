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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly Dictionary<string, Page> pages = new();
        private int currentPage = 0;

        public MainWindow()
        {
            InitializeComponent();

            pages.Add("Home", new Page());
            pages.Add("MusicManager", new MusicManager());
        }

        private void RefreshNavigationButtons()
        {
            for (int i = 0;  i < Navigations.Children.Count; i++)
            {
                Button? navButton = Navigations.Children[i] as Button;
                // Skipping if unable to find a button
                if (navButton is not Button) continue;

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


        private void ChangePage(string key)
        {
            ContentFrame.Content = pages[key];
            currentPage = GetPageIndex(key);
        }

        private void MusicManagerButton_Click(object sender, RoutedEventArgs e)
        {
            ChangePage("MusicManager");
        }

        private void Home_Click(object sender, RoutedEventArgs e)
        {
            ChangePage("Home");
        }

        private void ContentFrame_ContentRendered(object sender, EventArgs e)
        {
            RefreshNavigationButtons();
        }
    }
}
