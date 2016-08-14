using Chess.Models.Base;
using System;
using System.Collections.Generic;
using Chess.Models.Utils;

namespace Chess.Models.Pieces
{
    public class KnightChessPiece : ChessPiece
    {
        public KnightChessPiece(ChessSquare location, ChessColor color) : base(location, color)
        {
        }
        

        public override char Symbol
        {
            get
            {
                return 'N';
            }
        }

        public override MoveDirection[] MoveDirections
        {
            get
            {
                return Moves.HORIZ_VERT;
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
            return Color + "_Knight";
        }

        public override List<ChessSquare> GetAvailableMoves()
        {
            List<ChessSquare> available = new List<ChessSquare>();
            BoardScanner scanner = new BoardScanner(this);

            available.AddRange(scanner.ScanBranched());

            return available;
        }
    }
}
