using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace Vic3ModManager
{
    public partial class AnimatedPlaceholderTextBox : UserControl
    {
        private string _text;

        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
            "Text", typeof(string), typeof(AnimatedPlaceholderTextBox), new PropertyMetadata(string.Empty, OnTextChanged));

        public static readonly DependencyProperty PlaceholderProperty = DependencyProperty.Register(
            "Placeholder", typeof(string), typeof(AnimatedPlaceholderTextBox), new PropertyMetadata("Placeholder"));

        public static readonly DependencyProperty CaretIndexProperty = DependencyProperty.Register(
            "CaretIndex", typeof(int), typeof(AnimatedPlaceholderTextBox), new PropertyMetadata(0));

        public event TextChangedEventHandler TextChanged;

        public AnimatedPlaceholderTextBox()
        {
            InitializeComponent();
        }

        public string Text
        {
            get { return _text; }
            set
            {
                _text = value;
                InputTextBox.Text = value;
                InputTextBox.CaretIndex = value.Length;
            }
        }

        public string Placeholder
        {
            get { return (string)GetValue(PlaceholderProperty); }
            set { SetValue(PlaceholderProperty, value); }
        }

        public int CaretIndex
        {
            get { return (int)GetValue(CaretIndexProperty); }
            set
            {
                SetValue(CaretIndexProperty, value);
                InputTextBox.CaretIndex = value;
            }
        }

        public void ScrollToEnd()
        {
            InputTextBox.ScrollToEnd();
        }

        private void InputTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            _text = InputTextBox.Text;
            AnimateLabel(!string.IsNullOrWhiteSpace(_text) || InputTextBox.IsFocused);
            TextChanged?.Invoke(this, e);
        }

        private void InputTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            AnimateLabel(true);
        }

        private void InputTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(InputTextBox.Text))
            {
                AnimateLabel(false);
            }
        }

        private void AnimateLabel(bool hasFocus)
        {
            string storyboardName = hasFocus ? "FocusAnimation" : "UnfocusAnimation";
            if (FindResource(storyboardName) is Storyboard storyboard)
            {
                storyboard.Begin(PlaceholderLabel);
            }
        }

        private static void OnTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is AnimatedPlaceholderTextBox control && e.NewValue is string newText)
            {
                control.Text = newText;
            }
        }
    }
}
