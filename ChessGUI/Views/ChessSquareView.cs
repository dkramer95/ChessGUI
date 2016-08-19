using Chess.Models.Base;
using ChessGUI.Controllers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ChessGUI.Views
{
    /// <summary>
    /// This class represents a single individual graphical view of a ChessSquare.
    /// </summary>
    public class ChessSquareView : Button
    {
        // Preserve original color, for cases of highlighting
        private Brush _bgColor;

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
        /// Constructs a new ChessSquareView with a specified PieceView
        /// imagePath to render a ChessPiece onto this ChessSquareView.
        /// </summary>
        /// <param name="color">Background color to assign</param>
        /// <param name="imgPath">image path for ChessPieceView</param>
        //public ChessSquareView(Brush color, string imgPath)
        //{
        //    Background = color;
        //    PieceView = new ChessPieceView(imgPath);
        //    Init();
        //}

        /// <summary>
        /// Initializes this ChessSquareView with proper coloring and margins.
        /// </summary>
        private void Init()
        {
            _bgColor = Background;
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
            Background = (IsHighlighted) ? SquareStyles.HIGHLIGHT_COLOR : _bgColor;
        }

        /// <summary>
        /// Toggles between this ChessSquareView preview and regular Background color.
        /// </summary>
        public void TogglePreview()
        {
            Background = (Background == SquareStyles.PREVIEW_COLOR) ? _bgColor : SquareStyles.PREVIEW_COLOR;
        }

        public void ToggleCheck()
        {
            Background = SquareStyles.CHECK_COLOR;
        }

        /// <summary>
        /// Sets the Background to the original bgcolor.
        /// </summary>
        public void ResetBackground()
        {
            Background = _bgColor;
        }
    }
}
