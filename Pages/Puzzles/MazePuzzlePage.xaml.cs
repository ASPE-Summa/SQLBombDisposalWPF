using SQLBombDisposal.Models;
using SQLBombDisposal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SQLBombDisposal.Pages.Puzzles
{
    /// <summary>
    /// Interaction logic for MazePuzzlePage.xaml
    /// </summary>
    public partial class MazePuzzlePage : Page, IPuzzle
    {
        private int pattern;

        private List<List<char>> maze;

        private char playerCharacter = '@';
        private char goalCharacter = '$';
        private char emptyCharacter = '.';
        private char wallCharacter = '#';

        private int xPos;
        private int yPos;

        public MazePuzzlePage()
        {
            InitializeComponent();
            maze = new List<List<char>>();

            using (SqlBombDisposalContext context = new SqlBombDisposalContext())
            {
                Random r = new Random();
                int maxPattern = context.MazePuzzles.Max(p => p.Pattern) + 1;
                pattern = r.Next(1, maxPattern);
            }

            ResetMaze();
            Application.Current.MainWindow.KeyDown += MazeGrid_KeyDown;
        }

        public event EventHandler<EventArgs> PuzzleCompleted;
        public event EventHandler<EventArgs> TimePenalty;

        private void ResetMaze()
        {
            maze.Clear();
            using (SqlBombDisposalContext context = new SqlBombDisposalContext())
            {
                foreach (MazePuzzle row in context.MazePuzzles.Where(p => p.Pattern == pattern).OrderBy(p => p.Sequence))
                {
                    List<char> characterList = row.Contents.ToList();
                    if (characterList.Contains(playerCharacter))
                    {
                        xPos = characterList.IndexOf(playerCharacter);
                        yPos = row.Sequence - 1;
                    }
                    maze.Add(characterList);
                }
            }
            DrawMaze();
        }

        private void DrawMaze()
        {
            for (int rowIndex = 0; rowIndex < maze.Count; rowIndex++)
            {
                for (int colIndex = 0; colIndex < maze[rowIndex].Count; colIndex++)
                {
                    char character = maze[rowIndex][colIndex];
                    TextBlock tb = mazeGrid.Children.Cast<TextBlock>().First(e => Grid.GetRow(e) == rowIndex && Grid.GetColumn(e) == colIndex);

                    if (character == wallCharacter)
                    {
                        tb.Text = emptyCharacter.ToString();
                    }
                    else
                    {
                        tb.Text = character.ToString();
                    }
                }
            }
        }

        private void MazeGrid_KeyDown(object sender, KeyEventArgs e)
        {
            int newXPos = xPos;
            int newYPos = yPos;

            switch (e.Key)
            {
                case Key.Left:
                    newXPos = xPos - 1;
                    break;
                case Key.Right:
                    newXPos = xPos + 1;
                    break;
                case Key.Up:
                    newYPos = yPos - 1;
                    break;
                case Key.Down:
                    newYPos= yPos + 1;
                    break;
            }

            if(newXPos > maze[0].Count || newXPos < 0 || newYPos >= maze.Count || newYPos < 0 || maze[newYPos][newXPos] == wallCharacter) {
                TimePenalty?.Invoke(this, EventArgs.Empty);
                MessageBox.Show("Helaas de resterende tijd is verlaagd.");
                ResetMaze();
                return;
            }

            if (maze[newYPos][newXPos] == goalCharacter) {
                LevelComplete();
            }

            MoveCharacter(newXPos, newYPos);
            
        }

        private void MoveCharacter(int newXPos, int newYPos)
        {
            maze[yPos][xPos] = emptyCharacter;
            maze[newYPos][newXPos] = playerCharacter;

            xPos = newXPos;
            yPos = newYPos;

            DrawMaze();
        }

        private void LevelComplete()
        {
            Application.Current.MainWindow.KeyDown -= MazeGrid_KeyDown;
            PuzzleCompleted?.Invoke(this, EventArgs.Empty);
        }
    }
}
