using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using SQLBombDisposal.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
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

namespace SQLBombDisposal.Pages.Puzzles
{
    /// <summary>
    /// Interaction logic for AdventurerPuzzle.xaml
    /// </summary>
    public partial class AdventurerPuzzle : Page, IPuzzle
    {
        public event EventHandler<EventArgs> PuzzleCompleted;
        public event EventHandler<EventArgs> Penalize;

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

        public AdventurerPuzzle()
        {
            InitializeComponent();
            rand = new Random();

            function = functions[rand.Next(functions.Count)];
            selector = selectors[rand.Next(selectors.Count)];
            subject = subjects[rand.Next(subjects.Count)];
            targetNumber = rand.Next(1,21);

            questionString = GenerateQuestion();
            solution = GetSolution();

            MessageBox.Show(questionString);
            MessageBox.Show(solution.ToString());
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

            using (SqlBombDisposalContext context = new SqlBombDisposalContext())
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
            string query = $"SELECT {function.ToLower()}(";

            query += function == "COUNT" ? "" : "";
            if(function == "COUNT")
            {
                query += "id) FROM adventurer";
            }
            else
            {
                query += $"{subject.ToLower()}) FROM adventurer";
            }

            if(function == "COUNT" && selector == "ADVENTURER")
            {
                query += $" WHERE {subject.ToLower()} = {targetNumber};";
            }
            else if(function == "COUNT")
            {
                query += $" WHERE {subject.ToLower()} = {targetNumber} AND class = '{selector.ToLower()}';";
            }
            else if(selector != "ADVENTURER")
            {
                query += $" WHERE class = '{selector.ToLower()}';";
            }

            return query;
        }
    }
}
