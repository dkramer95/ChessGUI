using Chess.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chess.Models.Utils;
using ChessGUI.Models.Base;
using System.Windows;
using ChessGUI.Models.SpecialMoves;
using ChessGUI.Controllers;

namespace Chess.Models.Pieces
{
    /// <summary>
    /// This is the ChessPiece implementation for Pawns.
    /// 
    /// ::MOVEMENT_RULES::
    /// For their first move, a Pawn may move up to two ChessTiles, and thereafter they
    /// may only move one ChessSquare at a time.
    /// 
    /// ::PROMOTION::
    /// If a pawn advances to the end rank (8 for LIGHT, 1 for DARK), it can be promoted.
    /// A promotion means this Pawn can be exchanged for any other piece, with the exception
    /// of another Pawn or a King.
    /// 
    /// ::CAPTURING::
    /// Pawns capture by moving one square diagonally, either to the right or left. Pawns
    /// CANNOT capture, by moving straight foreward. Captures can only happen on pieces of
    /// the opposite color of this Pawn.
    /// 
    /// ::VALUE::
    /// Pawns have a value of 1.
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
            //CheckCaptureEnPassant(newLocation);
            return result;
        }

        private void CheckCaptureEnPassant(ChessSquare newLocation)
        {
            //if (newLocation == EnPassant.CaptureSquare)
            //{
            //    EnPassant.DidCapture = true;
            //    newLocation.Piece = this;
            //}
        }

        static class EnPassantTest
        {
            private static readonly List<int> ValidRanks = new List<int> { 4, 5 };

            // Capture location that an enemy pawn will move to, if they capture en passant.
            public static ChessSquare CaptureSquare { get; private set; }

            public static bool DidCaptureEnPassant { get; set; }

            public static void CheckCapture()
            {
                
            }
        }
    }
}
