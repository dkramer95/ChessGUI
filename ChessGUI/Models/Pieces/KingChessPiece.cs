using Chess.Models.Base;
using System.Collections.Generic;
using ChessGUI.Models.SpecialMoves;
using ChessGUI.Models.Utils;

namespace Chess.Models.Pieces
{
    public class KingChessPiece : ChessPiece
    {
        // Castling Movement
        public Castling Castling { get; private set; }
        public bool InCheck { get; set; }
        public override char Symbol { get { return 'K'; } }
        public override Move[] MoveDirections { get { return Moves.ALL; } }

        // technically, kings value is infinite because game is over when captured!
        public override int Value { get { return 10000; } }
        public override string ToString() { return Color + "_King"; }


        public KingChessPiece(ChessSquare location, ChessColor color) : base(location, color)
        {
            Init();
        }

        private void Init()
        {
            Castling = new Castling(this);
        }

        public override List<ChessSquare> GetAvailableMoves()
        {
            List<ChessSquare> available = BoardScanner.Scan(this, 1);
            Castling.Check(ref available);
            return available;
        }

        public override bool MoveTo(ChessSquare newLocation)
        {
            bool didMove = base.MoveTo(newLocation);
            Castling.CheckMovement(newLocation);
            return didMove;
        }
    }
}
