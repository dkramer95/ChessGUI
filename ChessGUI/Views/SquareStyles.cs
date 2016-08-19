using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace ChessGUI.Views
{
    public static class SquareStyles
    {
        // Color to help indicate this ChessSquareView is selected.
        public static Brush HIGHLIGHT_COLOR = Brushes.LightGreen;

        // Color to help indicate that this ChessSquareView is available to move to
        public static Brush PREVIEW_COLOR = Brushes.MediumAquamarine;

        public static Brush CHECK_COLOR = Brushes.Red;

        // Background ChessSquare styles
        public static Brush BRUSH_LIGHT = Brushes.AliceBlue;
        public static Brush BRUSH_DARK = Brushes.DeepSkyBlue;
    }
}
