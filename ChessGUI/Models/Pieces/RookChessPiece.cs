using Chess.Models.Base;
using ChessGUI.Models.Utils;

namespace Chess.Models.Pieces
{
    public class RookChessPiece : ChessPiece
    {
        public RookChessPiece(ChessSquare location, ChessColor color) : base(location, color)
        {
        }

        public override Move[] MoveDirections
        {
            get
            {
                return Moves.HORIZ_VERT;
            }
        }

        public override char Symbol
        {
            get
            {
                return 'R';
            }
        }

        public override int Value
        {
            get
            {
                return 525;
            }
        }

        public override string ToString()
        {
            return Color + "_Rook";
        }
    }
}
