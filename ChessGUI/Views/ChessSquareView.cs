using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ChessGUI.Views
{
    /// <summary>
    /// This class represents a single individual graphical view of a ChessSquare,
    /// which also contains a ChessPieceView for displaying a ChessPiece to the screen.
    /// </summary>
    public class ChessSquareView : Button
    {
        // Preserve original color, for cases of highlighting
        public Brush BGColor { get; private set; }

        public bool IsHighlighted { get; private set; }

        // ChessSquare image graphic
        public ChessPieceView PieceView { get; set; }

        /// <summary>
        /// Constructs a new ChessSquareView with an empty PieceView.
        /// </summary>
        /// <param name="color">Background color to assign</param>
        public ChessSquareView(Brush color)
        {
            Background = color;
            PieceView = new ChessPieceView();
            Init();
        }

        /// <summary>
        /// Initializes this ChessSquareView with proper coloring and margins.
        /// </summary>
        private void Init()
        {
            BGColor = Background;
            BorderBrush = Brushes.Black;
            Margin = new Thickness(-1);
            Content = PieceView;
        }

        /// <summary>
        /// Toggles between this ChessSquareView highlight and regular Background color.
        /// </summary>
        public void ToggleHighlight()
        {
            IsHighlighted = (IsHighlighted) ? false : true;
            Background = (IsHighlighted) ? SquareStyles.HIGHLIGHT_COLOR : BGColor;
        }

        /// <summary>
        /// Toggles between this ChessSquareView preview and regular Background color.
        /// </summary>
        public void TogglePreview()
        {
            Background = (Background == SquareStyles.PREVIEW_COLOR) ? BGColor : SquareStyles.PREVIEW_COLOR;
        }

        /// <summary>
        /// Sets the background of this ChessSquareView to the Color representing
        /// a King InCheck.
        /// </summary>
        public void SetCheck()
        {
            Background = SquareStyles.CHECK_COLOR;
        }

        /// <summary>
        /// Sets the Background to the original bgcolor.
        /// </summary>
        public void ResetBackground()
        {
            Background = BGColor;
        }
    }
}
