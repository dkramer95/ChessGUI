using System;
using Chess.Models.Pieces;

namespace Chess.Models.Base
{
    public class ChessUtils
    {

        /// <summary>
        /// Returns the proper ChessColor from the specified symbol.
        /// </summary>
        /// <param name="symbol"></param>
        /// <returns>Returns the proper ChessColor from the specified symbol.</returns>
        public static ChessColor ColorFromSymbol(char symbol)
        {
            switch (char.ToUpper(symbol))
            {
                case 'L':
                    return ChessColor.LIGHT;
                case 'D':
                    return ChessColor.DARK;
                default:
                    throw new Exception("Invalid chess color symbol!");
            }
        }


        /// <summary>
        /// Helpful factory method to return a chess piece based on the specified shorthand
        /// char symbol.
        /// </summary>
        /// <param name="location">Location of where to place piece</param>
        /// <param name="color">Color to assign to piece</param>
        /// <param name="symbol">The symbol type of the chess piece</param>
        /// <returns></returns>
        public static ChessPiece PieceFromSymbol(ChessSquare location, ChessColor color, char symbol)
        {
            ChessPiece piece = null;

            switch (char.ToUpper(symbol))
            {
                case 'P':
                    piece = new PawnChessPiece(location, color);
                    break;
                case 'N':
                    piece = new KnightChessPiece(location, color);
                    break;
                case 'B':
                    piece = new BishopChessPiece(location, color);
                    break;
                case 'R':
                    piece = new RookChessPiece(location, color);
                    break;
                case 'Q':
                    piece = new QueenChessPiece(location, color);
                    break;
                case 'K':
                    piece = new KingChessPiece(location, color);
                    break;
                default:
                    throw new ArgumentException("Invalid ChessPiece symbol! Must be 'P', 'N', 'B', 'R', 'Q', or 'K'");
            }
            return piece;
        }
    }
}
