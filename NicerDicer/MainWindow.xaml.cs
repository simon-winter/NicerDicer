using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using NicerDicer.Properties;

namespace NicerDicer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow() {            
            InitializeComponent();

            CommandTextBox.Text = Settings1.Default.Command;
            ExampleLabel.Text = $"{CommandTextBox.Text}1d6";
            CommandTextBox.TextChanged += OnCommandTextChanged;            

            for (int amount = 1; amount <= diceGrid.ColumnDefinitions.Count; amount++) {
                for (int row = 1; row <= diceGrid.RowDefinitions.Count; row++) {
                    int value = row * 2;
                    Button bt = new Button() {
                        Content = $"{amount}d{value}"
                    };
                    Grid.SetRow(bt, row - 1);
                    Grid.SetColumn(bt, amount - 1);
                    bt.Margin = new Thickness(2, 10, 2, 10);
                    bt.Click += OnButtonClicked;
                    diceGrid.Children.Add(bt);
                }
            }
        }

        private void OnCommandTextChanged(object sender, EventArgs e) {
            if (sender is TextBox txt) {
                ExampleLabel.Text = $"{txt.Text}1d6";
                Settings1.Default.Command = txt.Text;
                Settings1.Default.Save();
            }
        }

        private void OnButtonClicked(object sender, EventArgs e) {
            if (sender is Button btn) {
                ChangeBackgroundForDuration(btn, Brushes.LightGreen, 200);
                Clipboard.SetText($"{CommandTextBox.Text}{btn.Content}");
            }
        }

        private async Task ChangeBackgroundForDuration(Button btn, Brush newBrush, float durationMs) {            
            var originalBrush = btn.Background;
            btn.Background = newBrush;
            await Task.Delay(TimeSpan.FromMilliseconds(durationMs));
            btn.Background = originalBrush;
        }
    }
}
