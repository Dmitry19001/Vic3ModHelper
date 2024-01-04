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
    /// Interaction logic for SongControl.xaml
    /// </summary>
    public partial class SongControl : UserControl
    {
        public event RoutedEventHandler DeleteButtonClick;

        public static readonly DependencyProperty SongTitleProperty =
            DependencyProperty.Register("Title", typeof(string), typeof(SongControl), new PropertyMetadata("Untitled"));

        public static readonly DependencyProperty SongDurationProperty =
            DependencyProperty.Register("Duration", typeof(string), typeof(SongControl), new PropertyMetadata("0:00"));

        public SongControl()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DeleteButtonClick?.Invoke(this, new RoutedEventArgs());
        }

        public string Title
        {
            get { return (string)GetValue(SongTitleProperty); }
            set { SetValue(SongTitleProperty, value); }
        }

        public string Duration
        {
            get { return (string)GetValue(SongDurationProperty); }
            set { SetValue(SongDurationProperty, value); }
        }
    }
}
