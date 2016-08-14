using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chess.Models.Base;

namespace Chess.Commands
{
    public class MoveDoubleCommand : MoveSingleCommand
    {
        public override string Pattern
        {
            get
            {
                return @"([a-h])([1-8])\s+([a-h])([1-8])\s+([a-h])([1-8])\s+([a-h])([1-8])$";
            }
        }

        public override bool Execute(ChessBoard board)
        {
            return base.Execute(board);
        }
    }
}
