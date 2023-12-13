using Microsoft.EntityFrameworkCore;
using SQLBombDisposal.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SQLBombDisposal.Pages.Puzzles
{
    /// <summary>
    /// Interaction logic for AdventurerPuzzle.xaml
    /// </summary>
    public partial class AdventurerPuzzle : Page, IPuzzle, INotifyPropertyChanged
    {
        public event EventHandler<EventArgs> PuzzleCompleted;
        public event EventHandler<EventArgs> Penalize;
        public event PropertyChangedEventHandler? PropertyChanged;

        private List<String> functions = new List<String>() { "MIN", "MAX", "COUNT", "SUM", "AVG" };
        private List<String> functionStrings = new List<String>() { "LOWEST", "HIGHEST", "HOW MANY", "TOTAL", "AVERAGE" };
        private List<String> selectors = new List<String>() { "ADVENTURER", "FIGHTER", "WIZARD", "ROGUE", "WARLOCK", "BARD", "DRUID", "RANGER", "MONK" };
        private List<String> subjects = new List<String>() { "LEVEL", "STRENGTH", "DEXTERITY", "CONSTITUTION", "INTELLIGENCE", "WISDOM", "CHARISMA" };

        private Random rand;
        private string function;
        private string selector;
        private string subject;
        private int targetNumber;
        private decimal solution;

        private string questionString;

        public string QuestionString 
        {
            get
            {
                return questionString;
            }
            set 
            {
                questionString = value; OnPropertyChanged();
            }
        }

        public AdventurerPuzzle()
        {
            InitializeComponent();
            rand = new Random();

            function = functions[rand.Next(functions.Count)];
            selector = selectors[rand.Next(selectors.Count)];
            subject = subjects[rand.Next(subjects.Count)];
            targetNumber = rand.Next(1,21);

            QuestionString = GenerateQuestion();
            solution = GetSolution();

            DataContext = this;
        }

        protected void OnPropertyChanged([CallerMemberName] string? name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private string GenerateQuestion()
        {
            if(function == "COUNT")
            {
                return $"HOW MANY {selector}S HAVE {targetNumber} {subject}?";
            }

            return $"WHAT IS THE {functionStrings[functions.IndexOf(function)]} {subject} AMONGST {selector}S?";
        }

        private decimal GetSolution()
        {

            using (DataContext context = new DataContext())
            {
                var command = context.Database.GetDbConnection().CreateCommand();

                context.Database.GetDbConnection().Open();
                command.CommandText = GenerateQuery();

                var result = command.ExecuteScalar();
                context.Database.GetDbConnection().Close();
                if (result.GetType() == typeof(decimal))
                {
                    return (decimal)result;
                }

                return decimal.Parse(result.ToString());
            }
        }

        private string GenerateQuery()
        {
            if(function == "COUNT")
            {
                return generateSumQuery();
            }

            string query = $"SELECT {function.ToLower()}({subject.ToLower()}) FROM adventurer";
            if (selector != "ADVENTURER")
            {
                query += $" WHERE class = '{selector.ToLower()}';";
            }

            return query;
        }

        private string generateSumQuery()
        {
            string query = $"SELECT {function.ToLower()}(id) FROM adventurer";
            if (function == "COUNT" && selector == "ADVENTURER")
            {
                query += $" WHERE {subject.ToLower()} = {targetNumber};";
            }
            else if (function == "COUNT")
            {
                query += $" WHERE {subject.ToLower()} = {targetNumber} AND class = '{selector.ToLower()}';";
            }
            return query;
        }

        private void ProcessAnswer()
        {
            decimal answer = decimal.Parse(tbAnswer.Text);
            if(answer == solution) {
                PuzzleCompleted?.Invoke(this, new EventArgs());
                return;
            }

            Penalize?.Invoke(this, new EventArgs());
            MessageBox.Show($"Your answer: {answer} is incorrect! Penalty has been applied");
        }

        private void tbAnswer_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {
                ProcessAnswer();
            }
        }

        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            ProcessAnswer();
        }
    }
}
