using Chess.Models.Base;
using System.Collections.Generic;
using ChessGUI.Models.Utils;

namespace Chess.Models.Pieces
{
    /// <summary>
    /// This is the ChessPiece implementation for Pawns.
    /// </summary>
    public class PawnChessPiece : ChessPiece
    {
        private Move[] _moveDirection;

        // End rank of this Pawn for Promotion
        public int EndRank { get; private set; }
        public override char Symbol { get { return 'P'; } }
        public override Move[] MoveDirections { get { return _moveDirection; } }
        public override int Value { get { return 1; } }
        public override string ToString() { return Color + "_Pawn"; }


        public PawnChessPiece(ChessSquare location, ChessColor color) : base(location, color)
        {
            Init();
        }

        protected void Init()
        {
            _moveDirection = (Color == ChessColor.LIGHT) ? Moves.NORTH : Moves.SOUTH;
            EndRank = (_moveDirection == Moves.NORTH) ? ChessBoard.MAX_RANK : ChessBoard.MIN_RANK;
        }

        /// <summary>
        /// Checks to see if this PawnChessPiece can be promoted to another ChessPiece
        /// if it has reached its EndRank.
        /// </summary>
        /// <returns>true if this PawnChessPiece can be promoted</returns>
        public bool CanPromote()
        {
            bool canPromote = (Location.Rank == EndRank);
            return canPromote;
        }

        public override bool MoveTo(ChessSquare newLocation)
        {
            bool result = base.MoveTo(newLocation);
            return result;
        }

        public override List<ChessSquare> GetAvailableMoves()
        {
            int limit = (!HasMoved()) ? 2 : 1;

            // Scan vertically and remove any squares where we are blocked
            List<ChessSquare> available = BoardScanner.Scan(this, limit);
            available.RemoveAll(s => s.IsOccupied());

            // Get diagonals and remove any ChessSquares where there is no opponent to capture
            List<ChessSquare> diagonals = BoardScanner.GetDiagonals(Location, MoveDirections[0]);
            diagonals.RemoveAll(s => (!s.IsOccupied() || !IsOpponent(s.Piece)));
            available.AddRange(diagonals);

            return available;
        }
    }
}
