using System;
using System.Collections.Generic;
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
    /// Interaction logic for TestPuzzle.xaml
    /// </summary>
    public partial class TestPuzzle : Page, IPuzzle
    {

        public TestPuzzle()
        {
            InitializeComponent();
        }

        public event EventHandler<EventArgs> PuzzleCompleted;
        public event EventHandler<EventArgs> TimePenalty;

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            PuzzleCompleted?.Invoke(this, EventArgs.Empty);
        }
    }
}
