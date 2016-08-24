using Chess.Models.Base;
using System.Collections.Generic;
using ChessGUI.Models.Utils;

namespace Chess.Models.Pieces
{
    public class KnightChessPiece : ChessPiece
    {        
        public override char Symbol { get { return 'N'; } }
        public override Move[] MoveDirections { get { return Moves.HORIZ_VERT; } protected set { } }
        public override int Value { get { return 350; } }
        public override string ToString() { return Color + "_Knight"; }


        public KnightChessPiece(ChessSquare location, ChessColor color) : base(location, color)
        {
        }

        /// <summary>
        /// Gets list of available moves for this KnightChessPiece where the available
        /// pieces are those that are the result of moving in a right angle 'L' shape.
        /// </summary>
        /// <returns>List of available moves for this KnightChessPiece</returns>
        public override List<ChessSquare> GetAvailableMoves()
        {
            List<ChessSquare> available = BoardScanner.ScanBranched(this, 1);
            return available;
        }
    }
}
