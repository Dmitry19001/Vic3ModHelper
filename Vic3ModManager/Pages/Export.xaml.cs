using System;
using System.Windows;
using System.Windows.Controls;
using Vic3ModManager.Essentials;
using Vic3ModManager.Windows;
using Ookii.Dialogs.Wpf;
using TagLib.Riff;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.ComponentModel;

namespace Vic3ModManager
{
    /// <summary>
    /// Interaction logic for Export.xaml
    /// </summary>
    public partial class Export : CustomPage
    {
        private string modDirectory;
        private bool exportIsCanceled = false;

        private ModExporter? modExporter = null;
        private List<Func<Task>> stageList = new List<Func<Task>>();
        private int currentStage = 0;

        public Export()
        {
            InitializeComponent();

            ExportProgressPanel.Visibility = Visibility.Collapsed;

            InitilizeModFolder();
        }

        private void InitilizeModFolder()
        {
            // Set the default export path
            ExportPathTextBox.Text = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Paradox Interactive", "Victoria 3", "mod");

            // Scroll to the end of the textbox
            // Need dispatcher because the textbox is not rendered yet
            Dispatcher.BeginInvoke(new Action(() => ExportPathTextBox.ScrollToEnd()));
        }

        private void BrowseDirectoryButton_Click(object sender, RoutedEventArgs e)
        {
            // Show the folder browser dialog wpf

            var dialog = new VistaFolderBrowserDialog();
            if (dialog.ShowDialog() == true)
            {
                string selectedPath = dialog.SelectedPath;
                ExportPathTextBox.Text = selectedPath;
            }
        }

        private void BeginExportButton_Click(object sender, RoutedEventArgs e)
        {
            StartExport();
        }

        private void StartExport()
        {
            PrepareUI();

            exportIsCanceled = false;

            modExporter = new ModExporter(ExportPathTextBox.Text);

            modExporter.OnProgressChanged += ExportProgressChanged;
            modExporter.OnProgressDone += ExportDone;
            modExporter.OnStageDone += ExportStageDone;

            // Currently only music mod is generated
            bool convertMusic = IsMusicConversionAllowed(modExporter.MusicNeedsConversion);

            stageList.AddRange(new Func<Task>[]
            {
                () => modExporter.CreateDirectoriesAsync(),
                () => modExporter.CreateModMetaFileAsync(),
                () => modExporter.CreateMusicPlayerCategoriesFileAsync(),
                () => modExporter.CopyMusicFilesAsync(convertMusic),
                () => modExporter.CreateMusicListFileAsync(),
                () => modExporter.CopyAlbumCoversAsync(),
                () => modExporter.CreateLocalizationsAsync()
            });


            if (exportIsCanceled) {
                ExportLogTextBlock.Text += $"\nExport cancelled, no files were created!";
                return; 
            }

            stageList[currentStage].Invoke();
        }

        private void PrepareUI()
        {
            ExportProgressPanel.Visibility = Visibility.Visible;

            ExportLogTextBlock.Text = $"Starting export process...\n";

            MainWindow.Instance.ToggleNavigationButtons(false);
        }

        private void ExportStageDone(object sender, EventArgs e)
        {
            this.Dispatcher.Invoke(() =>
            {
                NextStage();
            });
        }

        private void NextStage()
        {
            // Next stage start here
            // Each task is a stage
            if (currentStage < stageList.Count - 1)
            {
                currentStage++;
                stageList[currentStage].Invoke();
            }
        }

        private void ExportDone(object sender, EventArgs e)
        {
            // Done message here
            // unsubscribe events
            this.Dispatcher.Invoke(() =>
            {
                CompleteExport();
            });

        }
        private void ExportProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            this.Dispatcher.Invoke(() =>
            {
                UpdateProgressUI(e);
            });
        }

        private void CompleteExport()
        {
            ExportLogTextBlock.Text += $"\n\nExport complete!";

            ExportLogScrollView.ScrollToEnd();

            MessageBox.Show("Export complete!");

            stageList = [];

            modExporter.OnProgressChanged -= ExportProgressChanged;
            modExporter.OnProgressDone -= ExportDone;
            modExporter.OnStageDone -= ExportStageDone;

            modExporter = null;

            MainWindow.Instance.RefreshNavigationButtons();
        }



        private void UpdateProgressUI(ProgressChangedEventArgs e)
        {
            // updating ui here
            ExportProgressBar.Value = e.ProgressPercentage;
            ExportPercentage.Text = $"{e.ProgressPercentage}% {modExporter.CurrentProgress}/{modExporter.TotalProgress}";
            ExportLogTextBlock.Text += $"\n{e.UserState}";
            ExportLogScrollView.ScrollToEnd();
        }

        private bool IsMusicConversionAllowed(bool musicNeedsConversion)
        {
            // TODO: Ask user to download ffmpeg if not found
            // TODO: Options for download: Auto or Manual (if user don't trust author approach)
            if (musicNeedsConversion && AppConfig.Instance.AskForConversionConfirm)
            {
                var customMessageBox = new CustomMessageBox("External .exe needed",
                                            "Warning! Some music needs conversion to .ogg. "
                                            + "FFMPEG can be used to perform the conversion. "
                                            + "FFMPEG is executable that is developed by other developers, so it will be used for your own risk. "
                                            + "Do you want to continue?");

                customMessageBox.ShowDialog();

                switch (customMessageBox.MessageBoxResult)
                {
                    case CustomMessageBoxResult.YesDontAskAgain:
                        AppConfig.Instance.AskForConversionConfirm = false;
                        AppConfig.Instance.Save();
                        break;
                    case CustomMessageBoxResult.Yes:
                        break;
                    case CustomMessageBoxResult.ContinueWithoutConversion:
                        return false;
                    case CustomMessageBoxResult.CancelExport:
                        exportIsCanceled = true;
                        return false;
                }
            }
            return true;
        }
        
    }
}
