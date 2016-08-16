using Chess.Models.Base;
using Chess.Models.Pieces;
using ChessGUI.Models.SpecialMoves;
using ChessGUI.Views;
using System.Collections.Generic;
using System.Windows;

namespace ChessGUI.Controllers
{
    /// <summary>
    /// Helper class for handling ChessPiece movement from Square_A to Square_B.
    /// </summary>
    public class ChessMovement
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

        public static NewChessGame Game { get; set; }

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
            }
            else
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
        /// Checks to see if we can move and updates ChessSquare and piece models.
        /// </summary>
        /// <returns></returns>
        private static bool CheckMove()
        {
            bool canMove = false;

            if (IsStartAndEndSet())
            {
                // Is EndSquare a valid place to move to?
                List<ChessSquare> available = MovePiece.GetAvailableMoves();
                KingInCheck.TestMoves(MovePiece, ref available);

                if (available.Contains(_endSquare))
                {
                    canMove = true;
                }
            }
            return canMove;
        }

        private static bool IsKingInCheck()
        {
            KingChessPiece king = ActivePlayer.KingPiece;
            List<ChessSquare> opponentMoves = new List<ChessSquare>();

            // Populate with all moves that an opponent can make against player
            ChessPlayer opponent = Game.GetOpponent(ActivePlayer);
            opponent.Pieces.ForEach(p => opponentMoves.AddRange(p.GetAvailableMoves()));

            // Check to see if any opponent moves would put king in check
            opponentMoves = opponentMoves.FindAll(s => (s == king.Location));
            king.InCheck = (opponentMoves.Count > 0);

            return king.InCheck;
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
            Start = null;
            End = null;
        }
    }
}
