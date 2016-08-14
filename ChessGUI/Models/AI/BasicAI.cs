using Chess.Models.Base;
using ChessGUI.Controllers;
using System;
using System.Collections.Generic;

namespace ChessGUI.Models.AI
{
    /// <summary>
    /// NON STRATEGIC AI. JUST PURELY RANDOM. WILL IMPLEMENT BETTER SOLUTION LATER.
    /// </summary>
    public class BasicAI
    {
        private Random _rng;
        public ChessPlayer Player { get; private set; }

        public BasicAI(ChessPlayer player)
        {
            Player = player;
            _rng = new Random();
        }

        public void GetMove(ChessBoardController controller)
        {
            ChessPiece pieceToMove = null;
            ChessSquare startLocation = null;
            ChessSquare moveLocation = null;
            List<ChessSquare> availableMoves = new List<ChessSquare>();

            do
            {
                int randPieceIndex = _rng.Next(0, Player.Pieces.Count - 1);
                pieceToMove = Player.Pieces[randPieceIndex];
                startLocation = pieceToMove.Location;
                availableMoves = pieceToMove.GetAvailableMoves();

                int randMoveIndex = _rng.Next(0, availableMoves.Count);
                if (availableMoves.Count > 0)
                {
                    moveLocation = availableMoves[randMoveIndex];
                }

            } while (availableMoves.Count == 0);

            //int startIndex = controller.BoardModel.Squares.IndexOf(startLocation);
            //int endIndex = controller.BoardModel.Squares.IndexOf(moveLocation);

            controller.SquareViewFromSquare(startLocation).PieceView.Clear();
            controller.SquareViewFromSquare(moveLocation).PieceView.SetImageFromPiece(pieceToMove);

            //controller.BoardView.SquareViews[startIndex].PieceView.Clear();
            //controller.BoardView.SquareViews[endIndex].PieceView.SetImagePath(ChessPieceView.GetChessPieceImage(pieceToMove));

            pieceToMove.MoveTo(moveLocation);
        }
    }
}
