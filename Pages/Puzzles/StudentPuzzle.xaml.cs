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
    /// Interaction logic for StudentPuzzle.xaml
    /// </summary>
    public partial class StudentPuzzle : Page, IPuzzle, INotifyPropertyChanged
    {
        public event EventHandler<EventArgs> PuzzleCompleted;
        public event EventHandler<EventArgs> Penalize;
        public event PropertyChangedEventHandler? PropertyChanged;

        private List<String> functions = new List<String>() {"SUM", "AVG" };
        private List<String> operators = new List<String>() { "HIGHEST", "LOWEST" };
        private List<String> functionStrings = new List<String>() { "TOTAL", "AVERAGE" };
        private List<String> grades = new List<String>() { "NINTH", "TENTH", "ELEVENTH", "TWELFTH" };
        private List<String> subjects = new List<String>() { "MATH", "ENGLISH", "HISTORY", "GEOGRAPHY", "SCIENCE", "ART" };

        private Random rand;
        private string function;
        private string operation;
        private string subject;
        private string solution;

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

        public StudentPuzzle()
        {
            InitializeComponent();
            rand = new Random();

            function = functions[rand.Next(functions.Count)];
            subject = subjects[rand.Next(subjects.Count)];
            operation = operators[rand.Next(operators.Count)];
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
            return $"WHICH GRADE HAS THE {operation.ToLower()} {functionStrings[functions.IndexOf(function)]} {subject} SCORE?";
        }

        private string GetSolution()
        {
            using (DataContext context = new DataContext())
            {
                var command = context.Database.GetDbConnection().CreateCommand();

                context.Database.GetDbConnection().Open();
                command.CommandText = GenerateQuery();

                var result = command.ExecuteScalar();
                context.Database.GetDbConnection().Close();

                return result.ToString();
            }

        }

        private string GenerateQuery()
        {
            if(operation == "HIGHEST")
            {
                return $"SELECT grade, {function.ToLower()}({subject.ToLower()}_score) AS aggregate FROM student GROUP BY (grade) ORDER BY (aggregate) DESC LIMIT 1";
            }
            return $"SELECT grade, {function.ToLower()}({subject.ToLower()}_score) AS aggregate FROM student GROUP BY (grade) ORDER BY (aggregate) ASC LIMIT 1";

        }

        private void ProcessAnswer()
        {
            string answer = tbAnswer.Text;
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
