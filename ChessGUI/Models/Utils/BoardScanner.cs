using Chess.Models.Base;
using System.Collections.Generic;
using System.Linq;

namespace ChessGUI.Models.Utils
{
    /// <summary>
    /// This utility class allows for "scanning" along the various ChessSquares of a ChessBoard.
    /// This class is used for checking what ChessSquares are available for a given ChessPiece
    /// and whether or not we can capture. Scanning along the board requires an initial starting
    /// ChessPiece and it's supported MoveDirections. From there, we can continue to scan along
    /// the board in each supported ChessPiece MoveDirection until we are either blocked, or we
    /// are out of range on the ChessBoard.
    /// 
    /// A limit can be set to determine how many iterations (scans) we make in each direction.
    /// Some ChessPieces' range of movement is limited, and thus they need specify their limit when
    /// creating a BoardScanner. The default is NO_LIMIT, where scanning will continue until the
    /// above conditions are met.
    /// 
    /// After a scan has been completed in the current MoveDirection, everything is reset back
    /// to the Pivot ChessPiece, and then scanning can continue, if there are any more
    /// MoveDirections.
    /// </summary>
    public static class BoardScanner
    {
        // Default limit where scanning will continue until we run off the board
        private const int NO_LIMIT = 64;

        // Board instance to check moves against
        public static ChessBoard Board { get; set; }

        // Piece containing starting location that all scans will start from
        public static ChessSquare Pivot { get; private set; }

        public static ChessPiece Piece { get; private set; }

        // Scan distance threshold for each Piece direction
        public static int Limit { get; set; }

        // Current direction we are scanning
        public static Move CurrentDirection { get; private set; }

        public static char NextFile { get; private set; }

        public static int NextRank { get; private set; }


        /// <summary>
        /// Scans the board in each supported movement direction of the ChessPiece until we
        /// have hit our scan limit.
        /// </summary>
        /// <param name="piece">Piece to begin scanning from</param>
        /// <param name="limit">Scan threshold to terminate scanning</param>
        /// <returns>List of all ChessSquares from all supported ChessPiece moves</returns>
        public static List<ChessSquare> Scan(ChessPiece piece, int limit = NO_LIMIT)
        {
            Init(piece, limit);
            List<ChessSquare> available = new List<ChessSquare>();
            ChessSquare nextSquare = null;

            foreach (Move d in Piece.MoveDirections)
            {
                CurrentDirection = d;
                int j = 0;

                while (HasNext() && j < Limit)
                {
                    j++;
                    nextSquare = NextSquare();

                    if (nextSquare.IsOccupied())
                    {
                        if (Piece.IsOpponent(nextSquare.Piece))
                        {
                            available.Add(nextSquare);
                            Piece.AvailableCaptures.Add(nextSquare.Piece);
                        }
                        break;
                    }
                    available.Add(nextSquare);
                }
                ResetStart();
            }
            return available;
        }

        /// <summary>
        /// Scans in each supported ChessPiece direction and then "branches out" to get the diagonally
        /// positioned ChessSquares, relative to where we stopped scanning.
        /// </summary>
        /// <param name="piece">Piece to begin scanning from</param>
        /// <param name="limit">Scan threshold to terminate scanning</param>
        /// <returns>List of all ChessSquares from all supported ChessPiece moves</returns>
        public static List<ChessSquare> ScanBranched(ChessPiece piece, int limit = 1)
        {
            Init(piece, limit);
            List<ChessSquare> available = new List<ChessSquare>();
            ChessSquare nextSquare = null;

            foreach (Move d in Piece.MoveDirections)
            {
                CurrentDirection = d;
                int j = 0;

                while (HasNext() && j < Limit)
                {
                    j++;
                    nextSquare = NextSquare();
                }
                // Get diagonals from the square we just moved to
                if (nextSquare != null)
                {
                    available.AddRange(GetDiagonals(nextSquare, d).Where(s => s != null && Piece.CanOccupy(s)));
                }
                ResetStart();
            }
            return available;
        }

        /// <summary>
        /// Gets all the diagonals from the specified ChessSquare and direction.
        /// </summary>
        /// <param name="square">Starting ChessSquare</param>
        /// <param name="dir">Direction to get diagonals relative to</param>
        /// <returns>List containing diagonals relative to the square</returns>
        public static List<ChessSquare> GetDiagonals(ChessSquare square, Move dir)
        {
            List<ChessSquare> diagonals = new List<ChessSquare>();
            Move[] moves = GetDiagonalMoves(dir);

            foreach(Move m in moves)
            {
                ChessSquare diagonal = Board.SquareAt((char)(square.File + m.FileMoveValue), square.Rank + m.RankMoveValue);
                if (diagonal != null)
                {
                    diagonals.Add(diagonal);

                    // TODO maybe move this elsewhere
                    if (diagonal.IsOccupied())
                    {
                        Piece.AvailableCaptures.Add(diagonal.Piece);
                    }
                }
            }
            return diagonals;
        }

        /// <summary>
        /// Returns an array of diagonal movements, relative to the specified direction.
        /// </summary>
        /// <param name="dir">Direction to get diagonals from</param>
        /// <returns>Array of moves that are diagonal from the specified direction</returns>
        private static Move[] GetDiagonalMoves(Move dir)
        {
            Move[] moves = null;

            switch (dir.Name)
            {
                case "North":
                    moves = new Move[] { Moves.northEast, Moves.northWest };
                    break;
                case "South":
                    moves = new Move[] { Moves.southEast, Moves.southWest };
                    break;
                case "East":
                    moves = new Move[] { Moves.northEast, Moves.southEast };
                    break;
                case "West":
                    moves = new Move[] { Moves.northWest, Moves.southWest };
                    break;
            }
            return moves;
        }

        /// <summary>
        /// Initializes this scanner with initial positioning and scan limits.
        /// </summary>
        /// <param name="piece">ChessPiece to begin all scans from</param>
        /// <param name="limit">Scan threshold limit</param>
        private static void Init(ChessPiece piece, int limit)
        {
            Piece = piece;
            Pivot = piece.Location;
            Limit = limit;
            Piece.AvailableCaptures.Clear();
            ResetStart();
        }

        /// <summary>
        /// Checks to see if the ChessSquare we're trying to access is contained within
        /// the board.
        /// </summary>
        /// <returns>true if ChessSquare exists in the Board</returns>
        private static bool HasNext()
        {
            Update();
            bool hasNext = Board.SquareAt(NextFile, NextRank) != null;
            return hasNext;
        }

        /// <summary>
        /// Gets the next ChessSquare from the board.
        /// </summary>
        /// <returns>The next ChessSquare from the board.</returns>
        private static ChessSquare NextSquare()
        {
            ChessSquare next = Board.SquareAt(NextFile, NextRank);
            return next;
        }

        /// <summary>
        /// Updates the NextFile / NextRank values based on the current direction.
        /// </summary>
        private static void Update()
        {
            NextFile += (char)CurrentDirection.FileMoveValue;
            NextRank += CurrentDirection.RankMoveValue;
        }

        /// <summary>
        /// Resets the next scan location back to the initial starting pivot location.
        /// </summary>
        private static void ResetStart()
        {
            NextFile = Pivot.File;
            NextRank = Pivot.Rank;
        }
    }
}
