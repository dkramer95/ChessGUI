using Chess.Models.Base;
using Chess.Models.Pieces;
using Chess.Models.Utils;
using ChessGUI.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ChessGUI.Models.Base
{
    /// <summary>
    /// This class manages the capturing of pawns en passant, which is when a
    /// PawnChessPiece moves two ranks forward from its starting position and
    /// an enemy pawn could have captured it if it had only moved one rank
    /// forward. En passant capture must be made at the next turn in the game
    /// otherwise the right to do so is lost.
    /// </summary>
    public static class PawnEnPassant
    {
        // valid first move (2 ranks) for capturing en passant
        private static readonly List<int> ValidRanks = new List<int> { 4, 5 };

        private static bool ShouldClear { get; set; }

        // Capture location that an enemy pawn will move to if they capture en passant.
        // This location is 1 rank back, from the MovedPawn.
        public static ChessSquare CaptureSquare { get; private set; }

        // Recently moved pawn that moved 2 ranks
        public static PawnChessPiece MovedPawn { get; private set; }

        public static bool DidCapture { get; set; }

        public static void CheckCapture()
        {
            CheckClear();

            // Check to see if last moved piece is a PawnChessPiece
            if (ChessMovement.MovePiece is PawnChessPiece)
            {
                PawnChessPiece pawn = ChessMovement.MovePiece as PawnChessPiece;

                // Check to see if pawn moved 2 ranks
                if ((pawn.MoveCount == 1) && ValidRanks.Contains(pawn.Location.Rank))
                {
                    UpdateMovedPawn(pawn);
                }
                // Check to see if pawn moved can capture en passant
                else
                {
                    ShouldClear = true;
                }
            }
            else
            {
                Clear();
            }
        }

        private static void CheckClear()
        {
            if (ShouldClear)
            {
                Clear();
                ShouldClear = false;
            }
        }

        public static void Clear()
        {
            if (MovedPawn != null)
            {
                MessageBox.Show("Cleared");
                CaptureSquare.Piece = null;
                MovedPawn = null;
                DidCapture = false;
            }
        }

        /// <summary>
        /// Updates this MovedPawn to the pawn that has recently moved 2 units.
        /// </summary>
        /// <param name="pawn"></param>
        private static void UpdateMovedPawn(PawnChessPiece pawn)
        {
            if (MovedPawn != null)
            {
                Clear();
            }
            MovedPawn = pawn;
            UpdateEnPassantLocation();
        }

        private static void UpdateEnPassantLocation()
        {
            // determine position for capturing en passant
            int direction = (MovedPawn.Color == ChessColor.LIGHT) ? -1 : 1;
            CaptureSquare = ChessPiece.Board.SquareAt(MovedPawn.Location.File, MovedPawn.Location.Rank + direction);
            CaptureSquare.Piece = MovedPawn;
        }
    }
}
