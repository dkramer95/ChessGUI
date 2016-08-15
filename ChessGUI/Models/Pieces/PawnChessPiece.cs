using Chess.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chess.Models.Utils;
using System.Windows;
using ChessGUI.Models.SpecialMoves;
using ChessGUI.Controllers;

namespace Chess.Models.Pieces
{
    /// <summary>
    /// This is the ChessPiece implementation for Pawns.
    /// </summary>
    public class PawnChessPiece : ChessPiece
    {
        private MoveDirection[] _moveDirection;

        // End rank of this Pawn for Promotion
        public int EndRank { get; private set; }

        public override char Symbol
        {
            get
            {
                return 'P';
            }
        }

        public override MoveDirection[] MoveDirections
        {
            get
            {
                return _moveDirection;
            }
        }

        public override int Value
        {
            get
            {
                return 1;
            }
        }

        public override string ToString()
        {
            return Color + "_Pawn";
        }

        public override List<ChessSquare> GetAvailableMoves()
        {
            // allowed to move 2 if first move, otherwise we can only move 1
            int limit = (MoveCount == 0) ? 2 : 1;
            BoardScanner scanner = new BoardScanner(this, limit);
            List<ChessSquare> available = scanner.Scan();

            // remove ChessSquares if blocked vertically
            available.RemoveAll(s => s.IsOccupied());

            // add nearest diagonals as possible moves if they contain an opponent to capture
            available.AddRange(scanner.DiagonalsFrom(Location, MoveDirections[0])
                     .Where(s => s.IsOccupied() && IsOpponent(s.Piece)));

            return available;

        }

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
    }
}
