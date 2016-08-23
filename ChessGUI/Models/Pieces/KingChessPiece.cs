using Chess.Models.Base;
using System.Collections.Generic;
using ChessGUI.Models.SpecialMoves;
using ChessGUI.Models.Utils;
using ChessGUI.Controllers;
using System.Windows;

namespace Chess.Models.Pieces
{
    public class KingChessPiece : ChessPiece
    {
        public bool InCheck { get; set; }

        // Castling Movement
        public Castling Castling { get; private set; }

        public KingChessPiece(ChessSquare location, ChessColor color) : base(location, color)
        {
            Init();
        }

        private void Init()
        {
            Castling = new Castling(this);
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
