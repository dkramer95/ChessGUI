using ChessGUI.Controllers;
using System.Windows;

namespace ChessGUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //private ChessBoardController _controller;
        private NewChessGame _game;

        public MainWindow()
        {
            InitializeComponent();
            Init();
        }

        private void Init()
        {
            //_controller = new ChessBoardController();
            //Content = _controller.BoardView;

            _game = new NewChessGame();
            Content = _game.Controller.BoardView;
        }
    }
}
