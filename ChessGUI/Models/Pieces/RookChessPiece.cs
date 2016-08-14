using Chess.Models.Base;
using Chess.Models.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.Models.Pieces
{
    public class RookChessPiece : ChessPiece
    {
        public RookChessPiece(ChessSquare location, ChessColor color) : base(location, color)
        {
        }

        public override MoveDirection[] MoveDirections
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
