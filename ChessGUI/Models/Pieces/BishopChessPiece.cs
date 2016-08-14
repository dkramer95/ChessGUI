using Chess.Models.Base;
using Chess.Models.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.Models.Pieces
{
    public class BishopChessPiece : ChessPiece
    {
        public BishopChessPiece(ChessSquare location, ChessColor color) : base(location, color)
        {
        }

        public override MoveDirection[] MoveDirections
        {
            get
            {
                return Moves.DIAGONAL;
            }
        }

        public override char Symbol
        {
            get
            {
                return 'B';
            }
        }

        public override int Value
        {
            get
            {
                return 350;
            }
        }

        public override string ToString()
        {
            return Color + "_Bishop";
        }
    }
}
