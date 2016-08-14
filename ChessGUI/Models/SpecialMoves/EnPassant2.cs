using Chess.Models.Base;
using Chess.Models.Pieces;
using ChessGUI.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ChessGUI.Models.SpecialMoves
{
    public class EnPassant2 : SpecialMove
    {
        private static readonly int[] ValidRanks = { 4, 5 };

        // Capture location that an enemy pawn will move to if they capture en passant
        public static ChessSquare CaptureSquare { get; private set; }

        // Pawn that recently moved 2 ranks and can be captured en passant
        public static PawnChessPiece MovedPawn { get; private set; }

        public override void Check()
        {
            CheckCapture();
        }

        private static void CheckCapture()
        {
            if (ChessMovement.MovePiece is PawnChessPiece)
            {
                PawnChessPiece pawn = ChessMovement.MovePiece as PawnChessPiece;

                if (DidMoveTwoRanks(pawn))
                {
                    Update(pawn);
                } else if ((pawn.Location == CaptureSquare))
                {
                    Debug.PrintMsg("Captured en passant");
                    MovedPawn.Location.Piece = null;
                } else
                {
                    // TODO fix invalid clearing!!!
                    Clear();
                    //CaptureSquare.Piece = null;
                }
            } else
            {
                Clear();
            }
        }

        /// <summary>
        /// Updates the MovedPawn and CaptureSquare.
        /// </summary>
        /// <param name="pawn"></param>
        private static void Update(PawnChessPiece pawn)
        {
            Clear();
            MovedPawn = pawn;
            UpdateCaptureSquare();
        }

        private static void Clear()
        {
            // Clear out previously moved pawn if exists
            if (MovedPawn != null)
            {
                CaptureSquare.Piece = null;
                MovedPawn = null;
            }
        }

        /// <summary>
        /// Updates the CaptureSquare, which is the ChessSquare that an enemy
        /// PawnChessPiece will land, if they capture the MovedPawn en passant.
        /// </summary>
        private static void UpdateCaptureSquare()
        {
            int direction = (MovedPawn.Color == ChessColor.LIGHT) ? -1 : 1;
            ChessSquare square = MovedPawn.Location;
            CaptureSquare = Game.Controller.BoardModel.SquareAt(square.File, square.Rank + direction);

            CaptureSquare.Piece = MovedPawn;
        }

        /// <summary>
        /// Checks to see if the PawnChessPiece moved 2 ranks so that it can be
        /// captured en passant.
        /// </summary>
        /// <param name="pawn">PawnChessPiece to check</param>
        /// <returns>true if PawnChessPiece moved 2 ranks</returns>
        private static bool DidMoveTwoRanks(PawnChessPiece pawn)
        {
            bool didMoveTwoRanks = (pawn.MoveCount == 1) && ValidRanks.Contains(pawn.Location.Rank);
            return didMoveTwoRanks;
        }
    }
}
