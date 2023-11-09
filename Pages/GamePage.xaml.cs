using SQLBombDisposal.Models;
using SQLBombDisposal.Pages.Puzzles;
using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace SQLBombDisposal.Pages
{
    /// <summary>
    /// Interaction logic for GamePage.xaml
    /// </summary>
    public partial class GamePage : Page, INotifyPropertyChanged
    {

        #region Properties
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        #endregion
        private DispatcherTimer dispatcherTimer = new DispatcherTimer();
        private TimeSpan elapsedTime = TimeSpan.Zero;
        private string currentTime = string.Empty;
        private int timePenalty = 5;

        public GamePage()
        {
            InitializeComponent();

            LoadPuzzle();

            dispatcherTimer.Tick += DispatcherTimer_Tick;
            dispatcherTimer.Interval = TimeSpan.FromSeconds(1);

            dispatcherTimer.Start();

            DataContext = this;
        }

        private void DispatcherTimer_Tick(object sender, EventArgs e)
        {
            elapsedTime += dispatcherTimer.Interval;
            TimeSpan ts = TimeSpan.FromMinutes(5).Subtract(elapsedTime);
            currentTime = String.Format("{0:00}:{1:00}", ts.Minutes, ts.Seconds);

            tbTimer.Text = currentTime;
            if (ts.Minutes == 0 && ts.Seconds == 0)
            {
                dispatcherTimer.Stop();

                MessageBox.Show("Boom");
            }

        }

        private void LoadPuzzle()
        {
            IPuzzle t = new MazePuzzlePage();

            t.PuzzleCompleted += HandleCompletion;
            t.TimePenalty += HandleTimePenalty;
            puzzleFrame.NavigationService.Navigate(t);

        }

        /**
         * Eventhandler for when one of the puzzles within the puzzleFrame is completed
         */
        private void HandleCompletion(object? sender, EventArgs e)
        {
            MessageBox.Show("Recieved Completion Event");
        }

        private void HandleTimePenalty(object? sender, EventArgs e)
        {
            elapsedTime += TimeSpan.FromSeconds(timePenalty);
            timePenalty += 5;
        }
    }
}
