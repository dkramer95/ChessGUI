using Chess.Models.Base;
using Chess.Models.Pieces;
using ChessGUI.Models.Utils;
using System.Collections.Generic;
using System;
using ChessGUI.Controllers;

namespace ChessGUI.Models.SpecialMoves
{
    /// <summary>
    /// This class handles Castling movement for a KingChessPiece. When a king castles,
    /// a RookChessPiece is also moved. This class handles movement for the Rook as well.
    /// </summary>
    public class Castling : SpecialMove
    {
        // Rook file locations
        public const char QUEEN_SIDE_ROOK = 'A';
        public const char KING_SIDE_ROOK = 'H';

        // Rook movement arrays
        public static readonly char[] QUEEN_SIDE_ROOK_MOVE = { 'A', 'D' };
        public static readonly char[] KING_SIDE_ROOK_MOVE  = { 'H', 'F' };

        // Movement limit required by the king for castling
        public const int KING_MOVE_LIMIT = 2;

        public int CastlingRank { get; private set; }
        public ChessSquare QueenSideCastle { get; private set; }
        public ChessSquare KingSideCastle { get; private set; }
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
        /// <param name="newLocation">NewLocation to check and see if we can Castle</param>
        public void CheckMovement(ChessSquare newLocation)
        {
            if ((QueenSideCastle == newLocation))
            {
                CastleMoveRook(QUEEN_SIDE_ROOK_MOVE);
            }
            else if ((KingSideCastle == newLocation))
            {
                CastleMoveRook(KING_SIDE_ROOK_MOVE);
            }
        }

        /// <summary>
        /// Checks to see if the king can castle.
        /// </summary>
        /// <param name="available">List of available moves of the King</param>
        public void Check(ref List<ChessSquare> available)
        {
            if (MoveController.ActivePlayer.Color == King.Color)
            {
                // Clear out previous CastlingPositions
                Clear();

                // King cannot have moved and cannot castle out of check
                if (!King.InCheck && !King.HasMoved())
                {
                    // Try to get left and right side castling, if it is valid to do so
                    QueenSideCastle  = GetCastleMove(QUEEN_SIDE_ROOK, ref available);
                    KingSideCastle   = GetCastleMove(KING_SIDE_ROOK,  ref available);
                }
            }
        }

        /// <summary>
        /// Gets the castle square that a king will move to if he castles.
        /// </summary>
        /// <param name="rookFile">File of the rook</param>
        /// <param name="filter">Filter to apply for castling to remove invalid squares</param>
        /// <param name="available">list of available king moves</param>
        /// <returns>ChessSquare that king can castle to if valid, otherwise null</returns>
        private ChessSquare GetCastleMove(char rookFile, ref List<ChessSquare> available)
        {
            ChessSquare castleSquare = null;
            List<ChessSquare> castleSquares = GetCastleSquares();

            // Filter out bad castle squares
            Filter(ref castleSquares, rookFile);

            // Check to make sure that king isn't threatened
            if (IsCastlingMovementValid(castleSquares))
            {
                castleSquare = GetCastleMove(rookFile, castleSquares, ref available);
            }
            return castleSquare;
        }

