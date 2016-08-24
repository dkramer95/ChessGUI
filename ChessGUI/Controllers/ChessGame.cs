using Chess.Models.Base;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Windows;
using ChessGUI.Models.AI;
using ChessGUI.Models.SpecialMoves;
using ChessGUI.Dialogs;

namespace ChessGUI.Controllers
{
    /// <summary>
    /// This class controls alternating between player movement for a ChessGame.
    /// </summary>
    public class ChessGame
    {
        private List<SpecialMove> _specialMoves;

        public ChessBoardController Controller { get; private set; }

        public ChessPlayer LightPlayer { get; private set; }
        public ChessPlayer DarkPlayer { get; private set; }
        
        // Very basic AI test player
        public BasicAI AIPlayer { get; private set; }

        // The player who is currently playing
        public ChessPlayer ActivePlayer { get; set; }

        public ChessGame()
        {
            Init();
            BeginGame();
        }

        /// <summary>
        /// Initializes everything needed to get start a game.
        /// </summary>
        private void Init()
        {
            Debug.SHOW_MESSAGES = true;
            MovementController.Game = this;

            InitSpecialMoves();

            Controller  = new ChessBoardController();
            LightPlayer = new ChessPlayer(ChessColor.LIGHT, Controller.BoardModel.LightPieces);
            DarkPlayer  = new ChessPlayer(ChessColor.DARK, Controller.BoardModel.DarkPieces);

            // TESTING
            AIPlayer = new BasicAI(DarkPlayer);
        }

        /// <summary>
        /// Creates the list of special moves that are checked after each player's move.
        /// </summary>
        private void InitSpecialMoves()
        {
            // Make sure special moves can communicate with this game
            SpecialMove.Init(this);

            _specialMoves = new List<SpecialMove>()
            {
                 new PawnPromotion(), new EnPassant(), new KingCheck(),
            };
        }

        /// <summary>
        /// Kicks off gameplay of Chess.
        /// </summary>
        private void BeginGame()
        {
            // fake out first time (really is light player who goes first)
            ActivePlayer = DarkPlayer;
            NextTurn();
        }

        /// <summary>
        /// Checks all special moves.
        /// </summary>
        public void CheckSpecialMoves()
        {
            _specialMoves.ForEach(m => m.Check());
        }

        /// <summary>
        /// Resets this ChessGame back to its initial starting context.
        /// </summary>
        private void Reset()
        {
            Init();
            BeginGame();
        }

        /// <summary>
        /// Checks to see if we have reached CheckMate. If we have, then
        /// this ChessGame is over.
        /// </summary>
        /// <returns></returns>
        public bool IsCheckMate()
        {
            List<ChessSquare> validMoves = new List<ChessSquare>();
            List<ChessPiece> playerPieces = GetPlayerPieces(ActivePlayer);

            playerPieces.ForEach(p =>
            {
                List<ChessSquare> movesForPiece = 
                    MovementController.GetValidMoves(p, ActivePlayer.KingPiece, GetOpponent());
                validMoves.AddRange(movesForPiece);
            });
            bool isCheckMate = (validMoves.Count == 0);
            return isCheckMate;
        }

        /// <summary>
        /// Advances to the next Player so that they can take their turn.
        /// </summary>
        public void NextTurn()
        {
            ClearActivePlayer();
            AdvanceActivePlayer();

            //TestAIPlayerMove();
        }

        /// <summary>
        /// Clears out the active player movement information.
        /// </summary>
        private void ClearActivePlayer()
        {
            ActivePlayer.DidMove = false;
        }

        /// <summary>
        /// Advances to the next player based on who the previous active player was.
        /// </summary>
        private void AdvanceActivePlayer()
        {
            // Alternate between Light and Dark
            ActivePlayer = (ActivePlayer == LightPlayer) ? DarkPlayer : LightPlayer;

            // Updates ChessMovement to only allow movement from pieces belonging
            // to the ActivePlayer
            MovementController.ActivePlayer = ActivePlayer;
        }

        /// <summary>
        /// Shows the pawn promotion dialog to allow player to select which piece
        /// they wish to promote their pawn to.
        /// </summary>
        public void ShowPromotionDialog()
        {
            Window w = new Window();
            w.Width = 300;
            w.Height = 350;
            w.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            w.Content = new PromotionDialog(this);
            w.ShowDialog();
        }

        /// <summary>
        /// Gets a list of all available moves for the specified Player.
        /// </summary>
        /// <param name="player">Player to get moves from</param>
        /// <returns>List of all available moves for specified Player</returns>
        public List<ChessSquare> GetPlayerMoves(ChessPlayer player)
        {
            List<ChessPiece> playerPieces = GetPlayerPieces(player);
            List<ChessSquare> playerMoves = new List<ChessSquare>();
            playerPieces.ForEach(p => playerMoves.AddRange(p.GetAvailableMoves()));
            return playerMoves;
        }

        /// <summary>
        /// Convenience method for getting the moves of the current enemy to the
        /// active player in the game.
        /// </summary>
        /// <returns></returns>
        public List<ChessSquare> GetEnemyMoves()
        {
            List<ChessSquare> enemyMoves = GetPlayerMoves(GetOpponent());
            return enemyMoves;
        }

        /// <summary>
        /// Gets all pieces for the specified ChessPlayer.
        /// </summary>
        /// <param name="player">Player that we want pieces from</param>
        /// <returns>List of ChessPieces</returns>
        public List<ChessPiece> GetPlayerPieces(ChessPlayer player)
        {
            List<ChessPiece> playerPieces = player.Pieces.FindAll(p => (!p.Ignore && !p.IsCaptured));
            return playerPieces;
        }

        /// <summary>
        /// Gets the opponent player from the specified player.
        /// </summary>
        /// <param name="player">Player to get opponent from</param>
        /// <returns>Opponent player</returns>
        public ChessPlayer GetOpponent()
        {
            ChessPlayer opponent = (ActivePlayer == LightPlayer) ? DarkPlayer : LightPlayer;
            return opponent;
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
    }
}
