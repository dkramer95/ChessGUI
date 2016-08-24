using System.Windows.Media;

namespace ChessGUI.Views
{
    /// <summary>
    /// This class contains a list of ChessSquare styles that are used for
    /// representing different states of a ChessSquare.
    /// </summary>
    public static class SquareStyles
    {
        // Color to help indicate this ChessSquareView is selected.
        public static Brush HIGHLIGHT_COLOR = Brushes.LightGreen;

        // Color to help indicate that this ChessSquareView is available to move to
        public static Brush PREVIEW_COLOR = Brushes.MediumAquamarine;

        // Color to help indicate that the King is in check.
        public static Brush CHECK_COLOR = Brushes.Red;

        // Background ChessSquare styles
        public static Brush BRUSH_LIGHT = Brushes.AliceBlue;
        public static Brush BRUSH_DARK = Brushes.DeepSkyBlue;
    }
}
