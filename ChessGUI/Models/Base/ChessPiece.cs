using Chess.Models.Utils;
using System;
using System.Collections.Generic;

namespace Chess.Models.Base
{
    /// <summary>
    /// This is the base class for all chess pieces. This class provides enforcement of basic
    /// functionality that all ChessPieces will share through abstraction, but allows sub-classes
    /// to define the individual behavior logic for each type of ChessPiece.
    /// All ChessPieces have a particular way of moving around on a ChessBoard and can also
    /// capture other ChessPieces based on the current context. 
    /// </summary>
    public abstract class ChessPiece
    {
        // Location of this chess piece on the board
        public ChessSquare Location { get; protected set; }

        // Light or Dark Color of this ChessPiece
        public ChessColor Color { get; set; }

        // Symbol representation of this ChessPiece
        public abstract char Symbol { get; }

        // Value of this ChessPiece (each piece has separate constant value)
        public abstract int Value { get; }

        // Amount of moves this ChessPiece has made
        public int MoveCount { get; protected set; }

        // Has this ChessPiece been captured by another ChessPiece?
        public bool IsCaptured { get; protected set; }

        // The ChessBoard that all ChessPieces belong to to access other tiles / pieces
        protected static ChessBoard _board;

        public static ChessBoard Board { get { return _board; } }

        // Have we initialized ChessPiece so that it can talk to the ChessBoard for moving around?
        private static bool _isInitialized = false;

        // Other ChessPieces that this ChessPiece can capture
        public List<ChessPiece> AvailableCaptures { get; private set; }

        public abstract MoveDirection[] MoveDirections { get; }

        /// <summary>
        /// Constructs a new chess piece at the specified location.
        /// </summary>
        /// <param name="location"></param>
        public ChessPiece(ChessSquare location, ChessColor color)
        {
            if (_isInitialized)
            {
                Location = location;
                Color = color;
                Location.Piece = this;
            } else
            {
                throw new Exception("ChessPiece hasn't been initialized to a board! Please " +
                    "call ChessPiece.Init() before constructing chess pieces!");
            }
            AvailableCaptures = new List<ChessPiece>();
        }

        /// <summary>
        /// Method to be called prior to creating any ChessPieces. This method allows for
        /// communication with the ChessBoard, to enable ChessPiece movement.
        /// </summary>
        /// <param name="chessBoard">ChessBoard instance</param>
        /// <returns>true if ChessBoard instance is not null</returns>
        public static bool Init(ChessBoard chessBoard)
        {
            if (chessBoard != null)
            {
                _isInitialized = true;
                _board = chessBoard;
            }
            return _isInitialized;
        }

        /// <summary>
        /// Method to be implemented that allows for a chess piece
        /// to move. When a chess piece moves, it must also update its current
        /// ChessSquare and inform it that it is no longer present.
        /// </summary>
        /// <param name="newLocation">New location to move this chess piece too</param>
        /// <returns>true if this chess piece moved to the new location successfully</returns>
        public virtual bool MoveTo(ChessSquare newLocation)
        {
            bool didMove = false;

            if (IsValidMove(newLocation) && !IsCaptured)
            {
                if (newLocation.IsOccupied() && IsOpponent(newLocation.Piece))
                {
                    Capture(newLocation.Piece);
                }
                UpdateLocation(newLocation);
                didMove = true;
                ++MoveCount;
            }
            return didMove;
        }

        /// <summary>
        /// Convenience method for moving to a new location by passing the string
        /// value of the location.
        /// </summary>
        /// <param name="newLocation"></param>
        /// <returns></returns>
        [Obsolete]
        public bool MoveTo(string newLocation)
        {
            return MoveTo(_board.SquareAt(newLocation));
        }

        /// <summary>
        /// Returns a List of available ChessSquare locations that this ChessPiece can legally move to.
        /// </summary>
        /// <returns>List of available ChessSquare locations to legally move to</returns>
        public virtual List<ChessSquare> GetAvailableMoves()
        {
            return new BoardScanner(this).Scan();
        }

        /// <summary>
        /// Updates the location of this ChessPiece by ensuring that the previous tile
        /// that this ChessPiece occupied, is now empty, and the newLocation ChessSquare
        /// will be occupied with this ChessPiece.
        /// </summary>
        /// <param name="newLocation"></param>
        protected void UpdateLocation(ChessSquare newLocation)
        {
            Location.ClearPiece();
            Location = newLocation;
            Location.Piece = this;
        }

        /// <summary>
        /// Method to be implemented that allows this chess piece to capture another chess piece.
        /// Capturing occurs when another ChessPiece moves onto another ChessSquare occupied by the
        /// opponent.
        /// </summary>
        /// <param name="pieceToCapture">The ChessPiece that this ChessPiece will capture</param>
        public virtual bool Capture(ChessPiece pieceToCapture)
        {
            bool didCapture = false;

            if (IsOpponent(pieceToCapture))
            {
                pieceToCapture.IsCaptured = true;
                didCapture = true;
                Debug.PrintMsg(this + " captured: " + pieceToCapture);
            } else
            {
                Debug.PrintWarning("Cannot capture pieces of the same color!");
            }
            return didCapture;
        }

        /// <summary>
        /// Checks to see the ChessPiece is an opponent to this ChessPiece.
        /// </summary>
        /// <param name="piece">ChessPiece to check</param>
        /// <returns>true if ChessPiece is an opponent</returns>
        public bool IsOpponent(ChessPiece piece)
        {
            return piece.Color != Color;
        }

        /// <summary>
        /// Checks to see if we can occupy a ChessSquare based on whether it is open,
        /// or occupied with an opponent piece.
        /// </summary>
        /// <param name="square">Square to check</param>
        /// <returns>true if this ChessPiece can occupy the specified ChessSquare</returns>
        public bool CanOccupy(ChessSquare square)
        {
            return !square.IsOccupied() || (square.IsOccupied() && IsOpponent(square.Piece));
        }

        /// <summary>
        /// Verifies that the new ChessSquare location is valid for this ChessPiece to
        /// move to, such that it is contained in the AvailableMoves and if it is 
        /// occupied, it is by an opponent that we can capture.
        /// </summary>
        /// <param name="newLocation">New location to check and see if this chess piece can move too</param>
        /// <returns>true if new location is valid</returns>
        protected virtual bool IsValidMove(ChessSquare newLocation)
        {
            bool isValid = false;

            if (GetAvailableMoves().Contains(newLocation))
            {
                // newLocation is empty OR occupied with an opponent that we can capture
                isValid = (!newLocation.IsOccupied()) ||
                          (newLocation.IsOccupied() && IsOpponent(newLocation.Piece));
            }
            return isValid;
        }
    }
}
