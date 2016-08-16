using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessGUI.Models.Utils
{
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

        // Movement arrays
        public static Move[] NORTH = { north };

        public static Move[] SOUTH = { south };

        public static Move[] HORIZ_VERT = { north, south, east, west };

        public static Move[] DIAGONAL = { northEast, northWest,
                                          southWest, southWest }; 

        public static Move[] ALL = { north, south, east, west, northEast,
                                     northWest, southWest, southWest };
    }

    public struct Move
    {
        public int RankMoveValue { get; set; }
        public int FileMoveValue { get; set; }

        public string Name { get; set; }

        public override bool Equals(object obj)
        {
            Move move = (Move)obj;

            bool isEqual = (FileMoveValue == move.FileMoveValue) &&
                           (RankMoveValue == move.RankMoveValue);
            return isEqual;
        }
    }
}
