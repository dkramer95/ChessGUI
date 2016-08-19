using ChessGUI.Controllers;
using System.Windows;

namespace ChessGUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ChessGame _game;

        public MainWindow()
        {
            InitializeComponent();
            Init();
        }

        private void Init()
        {
            _game = new ChessGame();
            boardPanel.Children.Add(_game.Controller.BoardView);
        }
    }
}
