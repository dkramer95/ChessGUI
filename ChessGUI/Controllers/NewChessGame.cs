using Chess.Models.Base;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Windows;
using ChessGUI.Models.AI;
using Chess.Models.Pieces;
using ChessGUI.Models.SpecialMoves;
using System.Windows.Media;

namespace ChessGUI.Controllers
{
    /// <summary>
    /// This class controls alternating between player movement for a ChessGame.
    /// </summary>
    public class NewChessGame
    {
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
            Debug.SHOW_MESSAGES = true;
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
                new EnPassant(), new PawnPromotion(), new KingInCheck(),
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
                NextTurn();
            }
        }

        private bool IsGameOver()
        {
            // TODO Check to see if king is in checkmate!
            return KingInCheck.IsCheckMate();
        }

        public void ToggleCheck(KingChessPiece king)
        {
            int squareIndex = Controller.BoardModel.Squares.IndexOf(king.Location);
            Controller.BoardView.Squares[squareIndex].ToggleCheck();
        }

        /// <summary>
        /// Gets the enemy from the specified piece and returns a list of all possible
        /// moves that the enemy could make.
        /// </summary>
        /// <param name="piece">Piece to check enemy against</param>
        /// <returns>List containing all possible enemy moves</returns>
        public List<ChessSquare> GetEnemyMoves(ChessPiece piece)
        {
            List<ChessSquare> enemyMoves = new List<ChessSquare>();
            GetEnemyPieces(piece).ForEach(p => enemyMoves.AddRange(p.GetAvailableMoves()));
            return enemyMoves;
        }

        /// <summary>
        /// Gets all enemy pieces from the specified piece.
        /// </summary>
        /// <param name="piece">Piece to check enemy against</param>
        /// <returns>List containing enemy pieces</returns>
        public List<ChessPiece> GetEnemyPieces(ChessPiece piece)
        {
            List<ChessPiece> pieces = (piece.Color == ChessColor.LIGHT) ? 
                                       Controller.BoardModel.DarkPieces : 
                                       Controller.BoardModel.LightPieces;
            return pieces;
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

        public bool IsActivePlayerPiece(ChessPiece piece)
        {
            bool isActivePlayerPiece = (piece.Color == ActivePlayer.Color);
            return isActivePlayerPiece;
        }

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
