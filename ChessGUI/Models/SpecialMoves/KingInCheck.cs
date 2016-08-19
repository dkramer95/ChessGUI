using System;

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
            throw new NotImplementedException();
        }
    }
}
