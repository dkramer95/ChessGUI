using Chess.Models.Base;
using System.Collections.Generic;
using System.Linq;

namespace Chess.Models.Utils
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
    public class BoardScanner
    {
        // The ChessPiece we will begin scanning from
        private ChessPiece _piece;
        private ChessBoard _board;

        // Next ChessPiece positioning
        private char _nextFile;
        private int _nextRank;

        // Used as default limit, which skips range limit checking
        public const int NO_LIMIT = 64;

        // The direction we are currently scanning through the board
        private MoveDirection _currentDirection;
        public MoveDirection CurrentDirection
        {
            get
            {
                return _currentDirection;
            }
            set
            {
                _currentDirection = value;
                // reset back to pivot after changing our current direction
                Reset();
            }
        }

        // Pivot location from which all scans will start from
        public ChessSquare Pivot { get; set; }

        // Scan threshold limit (for specific pieces) where the range is limited
        public int Limit { get; set; }


        /// <summary>
        /// Constructs a new BoardScanner starting about the specified ChessPiece
        /// </summary>
        /// <param name="piece">ChessPiece to begin scans from</param>
        /// <param name="limit">Optional range scan limit (default is none)</param>
        public BoardScanner(ChessPiece piece, int limit = NO_LIMIT)
        {
            _piece = piece;
            _board = ChessPiece.Board;  //TODO:: Try to remove static CHessBoard from player!
            Pivot  = _piece.Location;
            Limit = limit;
        }

        /// <summary>
        /// Scans the board in each supported direction from the current
        /// ChessPiece while ChessSquares are open, or until we hit an opponent.
        /// </summary>
        /// <returns>List of available ChessSquares to move to</returns>
        public List<ChessSquare> Scan()
        {
            List<ChessSquare> available = new List<ChessSquare>();
            _piece.AvailableCaptures.Clear();
            ChessSquare square = null;

            foreach (MoveDirection dir in _piece.MoveDirections)
            {
                CurrentDirection = dir;
                int count = 0;

                while (HasNext() && count < Limit)
                {
                    count++;
                    square = Next();

                    // add if opponent then break, otherwise just break because we can't 
                    // land on same ChessPiece color
                    if (square.IsOccupied())
                    {
                        if (_piece.IsOpponent(square.Piece))
                        {
                            available.Add(square);
                            _piece.AvailableCaptures.Add(square.Piece);
                        }
                        break;
                    }
                    available.Add(square);
                }
            }
            return available;
        }

        /// <summary>
        /// Scans the board in each supported direction from the current
        /// ChessPiece and branches out to also scan diagonal ChessSquares.
        /// NOTE: This should only be used on the KnightChessPiece as all
        /// other ChessPieces should use the regular Scan method.
        /// </summary>
        /// <returns></returns>
        public List<ChessSquare> ScanBranched()
        {
            List<ChessSquare> available = new List<ChessSquare>();
            _piece.AvailableCaptures.Clear();

            ChessSquare square = null;
            const int MOVE_LIMIT = 1;

            foreach (MoveDirection dir in _piece.MoveDirections)
            {
                CurrentDirection = dir;
                int count = 0;

                // Advance 1 square in each direction
                while (HasNext() && count < MOVE_LIMIT)
                {
                    square = Next();
                    count++;

                }
                // Get diagonals from the square we just moved to
                if (square != null)
                {
                    available.AddRange(DiagonalsFrom(square, dir).Where(s => s != null && _piece.CanOccupy(s)));
                }
            }
            return available;
        }

        /// <summary>
        /// Gets the diagonal ChessSquares from the starting ChessSquare and the specified
        /// move direction.
        /// </summary>
        /// <param name="square"></param>
        /// <param name="direction"></param>
        /// <returns></returns>
        public List<ChessSquare> DiagonalsFrom(ChessSquare startSquare, MoveDirection direction)
        {
            List<ChessSquare> diagonals = new List<ChessSquare>();

            // rank/file scan direction movement
            int moveValue = GetMoveValue(direction);
            int[] moveArr = null;

            // initialize moveArr[] values to get proper diagonals based on direction
            switch (direction)
            {
                // diagonals left and right
                case MoveDirection.NORTH:
                case MoveDirection.SOUTH:
                    moveArr = new int[] { 1, moveValue, -1, moveValue };
                    break;
                // diagonals above and below
                case MoveDirection.EAST:
                case MoveDirection.WEST:
                    moveArr = new int[] { moveValue, 1, moveValue, -1 };
                    break;
            }
            AddDiagonals(startSquare, diagonals, moveArr);
            return diagonals;
        }

        /// <summary>
        /// Updates list of diagonals by starting at the ChessSquare and using the moveArr
        /// values to determine the File / Rank DX to get the proper diagonal value. The
        /// new diagonal ChessSquare is added to the diagonals List.
        /// </summary>
        /// <param name="startSquare">Starting ChessSquare from which to get diagonals from</param>
        /// <param name="diagonals">List to append diagonal ChessSquares to</param>
        /// <param name="moveArr">Movement array specifying where / how to look for diagonals</param>
        private void AddDiagonals(ChessSquare startSquare, List<ChessSquare> diagonals, int[] moveArr)
        {
            for (int j = 0; j < moveArr.Length; j += 2)
            {
                ChessSquare diagonal = 
                    _board.SquareAt((char)(startSquare.File + moveArr[j]), startSquare.Rank + moveArr[j + 1]);

                // add if diagonals exists
                if (diagonal != null)
                {
                    diagonals.Add(diagonal);

                    if (diagonal.Piece != null)
                    {
                        _piece.AvailableCaptures.Add(diagonal.Piece);
                    }
                }
            }
        }

        /// <summary>
        /// Gets the direction advancement value from the specified MoveDirection.
        /// </summary>
        /// <param name="d">MoveDirection to check</param>
        /// <returns>int value of MoveDirection</returns>
        public static int GetMoveValue(MoveDirection d)
        {
            int value = 0;
            switch (d)
            {
                case MoveDirection.NORTH:
                case MoveDirection.EAST:
                    value = 1;
                    break;
                case MoveDirection.SOUTH:
                case MoveDirection.WEST:
                    value = -1;
                    break;
            }
            return value;
        }

        /// <summary>
        /// Resets _nextFile and _nextRank back to the starting Pivot values.
        /// </summary>
        private void Reset()
        {
            _nextFile = Pivot.File;
            _nextRank = Pivot.Rank;
        }

        /// <summary>
        /// Gets the next ChessSquare in the current scan direction context.
        /// </summary>
        /// <returns>next ChessSquare</returns>
        public ChessSquare Next()
        {
            Update(ref _nextFile, ref _nextRank);
            return _board.SquareAt(_nextFile, _nextRank);
        }

        /// <summary>
        /// Updates the nextFile and nextRank based on the current scan direction context.
        /// </summary>
        /// <param name="nextFile">nextFile to update (depending on direction)</param>
        /// <param name="nextRank">nextRank to update (depending on direction)</param>
        private void Update(ref char nextFile, ref int nextRank)
        {
            switch (CurrentDirection)
            {
                case MoveDirection.NORTH:
                    nextRank++;
                    break;
                case MoveDirection.SOUTH:
                    nextRank--;
                    break;
                case MoveDirection.EAST:
                    nextFile++;
                    break;
                case MoveDirection.WEST:
                    nextFile--;
                    break;
                case MoveDirection.NORTH_EAST:
                    nextFile++;
                    nextRank++;
                    break;
                case MoveDirection.NORTH_WEST:
                    nextFile--;
                    nextRank++;
                    break;
                case MoveDirection.SOUTH_EAST:
                    nextRank--;
                    nextFile++;
                    break;
                case MoveDirection.SOUTH_WEST:
                    nextRank--;
                    nextFile--;
                    break;
            }
        }

        /// <summary>
        /// Checks to see if there exists another ChessSquare in the current
        /// scan direction context.
        /// </summary>
        /// <returns>true if the next ChessSquare is not null</returns>
        public bool HasNext()
        {
            char nextFile = _nextFile;
            int nextRank = _nextRank;
            Update(ref nextFile, ref nextRank);
            return _board.SquareAt(nextFile, nextRank) != null;
        }
    }
}
