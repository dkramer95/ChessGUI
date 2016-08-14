using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chess.Models.Base;

namespace Chess.Commands
{
    class GetMovesCommand : ChessCommand
    {
        public override string Pattern
        {
            get
            {
                return @"_moves$";
            }
        }

        public override bool Execute(ChessBoard board)
        {
            Dictionary<ChessPiece, List<ChessSquare>> allMoves = new Dictionary<ChessPiece, List<ChessSquare>>();
            CurrentPlayer.Pieces.ForEach(p => allMoves.Add(p, p.GetAvailableMoves()));

            foreach (ChessPiece p in allMoves.Keys)
            {
                List<ChessSquare> movesForPiece = allMoves[p];

                movesForPiece.ForEach(s =>
                {
                    // print formatted pieces that can move and where they can move to
                    string output = string.Format("{0, -15} [{1, 2} => {2, 2}]", p, p.Location, s);

                    // print out pieces we can capture if we move
                    if (s.IsOccupied() && p.IsOpponent(s.Piece))
                    {
                        output += string.Format(" * can_capture: {0, -15}", s.Piece);
                    }
                    Console.WriteLine(output);
                });
            }
            return false;
        }
    }
}
