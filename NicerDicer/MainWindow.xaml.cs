using System;
using System.ComponentModel;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using NicerDicer.Properties;

namespace NicerDicer
{
    public partial class MainWindow : Window, INotifyPropertyChanged
    {        
        private Thickness minBorderThick = new Thickness(0.5f);
        private Thickness maxBorderThick = new Thickness(2);

        int diceAmount = 6;
        int diceValueCap = 14;

        private string _prefix;
        public string Prefix {
            get
            {
                return _prefix;
            }
            set
            {
                _prefix = value;
                RaisePropertyChanged("Prefix");
                ExampleOutput = BuildDiceString(1, 6);
                Settings.Default.Prefix = value;
                Settings.Default.Save();
            }
        }


        private bool _explodingDice = false;
        public bool ExplodingDice {
            get
            {
                return _explodingDice;
            }
            set
            {
                _explodingDice = value;
                RaisePropertyChanged("ExplodingDice");
                ExampleOutput = BuildDiceString(1, 6);
                Settings.Default.ExplodingDice = value;
                Settings.Default.Save();
            }
        }

        private string _exampleOutput;


        public string ExampleOutput {
            get
            {
                return _exampleOutput;
            }
            set
            {
                _exampleOutput = value;
                RaisePropertyChanged("ExampleOutput");
            }
        }

        public MainWindow() {

            InitializeComponent();

            this.SizeChanged += OnWindowSizeChanged;

            Prefix = Settings.Default.Prefix;
            ExplodingDice = Settings.Default.ExplodingDice;

            BuildDiceGrid();
        }

        private void BuildDiceGrid() {

            // add Gridcells
            for (int amount = 1; amount <= diceAmount; amount++) {
                diceGrid.ColumnDefinitions.Add(new ColumnDefinition() {
                    Width = new GridLength(1, GridUnitType.Star)
                });
            }
            for (int row = 1; row <= (diceValueCap / 2); row++) {
                diceGrid.RowDefinitions.Add(new RowDefinition() {
                    Height = new GridLength(1, GridUnitType.Star)
                });
            }

            // populate Gridcells
            for (int diceAmount = 1; diceAmount <= diceGrid.ColumnDefinitions.Count; diceAmount++) {
                for (int row = 1; row <= diceGrid.RowDefinitions.Count; row++) {
                    int diceSize = row * 2;
                    Button btn = new Button() {
                        Content = $"{diceAmount}d{diceSize}",
                        BorderThickness = new Thickness(2),
                        MinWidth = 10,
                        Margin = new Thickness(5)
                    };
                    Grid.SetRow(btn, row - 1);
                    Grid.SetColumn(btn, diceAmount - 1);
                    btn.Click += OnButtonClicked;
                    diceGrid.Children.Add(btn);
                }
            }
        }

        private string BuildDiceString(int diceAmount, int diceSize) {
            string explDice = ExplodingDice ? $"e{diceSize}" : "";
            return $"{_prefix}{diceAmount}d{diceSize}{explDice}";
        }


        private void OnWindowSizeChanged(object sender, EventArgs e) {

            foreach (Button btn in diceGrid.Children) {


                double cellHeight = diceGrid.ActualHeight / diceGrid.RowDefinitions.Count;
                double cellWidth = diceGrid.ActualWidth / diceGrid.ColumnDefinitions.Count;

                if (cellHeight < 30 || cellWidth < 40) {
                    btn.Margin = new Thickness(0);
                    btn.BorderThickness = minBorderThick;
                }
                else {
                    Thickness newMargin = new Thickness(0);
                    newMargin.Bottom = cellHeight * 0.07f;
                    newMargin.Top = cellHeight * 0.07f;
                    newMargin.Left = cellWidth * 0.01f;
                    newMargin.Right = cellWidth * 0.01f;

                    btn.Margin = newMargin;
                    btn.BorderThickness = maxBorderThick;
                }
            }
        }

        private void OnButtonClicked(object sender, EventArgs e) {
            if (sender is Button btn) {
                ChangeBackgroundForDuration(btn, Brushes.LightGreen, 200);
                string[] diceInfo = Regex.Split(btn.Content.ToString(), "d");
                Clipboard.SetText(
                    BuildDiceString(int.Parse(diceInfo[0]), int.Parse(diceInfo[1]))
                    );
            }
        }
        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e) {
            base.OnMouseLeftButtonDown(e);
            _ExplodingDiceButton.Focus();
        }

        private async Task ChangeBackgroundForDuration(Button btn, Brush newBrush, float durationMs) {
            btn.Background = newBrush;
            await Task.Delay(TimeSpan.FromMilliseconds(durationMs));
            btn.Background = Brushes.LightGray;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void RaisePropertyChanged(string propertyName) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
