using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chess.Models.Base;

namespace Chess.Commands
{
    public class MoveSingleCommand : ChessCommand
    {
        public override string Pattern
        {
            get
            {
                return @"([a-h])([1-8])\s+([a-h])([1-8])(\*)?$";
            }
        }

        public override bool Execute(ChessBoard board)
        {
            bool success = false;
            int count = _match.Groups.Count;

            ChessSquare startSquare = GetSquareFromMatch(board, 1, 2);

            if (startSquare.IsOccupied())
            {
                ChessPiece movePiece = startSquare.Piece;
                ChessSquare endSquare = GetSquareFromMatch(board, 3, 4);

                success = (CurrentPlayer.Color == movePiece.Color) && movePiece.MoveTo(endSquare);

                if (!success)
                {
                    Debug.PrintWarning(string.Format("Invalid for {0} at {1} to move to {2}", startSquare.Piece, startSquare, endSquare));
                }
            } else
            {
                Debug.PrintWarning("There is no piece to move on: " + startSquare);
            }
            return success;
        }
    }
}
