using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChessGUI.Controllers;
using Chess.Models.Pieces;
using Chess.Models.Base;

namespace ChessGUI.Models.SpecialMoves
{
    public class PawnPromotion : SpecialMove
    {

        /// <summary>
        /// Checks to see if a pawn can be promoted to another piece.
        /// </summary>
        public override void Check()
        {
            if (ChessMovement.MovePiece is PawnChessPiece)
            {
                PawnChessPiece pawn = ChessMovement.MovePiece as PawnChessPiece;
                if (pawn.CanPromote())
                {
                    ChessSquare square = pawn.Location;
                    square.Piece = new QueenChessPiece(square, pawn.Color);
                    Game.Controller.SquareViewFromSquare(square).PieceView.SetImageFromPiece(square.Piece);

                    //TODO try fix above so that method call is like:
                    // Controller.UpdateSquare(square).WithPiece(piece);

                    Game.Controller.UpdateSquareView(square, square.Piece);

                    // TODO create a promotion dialog to allow player to choose what they
                    // are promoted to. Right now it's just queen, which is the most common
                    // promotion!
                }
            }
        }
    }
}
