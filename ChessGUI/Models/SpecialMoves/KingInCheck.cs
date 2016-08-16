using Chess.Models.Base;
using Chess.Models.Pieces;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace ChessGUI.Models.SpecialMoves
{
    public class KingInCheck : SpecialMove
    {
        public override void Check()
        {
            IsInCheck();
            IsCheckMate();
        }

        /// <summary>
        /// Checks to see if the Opponent King Is in Check.
        /// </summary>
        /// <returns>True if King is in Check</returns>
        public static bool IsInCheck()
        {
            KingChessPiece king = Game.GetOpponent(Game.ActivePlayer).KingPiece;

            if (Game.GetEnemyMoves(king).Contains(king.Location))
            {
                king.InCheck = true;
                Game.ToggleCheck(king);
                MessageBox.Show("King in check!");
            }
            else
            {
                king.InCheck = false;
            }
            return king.InCheck;
        }

        public static bool IsCheckMate()
        {
            bool isCheckMate = false;

            ChessPlayer opponent = Game.GetOpponent(Game.ActivePlayer);
            KingChessPiece king = opponent.KingPiece;
            
            if (king.InCheck)
            {
                List<ChessSquare> moves = new List<ChessSquare>();

                Game.GetOpponent(Game.ActivePlayer).Pieces.ForEach(p =>
                {
                    moves.AddRange(GetValidMoves(p, king));
                });

                // Debug crap -- remove later
                StringBuilder sb = new StringBuilder();
                moves.ForEach(s => sb.Append(s + ","));
                MessageBox.Show("COUNT: " + moves.Count + " list: " + sb.ToString());
            }
            return isCheckMate;
        }


        /// <summary>
        /// Checks to make sure movePiece belongs to the active player and that
        /// the moves they make wouldn't threaten the player's KingChessPiece.
        /// </summary>
        /// <param name="movePiece"></param>
        /// <param name="moves"></param>
        public static void TestMoves(ChessPiece movePiece, ref List<ChessSquare> moves)
        {
            // Make sure we're testing movement against a piece that belongs to ActivePlayer
            if (Game.IsActivePlayerPiece(movePiece))
            {
                KingChessPiece king = Game.ActivePlayer.KingPiece;

                moves = (movePiece != king) ? 
                        GetValidMoves(movePiece, king) : 
                        GetSafeMoves(king, king.GetAvailableMoves());
            }
        }

        /// <summary>
        /// Gets all valid moves for the specified movePiece, such that they don't
        /// allow the KingChessPiece to be captured, if they move.
        /// </summary>
        /// <param name="movePiece">MovePiece to check</param>
        /// <param name="king">KingChessPiece to check</param>
        /// <returns>List of all safe moves, so that the KingChessPiece wouldn't be captured</returns>
        private static List<ChessSquare> GetValidMoves(ChessPiece movePiece, KingChessPiece king)
        {
            List<ChessSquare> validMoves = new List<ChessSquare>();
            List<ChessSquare> available = movePiece.GetAvailableMoves();

            // Pretend to move, by emptying out square that movePiece is on
            ChessSquare movePieceSquare = movePiece.Location;
            movePieceSquare.Piece = null;

            foreach (ChessSquare s in available)
            {
                // Test movement
                ChessPiece original = s.Piece;
                s.Piece = movePiece;

                List<ChessSquare> enemyMoves = Game.GetEnemyMoves(movePiece);

                // if we move and the enemy can't move to the king's location or if they can
                // but we can capture it, then we're safe
                if (!enemyMoves.Contains(king.Location))
                {
                    validMoves.Add(s);
                }
                // Clear out test movement
                s.Piece = original;
            }
            movePieceSquare.Piece = movePiece;
            return validMoves;
        }

        /// <summary>
        /// Updates the list of KingChessPiece's available moves by removing any
        /// moves that would place the King at risk for being captured.
        /// </summary>
        /// <param name="king"></param>
        /// <param name="available"></param>
        public static void RemoveUnsafe(KingChessPiece king, ref List<ChessSquare> available)
        {
            if (Game.IsActivePlayerPiece(king))
            {
                available = GetSafeMoves(king, available);
            }
        }
        
        /// <summary>
        /// Gets all the available safe moves that a KingChessPiece can make, such
        /// that he is not under threat of being captured.
        /// </summary>
        /// <param name="king">KingChessPiece</param>
        /// <param name="available">List of legal moves KingChessPiece can make</param>
        /// <returns>List of safe moves KingChessPiece can make</returns>
        private static List<ChessSquare> GetSafeMoves(KingChessPiece king, List<ChessSquare> available)
        {
            List<ChessSquare> safeMoves = new List<ChessSquare>();
            // Pretend to move, by emptying out square that movePiece is on
            ChessSquare kingSquare = king.Location;
            kingSquare.Piece = null;

            available.ForEach(s =>
            {
                // Test movement
                ChessPiece original = s.Piece;
                s.Piece = king;

                if (!Game.GetEnemyMoves(king).Contains(s))
                {
                    safeMoves.Add(s);
                }
                // Clear out test movement
                s.Piece = original;
            });
            kingSquare.Piece = king;
            return safeMoves;
        }
    }
}
