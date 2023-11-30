using Emoji.Wpf;
using Microsoft.EntityFrameworkCore;
using SQLBombDisposal.Classes;
using SQLBombDisposal.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace SQLBombDisposal.Pages.Puzzles
{
    /// <summary>
    /// Interaction logic for ButtonPuzzlePage.xaml
    /// </summary>
    public partial class ButtonPuzzlePage : Page, IPuzzle
    {
        private ObservableCollection<Models.Button> Buttons = new ObservableCollection<Models.Button>(); 
        public ButtonPuzzlePage()
        {
            InitializeComponent();
            LoadButtons();
            DrawButtons();
        }

        public event EventHandler<EventArgs> PuzzleCompleted;
        public event EventHandler<EventArgs> Penalize;

        /**
         * Draws 9 buttons from the database of which 3 are armed and 6 are not.
         */
        public void LoadButtons()
        {
            using(SqlBombDisposalContext context = new SqlBombDisposalContext())
            {
                List<Models.Button> dbButtons = new List<Models.Button>();

                dbButtons.AddRange(
                context.Buttons.Where(p => p.ButtonVoltage.Voltage == 0)
                    .Include("ButtonVoltage")
                    .AsEnumerable()
                    .OrderBy(p => Guid.NewGuid())
                    .Take(3)
                    );

                dbButtons.AddRange(
                context.Buttons.Where(p => p.ButtonVoltage.Voltage == 1)
                    .Include("ButtonVoltage")
                    .AsEnumerable()
                    .OrderBy(p => Guid.NewGuid())
                    .Take(6)
                    );

                Shuffler.Shuffle( dbButtons );
                dbButtons.ForEach(Buttons.Add);
            }
        }

        public void DrawButtons()
        {
            List<StackPanel> panels = buttonGrid.Children.Cast<StackPanel>().ToList();
            int index = 0;
            foreach(StackPanel stackPanel in panels)
            {
                stackPanel.Background = Brushes.White;
                System.Windows.Controls.Image i = stackPanel.Children.Cast<System.Windows.Controls.Image>().First();
                Emoji.Wpf.Image.SetSource(i, Buttons[index].Emoji);

                System.Windows.Controls.TextBlock t = (System.Windows.Controls.TextBlock)stackPanel.Children[1];
                t.Text = Buttons[index].Name;

                stackPanel.MouseDown += ButtonClicked;
                index++;
            }
        }

        private void ButtonClicked(object sender, MouseButtonEventArgs e)
        {
            StackPanel stackPanel = (StackPanel) sender;

            Models.Button b = Buttons[buttonGrid.Children.IndexOf(stackPanel)];
            if (b.ButtonVoltage.Voltage == 0)
            {
                stackPanel.Background = Brushes.Green;
                CheckVictory();
            }
            else
            {
                Penalize?.Invoke(this, EventArgs.Empty);
                DrawButtons();
            }
        }

        private void CheckVictory()
        {
            List<StackPanel> panels = buttonGrid.Children.Cast<StackPanel>().Where(b => b.Background == Brushes.Green).ToList();
            if(panels.Count == 3)
            {
                PuzzleCompleted?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}
