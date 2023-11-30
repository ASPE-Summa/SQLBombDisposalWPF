using SQLBombDisposal.Pages;
using System.Windows;

namespace SQLBombDisposal
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            mainFrame.NavigationService.Navigate(new ConfigPage());
        }
    }
}
