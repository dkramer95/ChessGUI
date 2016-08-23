using Chess.Models.Base;
using Chess.Models.Pieces;
using System;
using System.Collections.Generic;
using System.Windows;

namespace ChessGUI.Models.SpecialMoves
{
    /// <summary>
    /// This class handles checking if the KingChessPiece is in Check/CheckMate.
    /// This class also provides utilities for movement checking such that any
    /// pieces moved, including the KingChessPiece itself, doesn't not put the
    /// King at risk for capture.
    /// </summary>
    public class KingCheck : SpecialMove
    {
        public override void Check()
        {
            IsInCheck();
        }

        public static bool IsInCheck()
        {
            KingChessPiece king = Game.GetOpponent().KingPiece;
            List<ChessSquare> playerMoves = Game.GetPlayerMoves(Game.ActivePlayer);

            if (playerMoves.Contains(king.Location))
            {
                MessageBox.Show(king + " is in check!");
                king.InCheck = true;
            } else
            {
                king.InCheck = false;
            }

            return king.InCheck;
        }
    }
}
