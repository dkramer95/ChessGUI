using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chess.Models.Base;

namespace Chess.Commands
{
    public class PlaceCommand : ChessCommand
    {
        public override string Pattern
        {
            get
            {
                return @"([KQBNRP])([ld])([a-h])([1-8])$";
            }
        }

        public override bool Execute(ChessBoard board)
        {
            char pieceSymbol = char.Parse(_match.Groups[1].Value);

            ChessColor color = GetColorFromMatch(2);
            ChessSquare location = GetSquareFromMatch(board, 3, 4);
            ChessPiece piece = ChessUtils.PieceFromSymbol(location, color, pieceSymbol);

            location.Piece = piece;

            Debug.PrintMsg(piece + " at: " + location);

            return false; //TODO REIMPLEMENT THIS LATER!!
        }
    }
}
