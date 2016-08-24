using System.Collections.Generic;
using ChessGUI.Controllers;
using Chess.Models.Pieces;
using Chess.Models.Base;

namespace ChessGUI.Models.SpecialMoves
{
    /// <summary>
    /// This class handles checking for PawnPromotion. Pawns can be promoted as
    /// soon as they reach the end rank of their enemy, and can be promoted to
    /// any piece except for a King or another Pawn.
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
            if (MoveController.MovePiece is PawnChessPiece)
            {
                PawnChessPiece pawn = MoveController.MovePiece as PawnChessPiece;

                if (pawn.CanPromote())
                {
                    Pawn = pawn;
                    Game.ShowPromotionDialog();
                }
            }
        }

        /// <summary>
        /// Creates a promoted ChessPiece from the specified symbol representation.
        /// </summary>
        /// <param name="symbol">Symbol representation of ChessPiece to create</param>
        public static void SetPromotionFromSymbol(char symbol)
        {
            // Get Chesspiece based off symbol
            ChessPiece promotionPiece = 
                ChessUtils.PieceFromSymbol(Pawn.Location, Pawn.Color, symbol);

            // update to finish promotion
            Update(promotionPiece);
        }

        /// <summary>
        /// Updates the pawn to a PromotedPiece.
        /// </summary>
        /// <param name="promotionPiece"></param>
        private static void Update(ChessPiece promotionPiece)
        {
            // Update Player pieces to replace old pawn with promoted pawn
            UpdatePlayerPieces(promotionPiece);
            MoveController.MovePiece = promotionPiece;
            promotionPiece.Location.Piece = promotionPiece;
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
