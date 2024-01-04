using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Vic3ModManager
{
    public class PlaceholderTextBox : TextBox
    {
        static PlaceholderTextBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PlaceholderTextBox), new FrameworkPropertyMetadata(typeof(PlaceholderTextBox)));
        }

        public static readonly DependencyProperty WarningBorderBrushProperty =
            DependencyProperty.Register(
                "WarningBorderBrush",
                typeof(Brush),
                typeof(PlaceholderTextBox),
                new PropertyMetadata(Brushes.White)); // Default to white


        public Brush WarningBorderBrush
        {
            get { return (Brush)GetValue(WarningBorderBrushProperty); }
            set { SetValue(WarningBorderBrushProperty, value); }
        }


        public static readonly DependencyProperty PlaceholderProperty =
            DependencyProperty.Register("Placeholder", typeof(string), typeof(PlaceholderTextBox), new PropertyMetadata(string.Empty));

        public string Placeholder
        {
            get { return (string)GetValue(PlaceholderProperty); }
            set { SetValue(PlaceholderProperty, value); }
        }
    }
}
