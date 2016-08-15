using System;
using Chess.Models.Base;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Windows;
using ChessGUI.Models.AI;
using Chess.Models.Pieces;
using ChessGUI.Models.SpecialMoves;
using System.Windows.Media;
using System.Threading;
using System.Windows.Threading;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ChessGUI.Controllers
{
    /// <summary>
    /// This class controls alternating between player movement for a ChessGame.
    /// </summary>
    public class NewChessGame
    {
        private bool _isGameOver;

        private List<SpecialMove> _specialMoves;

        public BasicAI AIPlayer { get; private set; }

        public ChessBoardController Controller { get; private set; }
        public ChessPlayer LightPlayer { get; private set; }
        public ChessPlayer DarkPlayer { get; private set; }

        // The player who is currently playing
        public ChessPlayer ActivePlayer { get; set; }

        public NewChessGame()
        {
            Init();
            BeginGame();
        }

        private void Init()
        {
            Debug.SHOW_MESSAGES = false;
            ChessMovement.Game = this;

            InitSpecialMoves();

            Controller  = new ChessBoardController();
            LightPlayer = new ChessPlayer(ChessColor.LIGHT, Controller.BoardModel.LightPieces);
            DarkPlayer  = new ChessPlayer(ChessColor.DARK, Controller.BoardModel.DarkPieces);

            // TESTING
            AIPlayer = new BasicAI(DarkPlayer);
        }

        private void InitSpecialMoves()
        {
            SpecialMove.Init(this);

            _specialMoves = new List<SpecialMove>()
            {
                new EnPassant(), new KingInCheck(), new PawnPromotion(),
            };
        }

        private void BeginGame()
        {
            // fake out first time (really is light player who goes first)
            ActivePlayer = DarkPlayer;

            NextTurn();
            Play();
        }

        private async void Play()
        {
            while (!IsGameOver())
            {
                do
                {
                    await Task.Delay(50);
                } while (!ActivePlayer.DidMove);
                _specialMoves.ForEach(m => m.Check());
                Controller.BoardView.Squares.ForEach(s => s.Background = Brushes.Red);
                NextTurn();
            }
        }

        private bool IsGameOver()
        {
            // TODO Check to see if king is in checkmate!
            return false;
        }

        private bool IsKingInCheck()
        {
            KingChessPiece kingPiece = GetOpponent(ActivePlayer).KingPiece;

            // list to hold all possible moves from all the pieces
            List<ChessSquare> allMoves = new List<ChessSquare>();

            // Populate with all moves that the player can make against the opponent
            ChessPlayer opponent = ActivePlayer;
            opponent.Pieces.ForEach(p => allMoves.AddRange(p.GetAvailableMoves()));

            // check moves to see if any of them would land on the king
            allMoves = allMoves.FindAll(s => (s == kingPiece.Location));
            kingPiece.InCheck = (allMoves.Count > 0);

            if (kingPiece.InCheck)
            {
                MessageBox.Show("IN CHECK");
                Controller.SquareViewFromSquare(kingPiece.Location).ToggleCheck();
            }
            return kingPiece.InCheck;
        }

        /// <summary>
        /// Gets the opponent player from the specified player.
        /// </summary>
        /// <param name="player">Player to get opponent from</param>
        /// <returns>Opponent player</returns>
        public ChessPlayer GetOpponent(ChessPlayer player)
        {
            ChessPlayer opponent = (ActivePlayer == LightPlayer) ? DarkPlayer : LightPlayer;
            return opponent;
        }

        /// <summary>
        /// Checks to see if the last piece moved was a pawn and whether or not
        /// it has reached its end rank position. If it reached, it is promoted.
        /// </summary>
        //private void CheckPawnPromotion()
        //{
        //    if (ChessMovement.MovePiece is PawnChessPiece)
        //    {
        //        PawnChessPiece pawn = ChessMovement.MovePiece as PawnChessPiece;
        //        if (pawn.CanPromote())
        //        {
        //            ChessSquare square = pawn.Location;
        //            square.Piece = new QueenChessPiece(square, pawn.Color);
        //            //Controller.SquareViewFromSquare(square).PieceView.SetImageFromPiece(square.Piece);
        //            Controller.UpdateSquareView(square, square.Piece);

        //            // TODO create a promotion dialog to allow player to choose what they
        //            // are promoted to. Right now it's just queen, which is the most common
        //            // promotion!
        //        }
        //    }
        //}

        /// <summary>
        /// Advances to the next Player so that they can take their turn.
        /// </summary>
        private void NextTurn()
        {
            ClearActivePlayer();
            AdvanceActivePlayer();

            //TestAIPlayerMove();
        }

        // TESTING BASIC BASIC AIPLAYER MOVEMENT. TODO REMOVE THIS AND REPLACE WITH
        // BETTER AI IN THE FUTURE!!!
        private async void TestAIPlayerMove()
        {
            // TESTING AI PLAYER MOVEMENT
            if (AIPlayer.Player == ActivePlayer)
            {
                await Task.Delay(3000);
                AIPlayer.GetMove(Controller);
                AIPlayer.Player.DidMove = true;
            }
        }

        /// <summary>
        /// Clears out the active player movement information.
        /// </summary>
        private void ClearActivePlayer()
        {
            ActivePlayer.IsCurrentMove = false;
            ActivePlayer.DidMove = false;
        }

        /// <summary>
        /// Advances to the next player based on who the previous active player was.
        /// </summary>
        private void AdvanceActivePlayer()
        {
            ActivePlayer = (ActivePlayer == LightPlayer) ? DarkPlayer : LightPlayer;
            ActivePlayer.IsCurrentMove = true;

            ChessMovement.ActivePlayer = ActivePlayer;
            Controller.BoardView.Squares.ForEach(s => s.ResetBackground());
        }
    }
}
