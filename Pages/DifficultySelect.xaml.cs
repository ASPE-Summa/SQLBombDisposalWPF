using System.Windows.Controls;

namespace SQLBombDisposal.Pages
{
    /// <summary>
    /// Interaction logic for DifficultySelect.xaml
    /// </summary>
    public partial class DifficultySelect : Page
    {
        public DifficultySelect()
        {
            InitializeComponent();
        }

        private void SelectDifficulty(object sender, System.Windows.RoutedEventArgs e)
        {
            Button button = sender as Button;
            NavigationService.Navigate(new GamePage());
        }
    }
}
