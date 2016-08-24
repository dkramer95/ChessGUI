namespace ChessGUI.Models.Utils
{
    /// <summary>
    /// Static class containing move information. For each movement, this defines
    /// the advancement for File and/or Rank. ChessPieces use the movement arrays
    /// to define all supported moves.
    /// </summary>
    public static class Moves
    {
        // Directions that define movement throughout the ChessBoard
        public static readonly Move north 
            = new Move() { FileMoveValue = 0,  RankMoveValue = 1,  Name = "North" };

        public static readonly Move south 
            = new Move() { FileMoveValue = 0,  RankMoveValue = -1, Name = "South" };

        public static readonly Move east  
            = new Move() { FileMoveValue = 1,  RankMoveValue = 0,  Name = "East" };

        public static readonly Move west  
            = new Move() { FileMoveValue = -1, RankMoveValue = 0,  Name = "West" };

        public static readonly Move northEast 
            = new Move() { FileMoveValue = 1,  RankMoveValue = 1,  Name = "NorthEast" };

        public static readonly Move northWest 
            = new Move() { FileMoveValue = -1, RankMoveValue = 1,  Name = "NorthWest" };

        public static readonly Move southEast 
            = new Move() { FileMoveValue = 1,  RankMoveValue = -1, Name = "SouthEast" };

        public static readonly Move southWest 
            = new Move() { FileMoveValue = -1, RankMoveValue = -1, Name = "SouthWest" };



        // Movement arrays -- used by ChessPieces

        // Light Pawn
        public static readonly Move[] NORTH = { north };

        // Dark Pawn
        public static readonly Move[] SOUTH = { south };

        // Rook and Knight
        public static readonly Move[] HORIZ_VERT 
            = { north, south, east, west };

        // Bishop
        public static readonly Move[] DIAGONAL 
            = { northEast, northWest, southEast, southWest }; 

        // King / Queen
        public static readonly Move[] ALL 
            = { north, south, east, west, northEast,
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
