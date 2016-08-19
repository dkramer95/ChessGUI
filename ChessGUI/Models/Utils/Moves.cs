using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessGUI.Models.Utils
{
    /// <summary>
    /// Static class containing move information. For each movement, this defines
    /// the advancement for File and/or rank. ChessPieces use the movement arrays
    /// to define all supported moves.
    /// </summary>
    public static class Moves
    {
        public static Move north = new Move() { FileMoveValue = 0, RankMoveValue = 1, Name = "North" };
        public static Move south = new Move() { FileMoveValue = 0, RankMoveValue = -1, Name = "South" };
        public static Move east  = new Move() { FileMoveValue = 1, RankMoveValue = 0, Name = "East" };
        public static Move west  = new Move() { FileMoveValue = -1, RankMoveValue = 0, Name = "West" };

        public static Move northEast = new Move() { FileMoveValue = 1, RankMoveValue = 1, Name = "NorthEast" };
        public static Move northWest = new Move() { FileMoveValue = -1, RankMoveValue = 1, Name = "NorthWest" };
        public static Move southEast = new Move() { FileMoveValue = 1, RankMoveValue = -1, Name = "SouthEast" };
        public static Move southWest = new Move() { FileMoveValue = -1, RankMoveValue = -1, Name = "SouthWest" };

        // Movement arrays -- used by ChessPieces

        // Light Pawn
        public static Move[] NORTH = { north };

        // Dark Pawn
        public static Move[] SOUTH = { south };

        // Rook and Knight
        public static Move[] HORIZ_VERT = { north, south, east, west };

        // Bishop
        public static Move[] DIAGONAL = { northEast, northWest,
                                          southEast, southWest }; 

        // King / Queen
        public static Move[] ALL = { north, south, east, west, northEast,
                                     northWest, southEast, southWest };
    }

    /// <summary>
    /// Simple struct for encapsulating movement information.
    /// </summary>
    public struct Move
    {
        // Rank advancement value
        public int RankMoveValue { get; set; }

        // File advancement value
        public int FileMoveValue { get; set; }

        // Name identifier for type of move
        public string Name { get; set; }
    }
}
