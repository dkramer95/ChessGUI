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
    /// <summary>
    /// This class handles checking for PawnPromotion.
    /// </summary>
    public class PawnPromotion : SpecialMove
    {

        // Pawn chess piece that we can promote
        public static PawnChessPiece Pawn { get; private set; }

        /// <summary>
        /// Checks to see if a pawn can be promoted to another piece.
        /// </summary>
        public override void Check()
        {
            if (MovementController.MovePiece is PawnChessPiece)
            {
                PawnChessPiece pawn = MovementController.MovePiece as PawnChessPiece;

                if (pawn.CanPromote())
                {
                    Pawn = pawn;
                    Game.ShowPromotionDialog();
                }
            }
        }

        public static void SetPromotionFromSymbol(char symbol)
        {
            // Get Chesspiece based off symbol
            ChessPiece promotionPiece = 
                ChessUtils.PieceFromSymbol(Pawn.Location, Pawn.Color, symbol);

            // update to finish promotion
            Update(promotionPiece);
        }

        private static void Update(ChessPiece promotionPiece)
        {
            // Update Player pieces to replace old pawn with promoted pawn
            UpdatePlayerPieces(promotionPiece);
            MovementController.MovePiece = promotionPiece;
            // Clear out old pawn
            Pawn = null;

            // Update view with new piece
            Game.Controller.UpdateSquareView(promotionPiece.Location, promotionPiece);
        }

        /// <summary>
        /// Updates the ActivePlayer's pieces and replaces out the pawn, with
        /// the new promoted pawn ChessPiece.
        /// </summary>
        /// <param name="pawn">Pawn we're replacing</param>
        /// <param name="newPiece">ChessPiece we're replacing pawn with</param>
        private static void UpdatePlayerPieces(ChessPiece newPiece)
        {
            List<ChessPiece> playerPieces = Game.ActivePlayer.Pieces;
            int pieceIndex = playerPieces.IndexOf(Pawn);
            playerPieces[pieceIndex] = newPiece;
        }
    }
}
