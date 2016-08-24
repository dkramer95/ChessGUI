using ChessGUI.Controllers;
using System.Windows;
using System.Windows.Controls;

namespace ChessGUI.Dialogs
{
    /// <summary>
    /// Interaction logic for PlayAgainDialog.xaml
    /// </summary>
    public partial class PlayAgainDialog : UserControl
    {
        public Window Window { get; private set; }

        public ChessGame Game { get; private set; }


        /// <summary>
        /// Constructs a new PlayAgain dialog.
        /// </summary>
        /// <param name="game"></param>
        public PlayAgainDialog(ChessGame game)
        {
            Game = game;
            InitializeComponent();
        }

        /// <summary>
        /// Shows this dialog to the screen.
        /// </summary>
        public void Show()
        {
            Window = new Window();
            Window.Width = 300;
            Window.Height = 350;
            Window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            Window.Title = "Game Over. Play Again?";
            Window.Content = this;
            Window.ShowDialog();
        }

        /// <summary>
        /// Closes this dialog.
        /// </summary>
        private void CloseDialog()
        {
            Window.Close();
        }

        /// <summary>
        /// Handles click event for YES.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void yesBtn_Click(object sender, RoutedEventArgs e)
        {
            Game.Reset();
            CloseDialog();
        }

        /// <summary>
        /// Handles click event for NO.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void noBtn_Click(object sender, RoutedEventArgs e)
        {
            CloseDialog();
            Game.App.Quit();
        }
    }
}
