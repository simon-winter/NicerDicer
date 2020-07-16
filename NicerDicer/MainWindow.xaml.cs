using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using NicerDicer.Properties;

namespace NicerDicer
{
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private Thickness minBorderThick = new Thickness(1);
        private Thickness maxBorderThick = new Thickness(2);

        int lastRollAmount = 1;
        int lastRollSize = 6;

        #region Properties

        private bool _settingsOpened;
        public bool SettingsOpened {
            get
            {
                return _settingsOpened;
            }
            set
            {
                _settingsOpened = value;
                RaisePropertyChanged("SettingsOpened");
                if (!SettingsOpened) {
                    BuildDiceGrid();
                }
            }
        }

        private bool _useAutoPosting;
        public bool UseAutoPosting {
            get
            {
                return _useAutoPosting;
            }
            set
            {
                _useAutoPosting = value;
                RaisePropertyChanged("UseAutoPosting");
                Settings.Default.UseAutoPosting = value;
                Settings.Default.Save();
            }
        }

        private string _explDiceCmd;
        public string ExplDiceCmd {
            get
            {
                return _explDiceCmd;
            }
            set
            {
                _explDiceCmd = value;
                RaisePropertyChanged("ExplDiceCmd");
                Settings.Default.ExplDiceCmd = value;
                Settings.Default.Save();
            }
        }

        private string _diceAmount;
        public string DiceAmount {
            get
            {
                return _diceAmount;
            }
            set
            {
                _diceAmount = value;
                RaisePropertyChanged("DiceAmount");
                ValidateDiceAmount(out int bla, out int blu);
                Settings.Default.DiceAmount = value;
                Settings.Default.Save();
            }
        }

        private string _diceSizes;
        public string DiceSizes {
            get
            {
                return _diceSizes;
            }
            set
            {
                _diceSizes = value;
                RaisePropertyChanged("DiceSizes");
                ValidateDiceSizes(out List<int> r2d2);
                Settings.Default.DiceSizes = value;
                Settings.Default.Save();
            }
        }

        private string _diceSizesValidation;
        public string DiceSizesValidation {
            get
            {
                return _diceSizesValidation;
            }
            set
            {
                _diceSizesValidation = value;
                RaisePropertyChanged("DiceSizesValidation");
            }
        }

        private string _diceAmountValidation;
        public string DiceAmountValidation {
            get
            {
                return _diceAmountValidation;
            }
            set
            {
                _diceAmountValidation = value;
                RaisePropertyChanged("DiceAmountValidation");
            }
        }

