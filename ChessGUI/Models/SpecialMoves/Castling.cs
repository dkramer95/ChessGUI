using Chess.Models.Base;
using Chess.Models.Pieces;
using ChessGUI.Models.Utils;
using System.Collections.Generic;

namespace ChessGUI.Models.SpecialMoves
{
    public class Castling : SpecialMove
    {
        private const int KING_CASTLE_MOVE = 2;

        public static RookChessPiece KingSideRook { get; private set; }
        public static RookChessPiece QueenSideRook { get; private set; }

        public static ChessSquare KingSideCastle { get; private set; }
        public static ChessSquare QueenSideCastle { get; private set; }

        public override void Check()
        {
        }

        public static void CheckCastling(KingChessPiece king, ref List<ChessSquare> available)
        {
            if (!king.InCheck && (king.MoveCount == 0))
            {
                // King moves 2 squares left or right and the rook is moved to
                // stand on the opposite side of the king.
                IDictionary<int, List<ChessSquare>> squares = GetCastleSquares(king);

                foreach (int moveVal in squares.Keys)
                {
                    List<ChessSquare> squaresList = squares[moveVal];

                    // movement for the king
                    if ((squaresList.Count == KING_CASTLE_MOVE))
                    {
                        // check to make sure that if we move by castling, king would not
                        // be threatened.
                        if (IsCastlingMovementSafe(squaresList))
                        {
                            // square that king would move to
                            ChessSquare castleSquare = squaresList[0];

                            // get the rooks and add castleSquare to moves if castling is valid, finally!
                            if ((moveVal > 0))
                            {
                                // right
                                TryAddRook(king, 'H', castleSquare, ref available);
                            }
                            else
                            {
                                // left
                                TryAddRook(king, 'A', castleSquare, ref available);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Tries to add a rook if it would be valid.
        /// </summary>
        /// <param name="king">KingChessPiece</param>
        /// <param name="file">File location of Rook</param>
        /// <param name="castleSquare">Castle Square that king would move to</param>
        /// <param name="available">List of available moves</param>
        private static void TryAddRook(KingChessPiece king, char file, ChessSquare castleSquare, ref List<ChessSquare> available)
        {
            ChessSquare square = Game.Controller.BoardModel.SquareAt(file, king.Location.Rank);

            // check to see if rook exists and hasn't moved to be able to castle
            if (square.IsOccupied() && square.Piece is RookChessPiece)
            {
                RookChessPiece rookPiece = square.Piece as RookChessPiece;
                if ((rookPiece.MoveCount == 0))
                {
                    available.Add(castleSquare);
                    UpdateRook(file, rookPiece, castleSquare);
                }
            }
        }

        private static void UpdateRook(char file, RookChessPiece rook, ChessSquare square)
        {
            if (file == 'H')
            {
                KingSideRook = rook;
                KingSideCastle = square;
            }
            else
            {
                QueenSideRook = rook;
                QueenSideCastle = square;
            }
        }

        private static IDictionary<int, List<ChessSquare>> GetCastleSquares(KingChessPiece king)
        {
            IDictionary<int, List<ChessSquare>> squares = null;

            // left side -> remove squares that aren't to the left
            List<ChessSquare> leftSquares = BoardScanner.Scan(king, 2);
            leftSquares.RemoveAll(s => (s.Rank != king.Location.Rank) || (s.File > king.Location.File));
            leftSquares.Sort();

            // right side -> remove squares that aren't to the right
            List<ChessSquare> rightSquares = BoardScanner.Scan(king, 2);
            rightSquares.RemoveAll(s => (s.Rank != king.Location.Rank) || (s.File < king.Location.File));
            rightSquares.Sort();
            rightSquares.Reverse();

            // use key as movement direction. Sneaky hack :)
            squares = new Dictionary<int, List<ChessSquare>>() { { -1, leftSquares }, { 1, rightSquares } };

            return squares;
        }

        /// <summary>
        /// Checks to see if movement required for Castling is safe.
        /// </summary>
        /// <param name="castleSquares">List of squares king would move to castle that
        /// need to be checked to ensure that an enemy couldn't occupy and threaten
        /// the king.</param>
        /// <returns></returns>
        private static bool IsCastlingMovementSafe(List<ChessSquare> castleSquares)
        {
            List<ChessSquare> enemyMoves = Game.GetPlayerMoves(Game.GetOpponent());

            bool isMovementSafe = true;

            castleSquares.ForEach(s =>
            {
                if (enemyMoves.Contains(s))
                {
                    isMovementSafe = false;
                }
            });
            return isMovementSafe;
        }
    }
}
