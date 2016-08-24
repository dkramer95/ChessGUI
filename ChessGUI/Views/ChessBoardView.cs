using System.Collections.Generic;
using System.Windows.Controls.Primitives;
using Chess.Models.Base;
using System.Windows.Media;

namespace ChessGUI.Views
{
    /// <summary>
    /// Graphical representation of a ChessBoard.
    /// </summary>
    public class ChessBoardView : UniformGrid
    {
        public List<ChessSquareView> Squares { get; private set; }

        public ChessBoardView()
        {
            Columns = ChessBoard.COL_COUNT;
            Rows    = ChessBoard.ROW_COUNT;
            Squares = new List<ChessSquareView>();
        }

        /// <summary>
        /// Utility method to extract the correct Brush color from the specified
        /// ChessColor.
        /// </summary>
        /// <param name="color">ChessColor to check and convert into a brush</param>
        /// <returns>A ChessSquare Brush style</returns>
        public static Brush GetBrushFromChessColor(ChessColor color)
        {
            Brush brush = (color == ChessColor.WHITE) ? SquareStyles.BRUSH_LIGHT : SquareStyles.BRUSH_DARK;
            return brush;
        }

        /// <summary>
        /// Adds a SquareView to our list of Squares.
        /// </summary>
        /// <param name="squareView">ChessSquareView to add</param>
        public void AddSquare(ChessSquareView squareView)
        {
            Squares.Add(squareView);
            Children.Add(squareView);
        }
    }
}
