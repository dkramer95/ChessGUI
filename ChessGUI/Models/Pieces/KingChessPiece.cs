using Chess.Models.Base;
using System;
using System.Collections.Generic;
using Chess.Models.Utils;
using System.Windows;

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

        public override MoveDirection[] MoveDirections
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
            // limit of 1 'jump' per each move direction

            // Move King In Each of its normal valid positions.
            // Check to see if opponent pieces could capture it if it moves there

            List<ChessSquare> available = new BoardScanner(this, 1).Scan();


            // In all available positions, temporarily move the king there.

            /*
             * 
             * [ ][ ][ ][P]
             * [K][K][K][ ]
             * [K][K][K][ ]
             * [K][K][K][ ]
             * 
             */

            // store previously held pieces at each of the available squares
            // temporarily replace them with kings
            // check all opponent pieces to see if they could capture king
            // if they can, that location is invalid. Remove from available.
            // revert back



            return new BoardScanner(this, 1).Scan();
        }

        private void RemoveUnsafe(List<ChessSquare> squares)
        {

        }

        //private bool CheckCastling()
        //{
        //    bool canCastle = false;

        //    if (Location.Rank == CastlingRank)
        //    {

        //    }
        //    return canCastle;
        //}
    }
}
