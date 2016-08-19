using Chess.Models.Base;
using Chess.Models.Pieces;
using ChessGUI.Views;
using System.Collections.Generic;
using System.Windows;
using System;

namespace ChessGUI.Controllers
{
    /// <summary>
    /// Helper class for handling ChessPiece movement from Square_A to Square_B.
    /// </summary>
    public class MovementController
    {
        // Starting ChessSquare
        private static ChessSquare _startSquare;
        public static ChessSquareView Start { get; private set; }

        // Ending ChessSquare
        private static ChessSquare _endSquare;
        public static ChessSquareView End { get; private set; }

        // The piece we're moving
        public static ChessPiece MovePiece { get; set; }

        // Player whose turn it is.
        public static ChessPlayer ActivePlayer { get; set; }

        public static ChessGame Game { get; set; }

        /// <summary>
        /// Updates Start and End positions and ensures that the starting position
        /// has a ChessPiece to be able to move.
        /// </summary>
        /// <param name="squareView">ChessSquareView we clicked on</param>
        public static void Move(ChessSquareView squareView)
        {
            if (Start == null)
            {
                SetStart(squareView);
            } else
            {
                SetEnd(squareView);
                if (CheckMove())
                {
                    MovePiece.MoveTo(_endSquare);
                    ActivePlayer.DidMove = true;
                    ClearMovement();
                } else
                {
                    MessageBox.Show("Invalid " + MovePiece + " move!");
                }
            }
        }

        /// <summary>
        /// Clears out the movement and updates the ending location with the proper
        /// ChessPiece image that we just moved.
        /// </summary>
        private static void ClearMovement()
        {
            Start.PieceView.Clear();
            End.PieceView.SetImageFromPiece(MovePiece);
            Clear();
        }

        /// <summary>
        /// Checks to make sure that the ChessSquare that we're moving too
        /// is valid.
        /// </summary>
        /// <returns></returns>
        private static bool CheckMove()
        {
            bool canMove = false;
            // Check to make sure we have a ChessPiece to move and a destination
            if (IsStartAndEndSet())
            {
                List<ChessSquare> validMoves = 
                    GetValidMoves(MovePiece, ActivePlayer.KingPiece, Game.GetOpponent());

                if (validMoves.Contains(_endSquare))
                {
                    canMove = true;
                }
            }
            return canMove;
        }

        /// <summary>
        /// Gets a list of all valid moves for the specified ChessPiece where
        /// by moving, they do NOT allow the ActivePlayer's king to be
        /// put in check.
        /// </summary>
        /// <param name="movePiece">ChessPiece to check moves against</param>
        /// <param name="king">KingChessPiece we're trying to keep safe</param>
        /// <returns>List containing all valid moves for ChessPiece</returns>
        public static List<ChessSquare> GetValidMoves(ChessPiece movePiece, KingChessPiece king, ChessPlayer enemy)
        {
            List<ChessSquare> validMoves = new List<ChessSquare>();
            List<ChessSquare> available = movePiece.GetAvailableMoves();

            SetClear(movePiece, king, true);

            // For every move, check to see if it puts the ActivePlayer's King
            // in check. If it does, it is not a valid move!
            available.ForEach(s =>
            {
                // Possible pre-existing ChessPiece that we would capture, if we move here
                // Ignore this piece, so it is not included in the enemy moves
                ChessPiece original = s.Piece;
                SetIgnore(original, true);
                s.Piece = movePiece;

                List<ChessSquare> enemyMoves = Game.GetPlayerMoves(enemy);

                // Location that we don't want our enemy moves to contain as it
                // means the King is vulnerable
                ChessSquare checkSquare = (movePiece != king) ? king.Location : s;

                // Square is safe to move to
                if (!enemyMoves.Contains(checkSquare))
                {
                    validMoves.Add(s);
                }
                SetIgnore(original, false);
                s.Piece = original;
            });
            SetClear(movePiece, king, false);
            return validMoves;
        }

        private static void SetClear(ChessPiece movePiece, KingChessPiece king, bool clearFlag)
        {
            if (movePiece != king)
            {
                if (clearFlag)
                {
                    movePiece.Location.ClearPiece();
                } else
                {
                    movePiece.Location.Piece = movePiece;
                }
            }
        }

        private static void SetIgnore(ChessPiece pieceToIgnore, bool ignoreFlag)
        {
            if (pieceToIgnore != null)
            {
                pieceToIgnore.Ignore = ignoreFlag;
            }
        }
        
        /// <summary>
        /// Extracts the model from the Start and End ChessSquare views, and
        /// initializes the piece we're moving from the start square.
        /// </summary>
        private static void UpdateSquares()
        {
            _startSquare = Start.DataContext as ChessSquare;
            _endSquare = End.DataContext as ChessSquare;
            MovePiece = _startSquare.Piece;
        }

        /// <summary>
        /// Checks to see if both Start and End locations are set.
        /// </summary>
        /// <returns>true if both Start and End locations are set</returns>
        private static bool IsStartAndEndSet()
        {
            bool result = (Start != null) && (End != null);
            return result;
        }

        /// <summary>
        /// Sets the starting position and checks to make sure that it contains
        /// a ChessPiece that we can actually move.
        /// </summary>
        /// <param name="squareView">ChessSquareView starting position</param>
        private static void SetStart(ChessSquareView squareView)
        {
            // We have a piece that we can move
            ChessSquare square = squareView.DataContext as ChessSquare;

            if (square.IsOccupied())
            {
                MovePiece = square.Piece;

                if (MovePiece.Color != ActivePlayer.Color)
                {
                    MessageBox.Show(string.Format("It's {0}'s turn! You must move only your own pieces!", ActivePlayer.Color));
                } else
                {
                    Start = squareView;
                    Start.ToggleHighlight();
                }
            }
        }

        /// <summary>
        /// Sets the ending position and checks to make sure that the End is not the
        /// same position as the start.
        /// </summary>
        /// <param name="squareView"></param>
        private static void SetEnd(ChessSquareView squareView)
        {
            End = squareView;

            // Start and End are not the same
            if (Start != End)
            {
                UpdateSquares();
            }
            else
            {
                End = null;
            }
        }

        /// <summary>
        /// Clears out start and ending positions.
        /// </summary>
        public static void Clear()
        {
            if (Start != null)
            {
                Start.ToggleHighlight();
            }
            Game.Controller.ClearPreviews();
            Start = null;
            End = null;
        }
    }
}
