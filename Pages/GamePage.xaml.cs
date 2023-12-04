using SQLBombDisposal.Classes;
using SQLBombDisposal.Pages.Puzzles;
using System;
using System.Collections.Generic;
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

        private string currentTime = string.Empty;

        public string CurrentTime
        {
            get { return currentTime; }
            set { currentTime = value; OnPropertyChanged(); }
        }

        private int puzzlesTotal;
        public int PuzzlesTotal
        {
            get { return puzzlesTotal; }
            set { puzzlesTotal = value; OnPropertyChanged(); }
        }

        private int puzzlesCompleted;
        public int PuzzlesCompleted
        {
            get { return puzzlesCompleted; }
            set { puzzlesCompleted = value; OnPropertyChanged(); }
        }

        private int timePenalty;
        public int TimePenalty
        {
            get { return timePenalty; }
            set { timePenalty = value; OnPropertyChanged(); }
        }

        #endregion
        private DispatcherTimer dispatcherTimer = new DispatcherTimer();
        private TimeSpan elapsedTime = TimeSpan.Zero;
        private TimeSpan totalTime = TimeSpan.Zero;

        private List<IPuzzle> puzzles;
        private IPuzzle? puzzle = null;

        public GamePage(int iPuzzles, int minutes, int seconds)
        {
            InitializeComponent();

            puzzles = new List<IPuzzle>();

            PuzzlesTotal = iPuzzles;
            PuzzlesCompleted = 0;
            TimePenalty = 5;

            totalTime = TimeSpan.FromMinutes(minutes).Add(TimeSpan.FromSeconds(seconds));
            CurrentTime = String.Format("{0:00}:{1:00}", totalTime.Minutes, totalTime.Seconds);

            FillPuzzleList();
            LoadPuzzle();

            dispatcherTimer.Tick += DispatcherTimer_Tick;
            dispatcherTimer.Interval = TimeSpan.FromSeconds(1);
            dispatcherTimer.Start();

            DataContext = this;
        }

        private void FillPuzzleList()
        {
            puzzles.Add(new MazePuzzlePage());
            puzzles.Add(new ButtonPuzzlePage());
            puzzles.Add(new BattleshipPuzzle());
            puzzles.Add(new AdventurerPuzzle());
            puzzles.Add(new StudentPuzzle());
            Shuffler.Shuffle(puzzles);
        }

        private void DispatcherTimer_Tick(object sender, EventArgs e)
        {
            elapsedTime += dispatcherTimer.Interval;
            TimeSpan ts = TimeSpan.FromMinutes(5).Subtract(elapsedTime);
            CurrentTime = String.Format("{0:00}:{1:00}", ts.Minutes, ts.Seconds);

            if (ts.Minutes <= 0 && ts.Seconds <= 0)
            {
                dispatcherTimer.Stop();

                MessageBox.Show("Boom");
                NavigationService.Navigate(new ConfigPage());
            }

        }

        private void LoadPuzzle()
        {
            if(puzzle != null)
            {
                puzzle.PuzzleCompleted -= HandleCompletion;
                puzzle.Penalize -= HandleTimePenalty;
            }
            if (puzzles.Count == 0)
            {
                MessageBox.Show("Congratulations, you have beaten the game");
                Application.Current.Shutdown();
                return;
            }

            puzzle = puzzles.First();            
            puzzles.Remove(puzzle);

            puzzle.PuzzleCompleted += HandleCompletion;
            puzzle.Penalize += HandleTimePenalty;

            puzzleFrame.NavigationService.Navigate(puzzle);
        }

        /**
         * Eventhandler for when one of the puzzles within the puzzleFrame is completed
         */
        private void HandleCompletion(object? sender, EventArgs e)
        {
            MessageBox.Show("Recieved Completion Event");
            LoadPuzzle();
        }

        private void HandleTimePenalty(object? sender, EventArgs e)
        {
            elapsedTime += TimeSpan.FromSeconds(TimePenalty);
            TimePenalty += 5;
        }
    }
}
