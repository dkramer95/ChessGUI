using Chess.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chess.Models.Utils;

namespace Chess.Models.Pieces
{
    public class QueenChessPiece : ChessPiece
    {
        public QueenChessPiece(ChessSquare location, ChessColor color) : base(location, color)
        {
        }

        public override MoveDirection[] MoveDirections
        {
            get
            {
                return Moves.ALL;
            }
        }

        public override char Symbol
        {
            get
            {
                return 'Q';
            }
        }

        public override int Value
        {
            get
            {
                return 1000;
            }
        }

        public override string ToString()
        {
            return Color + "_Queen";
        }
    }
}
