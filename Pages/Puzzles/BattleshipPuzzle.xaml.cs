using SQLBombDisposal.Models;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace SQLBombDisposal.Pages.Puzzles
{
    /// <summary>
    /// Interaction logic for BattleshipPuzzle.xaml
    /// </summary>
    public partial class BattleshipPuzzle : Page, IPuzzle
    {
        public event EventHandler<EventArgs> PuzzleCompleted;
        public event EventHandler<EventArgs> Penalize;

        private Battleship solution;

        public string lipsum = "lorem ipsum dolor sit amet consectetur adipiscing elit maecenas magna nisi ultricies non erat non accumsan placerat nunc vestibulum mauris mi vulputate vel lectus at aliquam mattis diam pellentesque ornare sapien a tempus tristique cras convallis nulla nibh ut condimentum justo porttitor vitae integer vitae metus ac sem finibus gravida nullam consequat finibus velit ut dignissim massa blandit vitae sed ut massa vel ligula rutrum ornare interdum et malesuada fames ac ante ipsum primis in faucibus phasellus nec eros sed turpis varius gravida vestibulum venenatis eleifend metus ac fermentum augue scelerisque et maecenas mi enim bibendum fringilla vulputate sit amet volutpat et elit donec posuere leo in justo porta iaculis morbi purus augue mollis sit amet lobortis nec rutrum eu nisl duis ornare commodo neque sed hendrerit nibh ultricies ac aenean pulvinar dapibus massa quis malesuada aliquam suscipit lacus sed ornare dignissim vestibulum eu ligula arcu pellentesque nibh mi feugiat id placerat et gravida vel diam vestibulum fermentum aliquam lectus a bibendum velit pretium et nunc consectetur mauris sed aliquet finibus nibh ipsum placerat ligula eget posuere enim turpis eget leo duis interdum aliquam dignissim duis dapibus leo at vestibulum ultrices nisi libero mollis erat a laoreet velit erat ut nisl pellentesque nisl nunc egestas sit amet porta sed condimentum et felis etiam ut eros sapien fusce elementum ex pharetra sem cursus vulputate morbi commodo metus sem quis varius purus vulputate eu mauris at ante quis felis pulvinar auctor proin id urna nec orci auctor ornare nam scelerisque risus convallis feugiat dapibus odio leo varius ex a pulvinar orci justo sed massa nunc pretium consequat fermentum praesent pharetra ante et ante euismod egestas class aptent taciti sociosqu ad litora torquent per conubia nostra per inceptos himenaeos nunc at sodales risus ut pharetra est vitae rutrum faucibus orci nibh sollicitudin ligula a sollicitudin libero leo a lacus donec viverra tortor et convallis aliquet nisi est eleifend nunc eu pulvinar arcu est nec ante aliquam lectus nulla suscipit consectetur pharetra non volutpat nec ligula maecenas maximus tortor quis sodales mattis donec elementum elementum turpis dignissim lacinia fusce accumsan eros et eros malesuada sit amet hendrerit lorem cursus suspendisse libero dui vulputate vulputate erat eget placerat varius neque curabitur quis erat lectus pellentesque et elit ullamcorper metus aliquam tincidunt morbi pulvinar nisl justo vitae ornare libero viverra sit amet vestibulum cursus purus non consectetur pellentesque nisl est rhoncus libero sodales fringilla quam augue fermentum ante fusce auctor elementum purus sit amet aliquam pellentesque quis interdum nisi";

        public BattleshipPuzzle()
        {
            InitializeComponent();
            InitializeGrid();
            SelectRandomPuzzle();
        }
        public void InitializeGrid()
        {
            for (int row = 0; row < 10; row++)
            {
                for (int column = 0; column < 10; column++)
                {
                    Border bord = new Border();
                    bord.BorderBrush = Brushes.Black;
                    bord.BorderThickness = new Thickness(1);
                    System.Windows.Controls.Button b = new System.Windows.Controls.Button();
                    b.Click += Battleship_Button_Click;
                    b.Background = Brushes.Gray;
                    b.Padding = new Thickness(0, 0, 0, 0);
                    b.FontSize = 10;
                    char c = (char)(97 + column);
                    b.Content = c.ToString() + row.ToString();
                    Grid.SetRow(b, row);
                    Grid.SetColumn(b, column);
                    buttonGrid.Children.Add(b);
                }
            }
        }
        private void Battleship_Button_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.Button? battleshipButton = sender as System.Windows.Controls.Button;
            string content = battleshipButton.Content.ToString();
            if(content == solution.Coordinates.ToString())
            {
                PuzzleCompleted.Invoke(this, new EventArgs());
            }

            else
            {
                battleshipButton.IsEnabled = false;
                battleshipButton.Background = Brushes.Red;
                Penalize.Invoke(this, new EventArgs());
            }
        }

        private void SelectRandomPuzzle()
        {
            using (SqlBombDisposalContext context = new SqlBombDisposalContext())
            {
                solution = context.Battleships.AsEnumerable().OrderBy(p => Guid.NewGuid()).First();
            }
            ExtractWords();
        }

        private void ExtractWords()
        {
            string[] words = solution.Description.Split(' ');

            words = words.Where(x => !lipsum.Contains(x)).ToArray();
            tbWord1.Text = words[0];
            tbWord2.Text = words[1];
            tbWord3.Text = words[2];
        }
    }
}