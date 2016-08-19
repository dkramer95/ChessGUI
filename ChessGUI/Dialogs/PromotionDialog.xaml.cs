using ChessGUI.Controllers;
using ChessGUI.Models.SpecialMoves;
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

namespace ChessGUI.Dialogs
{
    /// <summary>
    /// Interaction logic for PromotionDialog.xaml
    /// </summary>
    public partial class PromotionDialog : UserControl
    {
        public ChessGame Game { get; private set; }
        public PromotionDialog(ChessGame game)
        {
            InitializeComponent();
            Game = game;
        }

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
