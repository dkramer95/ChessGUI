using Chess.Models.Base;
using Chess.Models.Pieces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ChessGUI.Models.SpecialMoves
{
    public class KingInCheck : SpecialMove
    {
        public override void Check()
        {
            IsInCheck();
        }

        //public static void IsInCheck()
        //{
        //    KingChessPiece kingPiece = Game.ActivePlayer.KingPiece;

        //    // list to hold all possible moves from all the pieces
        //    List<ChessSquare> allMoves = new List<ChessSquare>();

        //    // Populate with all moves that the player can make against the opponent
        //    ChessPlayer opponent = Game.GetOpponent(Game.ActivePlayer);
        //    opponent.Pieces.ForEach(p => allMoves.AddRange(p.GetAvailableMoves()));

        //    // check moves to see if any of them would land on the king
        //    allMoves = allMoves.FindAll(s => (s == kingPiece.Location));
        //    kingPiece.InCheck = (allMoves.Count > 0);

        //    if (kingPiece.InCheck)
        //    {
        //        MessageBox.Show("IN CHECK");
        //        Game.Controller.SquareViewFromSquare(kingPiece.Location).ToggleCheck();
        //    }
        //}

        public static void IsInCheck()
        {
            KingChessPiece king = Game.ActivePlayer.KingPiece;
        }
    }
}
