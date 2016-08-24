using ChessGUI.Controllers;
using ChessGUI.Models.SpecialMoves;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace ChessGUI.Dialogs
{
    /// <summary>
    /// Dialog that appears when a PawnChessPiece is being promoted. This allows
    /// the user to choose what piece they wish to promote their Pawn too.
    /// </summary>
    public partial class PromotionDialog : UserControl
    {
        public ChessGame Game { get; private set; }

        public PromotionDialog(ChessGame game)
        {
            InitializeComponent();
            Game = game;
        }

        /// <summary>
        /// Updates the PawnPromotion with the the selected radio button value.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void promoteBtn_Click(object sender, RoutedEventArgs e)
        {
            var checkedButton = contentPanel.Children.OfType<RadioButton>().FirstOrDefault(r => (bool)r.IsChecked);

            // use char from name for ChessPiece factory
            string name = checkedButton.Content.ToString();
            char symbol = (checkedButton != knightRadioBtn) ? name[0] : name[1];

            PawnPromotion.SetPromotionFromSymbol(symbol);
            DismissDialog();
        }

        /// <summary>
        /// Dismisses this Promotion Dialog.
        /// </summary>
        private void DismissDialog()
        {
            Window w = (Window)Parent;
            w.Close();
        }
    }
}