        /// <summary>
        /// Gets the movement location to where this King will move
        /// if he castles. This checks to make sure that the rook exists
        /// and hasn't moved, because otherwise the king can't castle.
        /// </summary>
        /// <param name="rookFile">Starting File of the rook</param>
        /// <param name="castlingSquares">Squares containing castling movement</param>
        /// <param name="available">List of available moves to append to, if castling is valid</param>
        /// <returns></returns>
        private ChessSquare GetCastleMove(char rookFile, List<ChessSquare> castlingSquares,
                                          ref List<ChessSquare> available)
        {
            // Square that king will castle to
            ChessSquare castlingSquare = null;

            // Rook that will also move if King castles
            RookChessPiece rook = null;

            // Square that should contain the rook
            ChessSquare square = Game.Controller.BoardModel.SquareAt(rookFile, CastlingRank);
            
            // Check to make sure that the square does indeed contain the rook
            if (TryGetRook(square, out rook))
            {                
                // rook cannot have moved at all
                if (!rook.HasMoved())
                {
                    // King must move 2 squares to castle
                    if ((castlingSquares.Count == KING_MOVE_LIMIT))
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
        /// Tries to get the rook from the specified ChessSquare.
        /// </summary>
        /// <param name="square">ChessSquare that should contain rook</param>
        /// <param name="rook">RookChessPiece to update, if valid</param>
        /// <returns>True if square contains RookChessPiece</returns>
        private bool TryGetRook(ChessSquare square, out RookChessPiece rook)
        {
            bool isRook = square.IsOccupied() && (square.Piece is RookChessPiece);
            rook = isRook ? square.Piece as RookChessPiece : null;
            return isRook;
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
            List<ChessSquare> enemyMoves = MoveController.Game.GetEnemyMoves();

            // king must move 2 squares in order to castle
            if ((castlingSquares.Count != KING_MOVE_LIMIT))
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
        /// <returns>List containing squares for castling</returns>
        private List<ChessSquare> GetCastleSquares()
        {
            List<ChessSquare> castleSquares = BoardScanner.Scan(King, KING_MOVE_LIMIT);
            castleSquares.Sort();
            return castleSquares;
        }

        /// <summary>
        /// Filters out invalid squares in castleSquares.
        /// </summary>
        /// <param name="castleSquares">List of squares to filter</param>
        /// <param name="rookFile">The file of the rook</param>
        private void Filter(ref List<ChessSquare> castleSquares, char rookFile)
        {
            // filters for invalid castling squares
            Predicate<ChessSquare>[] filters = GetFilters(rookFile);

            // filter out invalid castle squares
            foreach (var f in filters) { castleSquares.RemoveAll(f); }
            CheckReverse(ref castleSquares, rookFile);
        }

        /// <summary>
        /// Gets an array of filters, needed for removing invalid ChessSquares
        /// from castling movement.
        /// </summary>
        /// <param name="rookRank">The rank of the RookChessPiece</param>
        /// <returns>Array of filters for the specified rookRank</returns>
        private Predicate<ChessSquare>[] GetFilters(char rookRank)
        {
            Predicate<ChessSquare> rankFilter = (s => s.Rank != King.Location.Rank);
            Predicate<ChessSquare> fileFilter = null;

            if      (rookRank == QUEEN_SIDE_ROOK) { fileFilter = (s => s.File > King.Location.File); }
            else if (rookRank == KING_SIDE_ROOK)  { fileFilter = (s => s.File < King.Location.File); }

            Predicate<ChessSquare>[] filters = { rankFilter, fileFilter };
            return filters;
        }

        /// <summary>
        /// Checks to see if the list of CastleSquares should be reversed.
        /// This is useful for retrieving the proper square at index 0.
        /// </summary>
        /// <param name="castleSquares">List of castling squares</param>
        /// <param name="rookFile">File of the rook</param>
        private void CheckReverse(ref List<ChessSquare> castleSquares, char rookFile)
        {
            bool reverse = (rookFile == KING_SIDE_ROOK);
            if (reverse) { castleSquares.Reverse(); }
        }

        /// <summary>
        /// Clears out old movement values for the king.
        /// </summary>
        private void Clear()
        {
            QueenSideCastle = null;
            KingSideCastle = null;
        }

        /// <summary>
        /// Moves the rook to the correct location when the king castles.
        /// </summary>
        /// <param name="moveArr">KING_SIDE or QUEEN_SIDE array containing start and end files</param>
        private void CastleMoveRook(char[] moveArr)
        {
            ChessBoard board = Game.Controller.BoardModel;
            ChessPiece rook = board.SquareAt(moveArr[0], CastlingRank).Piece;
            ChessSquare endSquare = board.SquareAt(moveArr[1], CastlingRank);
            MoveController.Move(rook, endSquare);
        }

        // Unused
        public override void Check() { throw new NotImplementedException();  }
    }
}
