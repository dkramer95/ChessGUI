using Chess.Models.Base;
using Chess.Models.Pieces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace ChessGUI.Models.SpecialMoves
{
    public class KingInCheck : SpecialMove
    {
        public override void Check()
        {
            IsInCheck();
        }

        public static void IsInCheck()
        {
            ChessPlayer activePlayer = Game.ActivePlayer;
            KingChessPiece king = Game.GetOpponent(activePlayer).KingPiece;

            List<ChessSquare> opponentMoves = Game.Controller.BoardModel.GetEnemyMoves(king);
            if (opponentMoves.Contains(king.Location))
            {
                MessageBox.Show("KING IN CHECK!");
                //Game.Controller.SquareViewFromSquare(king.Location).Background = Brushes.Red;

                int index = Game.Controller.BoardModel.Squares.IndexOf(king.Location);
                Game.Controller.BoardView.Squares[index].ToggleCheck();
                king.InCheck = true;
            }

        }
        
        /// <summary>
        /// Checks and removes any unsafe moves that would the 
        /// Player's KingChessPiece at risk.
        /// </summary>
        /// <param name="movePiece">Piece we're trying to move and check</param>
        /// <param name="moves"></param>
        //public static void TestMoves(ChessPiece movePiece, ref List<ChessSquare> moves)
        //{
        //    if (Game.ActivePlayer.Color == movePiece.Color)
        //    {
        //        List<ChessSquare> validMoves = new List<ChessSquare>();
        //        IDictionary<ChessSquare, ChessPiece> originalSquares = new Dictionary<ChessSquare, ChessPiece>();
        //        ChessSquare movePieceSquare = movePiece.Location;

        //        // Preserve squares and pieces and temporarily set each square to
        //        // the piece we're moving to check and see if it makes the king vulnerable
        //        moves.ForEach(s =>
        //        {
        //            originalSquares.Add(s, s.Piece);
        //            s.Piece = movePiece;
        //        });

        //        List<ChessSquare> enemyMoves = Game.Controller.BoardModel.GetEnemyMoves(movePiece);
        //        KingChessPiece king = Game.ActivePlayer.KingPiece;

        //        // Check and see that if after moving, our king is vulnerable to capture

        //        //moves.ForEach(s =>
        //        //{
        //        //    if (!enemyMoves.Contains(s) && !enemyMoves.Contains(king.Location))
        //        //    {
        //        //        validMoves.Add(s);
        //        //    }
        //        //});

        //        moves.ForEach(s =>
        //        {
        //            if (originalSquares.ContainsKey(s))
        //            {
        //                ChessPiece piece = null;
        //                originalSquares.TryGetValue(s, out piece);
        //                s.Piece = piece;
        //            }
        //        });

        //        moves = validMoves;
        //    }
        //}

        public static void TestMoves(ChessPiece movePiece, ref List<ChessSquare> moves)
        {
            if (Game.ActivePlayer.Color == movePiece.Color)
            {
                KingChessPiece king = Game.ActivePlayer.KingPiece;
                List<ChessSquare> validMoves = new List<ChessSquare>();
                List<ChessSquare> opponentMoves = null;

                // Pretend to move, by making the square of the piece we're moving empty
                ChessSquare movePieceSquare = movePiece.Location;
                movePieceSquare.Piece = null;

                // For every legal move, check to see if by making that move it would threaten
                // the king
                moves.ForEach(s =>
                {
                    ChessPiece original = s.Piece;
                    s.Piece = movePiece;

                    opponentMoves = Game.Controller.BoardModel.GetEnemyMoves(movePiece);

                    // is valid -- doesn't threaten king
                    if (!opponentMoves.Contains(king.Location))
                    {
                        validMoves.Add(s);
                    }
                    // revert back
                    s.Piece = original;
                });
                movePieceSquare.Piece = movePiece;

                moves = validMoves;
            }
        }


        // TODO - this works, but try to refactor. It's a little too long!

        public static void RemoveUnsafe(KingChessPiece king, ref List<ChessSquare> available)
        {
            if (Game.ActivePlayer.KingPiece == king)
            {
                List<ChessSquare> safeKingMoves = new List<ChessSquare>();
                // preserve existing pieces so that we can revert back
                IDictionary<ChessSquare, ChessPiece> originalSquares = new Dictionary<ChessSquare, ChessPiece>();

                // set each available square as this king so that we can check and see if
                // enemies would be able to capture it, if it moved there.
                available.ForEach(s =>
                {
                    originalSquares.Add(s, s.Piece);
                    s.Piece = king;
                });
                List<ChessSquare> enemyMoves = Game.Controller.BoardModel.GetEnemyMoves(king);

                available.ForEach(s =>
                {
                    if (!enemyMoves.Contains(s) && !safeKingMoves.Contains(s))
                    {
                        safeKingMoves.Add(s);
                    }
                });

                foreach (ChessSquare s in originalSquares.Keys)
                {
                    ChessPiece piece = null;
                    originalSquares.TryGetValue(s, out piece);
                    s.Piece = piece;
                }

                available = safeKingMoves;
            }
        }
    }
}
