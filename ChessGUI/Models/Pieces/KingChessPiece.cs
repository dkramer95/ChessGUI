using Chess.Models.Base;
using System.Collections.Generic;
using ChessGUI.Models.SpecialMoves;
using ChessGUI.Models.Utils;

namespace Chess.Models.Pieces
{
    public class KingChessPiece : ChessPiece
    {
        public int CastlingRank { get; private set; }

        public bool InCheck { get; set; }

        public KingChessPiece(ChessSquare location, ChessColor color) : base(location, color)
        {
            Init();
        }

        private void Init()
        {
            CastlingRank = (Color == ChessColor.LIGHT) ? ChessBoard.MIN_FILE : ChessBoard.MAX_RANK;
        }

        public override char Symbol
        {
            get
            {
                return 'K';
            }
        }

        public override Move[] MoveDirections
        {
            get
            {
                return Moves.ALL;
            }
        }

        public override int Value
        {
            get
            {
                // technically, kings value is infinite because game is over when captured!
                return 10000;
            }
        }

        public override string ToString()
        {
            return Color + "_King";
        }

        //public override List<ChessSquare> GetAvailableMoves()
        //{
        //    //List<ChessSquare> available = new BoardScanner(this, 1).Scan();
        //    List<ChessSquare> available = null;
        //    // filter out moves that would put this king in check
        //    KingInCheck.RemoveUnsafe(this, ref available);
        //    return available;
        //}
    }
}
