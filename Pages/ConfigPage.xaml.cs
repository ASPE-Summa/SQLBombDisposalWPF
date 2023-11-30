using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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

namespace SQLBombDisposal.Pages
{
    /// <summary>
    /// Interaction logic for ConfigPage.xaml
    /// </summary>
    public partial class ConfigPage : Page, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        private int puzzleAmount = 3;
        public int PuzzleAmount
        {
            get { return puzzleAmount; }
            set { puzzleAmount = value; OnPropertyChanged(); }
        }

        private string minutes = "05";
        public string Minutes
        {
            get { return minutes; }
            set { minutes = value; OnPropertyChanged(); }
        }


        private string seconds = "00";
        public string Seconds
        {
            get { return seconds; }
            set { seconds = value; OnPropertyChanged(); }
        }

        public ConfigPage()
        {
            InitializeComponent();
            DataContext = this;
        }

        protected void OnPropertyChanged([CallerMemberName] string? name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private void startButton_Click(object sender, RoutedEventArgs e)
        {
            int iMin;
            int iSec;

            if (int.TryParse(Minutes, out iMin) && int.TryParse(Seconds, out iSec))
            {
                if (iMin <= 10 && iMin >= 1 && iSec <= 60 && iSec >= 0)
                {
                    NavigationService.Navigate(new GamePage(PuzzleAmount, $"{Minutes}:{Seconds}"));
                }
                return;
            }
            MessageBox.Show("Please enter a timespan between 1 and 10 minutes", "Invalid Timespan", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = !IsNumberKey(e.Key) && !IsDelOrBackspaceOrTabKey(e.Key);
        }
        private bool IsNumberKey(Key inKey)
        {
            if (inKey < Key.D0 || inKey > Key.D9)
            {
                if (inKey < Key.NumPad0 || inKey > Key.NumPad9)
                {
                    return false;
                }
            }
            return true;
        }

        private bool IsDelOrBackspaceOrTabKey(Key inKey)
        {
            return inKey == Key.Delete || inKey == Key.Back || inKey == Key.Tab;
        }
    }
}