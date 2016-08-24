using ChessGUI.Controllers;
using System.Windows;

namespace ChessGUI
{
    /// <summary>
    /// Main starting point of the application that creates
    /// the ChessGame and viewable Window.
    /// </summary>
    public partial class MainWindow : Window
    {
        public ChessGame Game { get; private set; }


        public MainWindow()
        {
            InitializeComponent();
            Init();
        }

        /// <summary>
        /// Initializes the ChessGame.
        /// </summary>
        private void Init()
        {
            Game = new ChessGame(this);
        }

        /// <summary>
        /// Exits the ChessGame application.
        /// </summary>
        public void Quit()
        {
            Application.Current.Shutdown();
        }
    }
}