        private string _channelName;
        public string ChannelName {
            get
            {
                return _channelName;
            }
            set
            {
                _channelName = value;
                RaisePropertyChanged("ChannelName");

                Settings.Default.ChannelName = value;
                Settings.Default.Save();
            }
        }

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
                ExampleOutput = BuildDiceString(lastRollAmount, lastRollSize);
                Settings.Default.Prefix = value;
                Settings.Default.Save();
            }
        }

        private bool _useExplodingDice = false;
        public bool UseExplodingDice {
            get
            {
                return _useExplodingDice;
            }
            set
            {
                _useExplodingDice = value;
                RaisePropertyChanged("UseExplodingDice");
                ExampleOutput = BuildDiceString(lastRollAmount, lastRollSize);
                Settings.Default.UseExplodingDice = value;
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
                Clipboard.SetText(_exampleOutput);
            }
        }

        #endregion

        public MainWindow() {

            InitializeComponent();

            ToolTipService.ShowDurationProperty.OverrideMetadata(
                typeof(DependencyObject), new FrameworkPropertyMetadata(Int32.MaxValue));

            this.SizeChanged += OnWindowSizeChanged;

            DiceAmount = Settings.Default.DiceAmount;
            DiceSizes = Settings.Default.DiceSizes;

            Prefix = Settings.Default.Prefix;

            UseExplodingDice = Settings.Default.UseExplodingDice;
            ExplDiceCmd = Settings.Default.ExplDiceCmd;

            UseAutoPosting = Settings.Default.UseAutoPosting;
            ChannelName = Settings.Default.ChannelName;
            // Set ExampleOutput after setting all variables
            ExampleOutput = BuildDiceString(lastRollAmount, lastRollSize);

            BuildDiceGrid();
        }

        private void BuildDiceGrid() {

            diceGrid.Children.Clear();
            diceGrid.ColumnDefinitions.Clear();
            diceGrid.RowDefinitions.Clear();

            int minDiceAmount, maxDiceAmount;
            if (!ValidateDiceAmount(out minDiceAmount, out maxDiceAmount)) {
                return;
            }

            List<int> diceSizes;
            if (!ValidateDiceSizes(out diceSizes)) {
                return;
            }

            // add Gridcells
            for (int i = minDiceAmount; i <= maxDiceAmount; i++) {
                diceGrid.ColumnDefinitions.Add(new ColumnDefinition() {
                    Width = new GridLength(1, GridUnitType.Star)
                });
            }
            for (int i = 0; i < diceSizes.Count; i++) {
                diceGrid.RowDefinitions.Add(new RowDefinition() {
                    Height = new GridLength(1, GridUnitType.Star)
                });
            }

            // populate Gridcells
            for (int col = 0; col <= maxDiceAmount - minDiceAmount; col++) {
                for (int row = 0; row < diceSizes.Count; row++) {
                    int amount = minDiceAmount + col;

                    Button btn = new Button() {
                        Content = $"{amount}d{diceSizes[row]}",
                        BorderThickness = new Thickness(2),
                        MinWidth = 10,
                        Margin = new Thickness(5),
                        Background = Brushes.LightGray
                    };
                    Grid.SetRow(btn, row);
                    Grid.SetColumn(btn, col);
                    btn.Click += OnButtonClicked;

                    diceGrid.Children.Add(btn);
                }
            }
        }

        private bool ValidateDiceAmount(out int min, out int max) {
            try {
                var text = Regex.Split(DiceAmount, ",");
                min = int.Parse(text[0]);
                max = int.Parse(text[1]);
                if (min <= 0 || max <= 0) {
                    throw new InvalidOperationException();
                }
                DiceAmountValidation = "Valid";
                return true;
            }
            catch {
                DiceAmountValidation = "Invalid";
                min = 0;
                max = 0;
                return false;
            }
        }

        private bool ValidateDiceSizes(out List<int> diceSizes) {
            diceSizes = new List<int>();
            try {
                var text = Regex.Split(DiceSizes, ",");
                foreach (string diceSizeString in text) {
                    int diceSize = int.Parse(diceSizeString);
                    if (diceSize <= 0) {
                        throw new InvalidOperationException();
                    }
                    diceSizes.Add(diceSize);
                }
                DiceSizesValidation = "Valid";
                return true;
            }
            catch {
                DiceSizesValidation = "Invalid";
                diceSizes = null;
                return false;
            }
        }


        private string BuildDiceString(int diceAmount, int diceSize) {
            string explDice = UseExplodingDice ? $"{ExplDiceCmd}{diceSize}" : ""; // Todo: make command customizable
            return $"{_prefix}{diceAmount}d{diceSize}{explDice}";
        }


        private void OnWindowSizeChanged(object sender, EventArgs e) {

            foreach (Button btn in diceGrid.Children) {

                double cellHeight = diceGrid.ActualHeight / diceGrid.RowDefinitions.Count;
                double cellWidth = diceGrid.ActualWidth / diceGrid.ColumnDefinitions.Count;

                if (cellHeight < 30 || cellWidth < 50) {
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
                lastRollAmount = int.Parse(diceInfo[0]);
                lastRollSize = int.Parse(diceInfo[1]);

                string rollText = BuildDiceString(lastRollAmount, lastRollSize);
                ExampleOutput = rollText;
                if (UseAutoPosting) {
                    if (!DiscordPaster.PostInDiscord(rollText, ChannelName)) {
                        ChangeBackgroundForDuration(btn, Brushes.Red, 200);
                    }
                }
            }
        }
        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e) {
            base.OnMouseLeftButtonDown(e);
            // reset focus on click into application, so textboxes get unselected and updated
            _UseExplodingDiceButton.Focus();
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

        private void Button_Click(object sender, RoutedEventArgs e) {
            SettingsOpened = !SettingsOpened;
        }
    }
}
