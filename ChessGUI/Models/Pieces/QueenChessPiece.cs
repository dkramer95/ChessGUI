using Chess.Models.Base;
using ChessGUI.Models.Utils;

namespace Chess.Models.Pieces
{
    public class QueenChessPiece : ChessPiece
    {
        public override Move[] MoveDirections { get { return Moves.ALL; } }
        public override char Symbol { get { return 'Q'; } }
        public override int Value { get { return 1000; } }
        public override string ToString() { return Color + "_Queen"; }

        public QueenChessPiece(ChessSquare location, ChessColor color) : base(location, color)
        {
        }
    }
}
