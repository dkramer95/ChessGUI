using Chess.Models.Base;
using Chess.Models.Pieces;
using ChessGUI.Controllers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ChessGUI.Models.SpecialMoves
{
    public class EnPassant : SpecialMove
    {
        //// valid first move (2 ranks) for capturing en passant
        //private static readonly List<int> ValidRanks = new List<int> { 4, 5 };

        //private static bool ShouldClear { get; set; }

        //// Capture location that an enemy pawn will move to if they capture en passant.
        //// This location is 1 rank back, from the MovedPawn.
        //public static ChessSquare CaptureSquare { get; private set; }

        //// Recently moved pawn that moved 2 ranks
        //public static PawnChessPiece MovedPawn { get; private set; }
        ////public static bool DidCapture { get; set; }

        //private static bool _didCapture;
        //public static bool DidCapture
        //{
        //    get
        //    {
        //        return _didCapture;
        //    }
        //    set
        //    {
        //        _didCapture = value;
        //        Clear();
        //    }
        //}

        public override void Check()
        {
            //CheckCapture();

            //if (DidCapture)
            //{
            //    Game.Controller.UpdateSquareView(MovedPawn.Location, null);
            //    MovedPawn.Location.Piece = null;
            //    ShouldClear = true;
            //}
        }

        // TODO -- Clearing is a little fragile. Be sure to keep testing and clean up a bit.
        // Issue arises when nullifying references, in places they shouldn't be, yet!

        //public static void CheckCapture()
        //{
        //    CheckClear();

        //    // Check to see if last moved piece is a PawnChessPiece
        //    if (ChessMovement.MovePiece is PawnChessPiece)
        //    {
        //        PawnChessPiece pawn = ChessMovement.MovePiece as PawnChessPiece;

        //        // Check to see if pawn moved 2 ranks
        //        if ((pawn.MoveCount == 1) && ValidRanks.Contains(pawn.Location.Rank))
        //        {
        //            UpdateMovedPawn(pawn);
        //        }
        //        // Check to see if pawn moved can capture en passant
        //        else
        //        {
        //            //ShouldClear = true;
        //            Clear();
        //        }
        //    }
        //    else
        //    {
        //        Clear();
        //    }
        //}

        //private static void CheckClear()
        //{
        //    if (ShouldClear)
        //    {
        //        Clear();
        //        ShouldClear = false;
        //    }
        //}

        //public static void Clear()
        //{
        //    if (MovedPawn != null)
        //    {
        //        if (!DidCapture)
        //        {
        //            CaptureSquare.Piece = null;
        //        }
        //        MovedPawn = null;
        //        DidCapture = false;
        //    }
        //}

        ///// <summary>
        ///// Updates this MovedPawn to the pawn that has recently moved 2 units.
        ///// </summary>
        ///// <param name="pawn"></param>
        //private static void UpdateMovedPawn(PawnChessPiece pawn)
        //{
        //    Clear();
        //    MovedPawn = pawn;
        //    UpdateEnPassantLocation();
        //}

        //private static void UpdateEnPassantLocation()
        //{
        //    // determine position for capturing en passant
        //    int direction = (MovedPawn.Color == ChessColor.LIGHT) ? -1 : 1;
        //    CaptureSquare = ChessPiece.Board.SquareAt(MovedPawn.Location.File, MovedPawn.Location.Rank + direction);
        //    CaptureSquare.Piece = MovedPawn;
        //}
    }
}
