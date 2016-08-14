using ChessGUI.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessGUI.Models.SpecialMoves
{
    /// <summary>
    /// Base class for handling special moves.
    /// </summary>
    public abstract class SpecialMove
    {
        private static bool _isInitialized;
        public static NewChessGame Game { get; set; }

        public SpecialMove()
        {
            if (!_isInitialized)
            {
                throw new Exception("Call Init() on SpecialMove before constructing them!");
            }
        }

        public static void Init(NewChessGame game)
        {
            if (game != null)
            {
                Game = game;
                _isInitialized = true;
            }
        }

        public abstract void Check();
    }
}
