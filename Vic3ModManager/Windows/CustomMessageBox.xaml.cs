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
using System.Windows.Shapes;

namespace Vic3ModManager.Windows
{
    /// <summary>
    /// Interaction logic for CustomMessageBox.xaml
    /// </summary>

    public enum CustomMessageBoxResult
    {
        YesDontAskAgain,
        Yes,
        ContinueWithoutConversion,
        CancelExport
    }

    public partial class CustomMessageBox : Window
    {
        public CustomMessageBoxResult MessageBoxResult { get; private set; }
        public string HeaderText { get; set; }
        public string Message { get; set; }


        public CustomMessageBox(string headerText, string message)
        {
            InitializeComponent();

            HeaderText = headerText;
            Message = message;
            DataContext = this;
        }

        private void YesDontAsk_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult = CustomMessageBoxResult.YesDontAskAgain;
            Close();
        }

        private void Yes_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult = CustomMessageBoxResult.Yes;
            Close();
        }

        private void ContinueWithout_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult = CustomMessageBoxResult.ContinueWithoutConversion;
            Close();
        }

        private void CancelExport_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult = CustomMessageBoxResult.CancelExport;
            Close();
        }
    }

}
