using Chess.Models.Base;
using Chess.Models.Pieces;
using ChessGUI.Models.Utils;
using System.Collections.Generic;
using System;
using ChessGUI.Controllers;

namespace ChessGUI.Models.SpecialMoves
{
    /// <summary>
    /// This class handles Castling for kings.
    /// </summary>
    public class Castling : SpecialMove
    {
        // Rook movement arrays
        public static readonly char[] QUEEN_SIDE = { 'A', 'D' };
        public static readonly char[] KING_SIDE  = { 'H', 'F' };

        public int CastlingRank { get; private set; }
        public ChessSquare LeftSideCastle { get; private set; }
        public ChessSquare RightSideCastle { get; private set; }
        public KingChessPiece King { get; private set; }


        public Castling(KingChessPiece king)
        {
            King = king;
            Init();
        }

        /// <summary>
        /// Initializes Castling based on our KingChessPiece.
        /// </summary>
        private void Init()
        {
            CastlingRank = (King.Color == ChessColor.LIGHT) ? ChessBoard.MIN_RANK : ChessBoard.MAX_RANK;
        }

        /// <summary>
        /// Checks to see if the new location is equal to that of either the
        /// LeftSide or RightSide castling movements.
        /// </summary>
        /// <param name="newLocation"></param>
        public void CheckMovement(ChessSquare newLocation)
        {
            if ((LeftSideCastle == newLocation))
            {
                CastleMoveRook(QUEEN_SIDE);
            }
            else if ((RightSideCastle == newLocation))
            {
                CastleMoveRook(KING_SIDE);
            }
        }

        /// <summary>
        /// Checks to see if the king can castle.
        /// </summary>
        /// <param name="King"></param>
        /// <param name="available"></param>
        public void Check(ref List<ChessSquare> available)
        {
            if (MovementController.ActivePlayer.Color == King.Color)
            {
                // Clear out previous CastlingPositions
                Clear();

                // King cannot have moved and cannot castle out of check
                if (!King.InCheck && (King.MoveCount == 0))
                {
                    // Left side
                    List<ChessSquare> leftCastleSquares 
                        = GetCastleSquares(s => (s.Rank != this.King.Location.Rank) || (s.File > this.King.Location.File), false);

                    if (IsCastlingMovementValid(leftCastleSquares))
                    {
                        LeftSideCastle = GetCastleMove('A', leftCastleSquares, ref available);
                    }

                    // Right side
                    List<ChessSquare> rightCastleSquares
                        = GetCastleSquares(s => (s.Rank != this.King.Location.Rank) || (s.File < this.King.Location.File), true);

                    if (IsCastlingMovementValid(rightCastleSquares))
                    {
                        RightSideCastle = GetCastleMove('H', rightCastleSquares, ref available);
                    }
                }
            }
        }

        /// <summary>
        /// Gets the movement location to where this King will move
        /// if he castles. This checks to make sure that the rook exists
        /// and hasn't moved, because otherwise the king can't castle.
        /// </summary>
        /// <param name="rookFile"></param>
        /// <param name="castlingSquares"></param>
        /// <param name="available"></param>
        /// <returns></returns>
        private ChessSquare GetCastleMove(char rookFile, List<ChessSquare> castlingSquares, ref List<ChessSquare> available)
        {
            // Square that king will castle to
            ChessSquare castlingSquare = null;

            // Square that should contain the rook
            ChessSquare square = Game.Controller.BoardModel.SquareAt(rookFile + "" + CastlingRank);

            // Check to make sure that the square does indeed contain the rook
            if (square.IsOccupied() && square.Piece is RookChessPiece)
            {
                RookChessPiece rook = square.Piece as RookChessPiece;
                
                // rook cannot have moved at all
                if ((rook.MoveCount == 0))
                {
                    // King must move 2 squares to castle
                    if ((castlingSquares.Count == 2))
                    {
                        // add to the king's available moves for castling.
                        castlingSquare = castlingSquares[0];
                        available.Add(castlingSquare);
                    }
                }
            }
            return castlingSquare;
        }

        /// <summary>
        /// Checks to see if castling movement is valid, such that the distance we move
        /// is correct and that by moving, the king isn't threatened even just by passing.
        /// </summary>
        /// <param name="castlingSquares"></param>
        /// <returns>true if list of castling squares is valid</returns>
        private bool IsCastlingMovementValid(List<ChessSquare> castlingSquares)
        {
            bool isValidMovement = true;
            List<ChessSquare> enemyMoves = MovementController.Game.GetEnemyMoves();

            if ((castlingSquares.Count < 2))
            {
                isValidMovement = false;
            }
            else
            {
                // check to make sure enemy couldn't move to any square 
                foreach (ChessSquare s in castlingSquares)
                {
                    // king is not allowed to move if he would be threatened
                    if (enemyMoves.Contains(s))
                    {
                        isValidMovement = false;
                        break;
                    }
                }
            }
            return isValidMovement;
        }

        /// <summary>
        /// Gets the castling ChessSquares that the king must move over in
        /// order to castle.
        /// </summary>
        /// <param name="filter">Filter used to exclude invalid castle squares</param>
        /// <param name="reverse">Should the list be reversed. (This is helpful later)</param>
        /// <returns>List containing squares for castling</returns>
        private List<ChessSquare> GetCastleSquares(Predicate<ChessSquare> filter, bool reverse)
        {
            List<ChessSquare> castleSquares = BoardScanner.Scan(King, 2);
            castleSquares.RemoveAll(filter);
            castleSquares.Sort();
            if (reverse)
            {
                castleSquares.Reverse();
            }
            return castleSquares;
        }

        /// <summary>
        /// Clears out old movement values for the king.
        /// </summary>
        private void Clear()
        {
            LeftSideCastle = null;
            RightSideCastle = null;
        }

        /// <summary>
        /// Moves the rook to the correct location when the king castles.
        /// </summary>
        /// <param name="moveArr">KING_SIDE or QUEEN_SIDE array containing start and end files</param>
        private void CastleMoveRook(char[] moveArr)
        {
            ChessBoard board = Game.Controller.BoardModel;
            ChessPiece rook  = board.SquareAt(GetSquare(0, moveArr)).Piece;
            ChessSquare endSquare = board.SquareAt(GetSquare(1, moveArr));
            MovementController.Move(rook, endSquare);
        }

        /// <summary>
        /// Convenience method for getting the correct string from the moveArr
        /// and specified index.
        /// </summary>
        /// <param name="index">Index in moveArr</param>
        /// <param name="moveArr">MoveArr</param>
        /// <returns>string contianing file and castling rank</returns>
        private string GetSquare(int index, char[] moveArr)
        {
            string squareStr = moveArr[index] + "" + CastlingRank;
            return squareStr;
        }

        // Unused
        public override void Check() { }
    }
}
