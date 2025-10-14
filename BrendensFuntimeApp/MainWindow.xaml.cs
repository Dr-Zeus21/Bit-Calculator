using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using BrendensFuntimeApp.ViewModel;

namespace BrendensFuntimeApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            DataContext = new CalculatorViewModel();
        }

        // Text Formatting

        private static readonly Regex _hexRegex = new Regex("^[0-9A-F]+$", RegexOptions.IgnoreCase);
        private static readonly Regex _decRegex = new Regex("^[0-9]+$");

        private void HexTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !_hexRegex.IsMatch(e.Text);
        }

        private void HexTextBox_Pasting(object sender, DataObjectPastingEventArgs e)
        {
            if (e.DataObject.GetDataPresent(typeof(string)))
            {
                var text = (string)e.DataObject.GetData(typeof(string));
                if (!_hexRegex.IsMatch(text))
                    e.CancelCommand();
            }
            else
            {
                e.CancelCommand();
            }
        }

        private void DecTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !_decRegex.IsMatch(e.Text);
        }

        private void DecTextBox_Pasting(object sender, DataObjectPastingEventArgs e)
        {
            if (e.DataObject.GetDataPresent(typeof(string)))
            {
                var text = (string)e.DataObject.GetData(typeof(string));
                if (!_decRegex.IsMatch(text))
                    e.CancelCommand();
            }
            else
            {
                e.CancelCommand();
            }
        }

        private void TextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            var textBox = sender as TextBox;
            if (textBox == null)
                return;

            // Prevent spaces
            if (e.Key == Key.Space)
            {
                e.Handled = true;
                return;
            }

            // If "0" is showing and user types any valid key (not delete/backspace)
            if (textBox.Text == "0" &&
                (e.Key >= Key.D0 && e.Key <= Key.D9 || e.Key >= Key.A && e.Key <= Key.F))
            {
                textBox.Text = "";
            }

            // Prevent empty state — if user backspaces the last character, put back a "0"
            if (e.Key == Key.Back && textBox.Text.Length == 1)
            {
                textBox.Text = "0";
                textBox.CaretIndex = 1;
                e.Handled = true;
            }
        }

        private void TextBox_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            var textBox = sender as TextBox;
            if (textBox == null) return;

            // Always force caret to the end when clicked
            textBox.Focus();
            textBox.CaretIndex = textBox.Text.Length;
            e.Handled = true;
        }

        private void TextBox_Loaded(object sender, RoutedEventArgs e)
        {
            var textBox = sender as TextBox;
            if (textBox != null && string.IsNullOrEmpty(textBox.Text))
                textBox.Text = "0";
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            var textBox = sender as TextBox;
            if (textBox != null && string.IsNullOrEmpty(textBox.Text))
                textBox.Text = "0";
        }
    }
}
