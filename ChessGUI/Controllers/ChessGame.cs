using Chess.Models.Base;
using System.Collections.Generic;
using System.Windows;
using ChessGUI.Models.SpecialMoves;
using ChessGUI.Dialogs;
using Chess.Models.Pieces;

namespace ChessGUI.Controllers
{
    /// <summary>
    /// This class controls alternating between player movement for a ChessGame.
    /// </summary>
    public class ChessGame
    {
        public MainWindow App { get; private set; }

        public ChessBoardController Controller { get; private set; }
        public List<SpecialMove> SpecialMoves { get; private set; }

        public ChessPlayer ActivePlayer { get; set; }
        public ChessPlayer LightPlayer { get; private set; }
        public ChessPlayer DarkPlayer { get; private set; }

        public int TurnCount { get; private set; }

        /// <summary>
        /// Constructs a new ChessGame.
        /// </summary>
        /// <param name="app">MainWindow application instance</param>
        public ChessGame(MainWindow app)
        {
            App = app;
            Init();
            BeginGame();
        }

        /// <summary>
        /// Initializes everything needed to get start a game.
        /// </summary>
        private void Init()
        {
            Debug.SHOW_MESSAGES = true;
            MoveController.Game = this;
            TurnCount = 0;

            InitSpecialMoves();

            Controller  = new ChessBoardController();
            LightPlayer = new ChessPlayer(ChessColor.LIGHT, Controller.BoardModel.LightPieces);
            DarkPlayer  = new ChessPlayer(ChessColor.DARK, Controller.BoardModel.DarkPieces);

            InitGameView();
        }

        /// <summary>
        /// Initializes the GameView so that everything can be seen on screen.
        /// </summary>
        private void InitGameView()
        {
            App.boardPanel.Children.Clear();
            App.boardPanel.Children.Add(Controller.BoardView);
        }

        /// <summary>
        /// Updates the InCheck state of the king. If a king is in check
        /// and CheckMate, the game ends.
        /// </summary>
        /// <param name="king"></param>
        public void UpdateKingCheck(KingChessPiece king)
        {
            Controller.ToggleCheck(king);

            if (king.InCheck)
            {
                // NextTurn used to preview if king is in checkmate
                NextTurn();
                if (!IsCheckMate())
                {
                    // Just in check
                    MessageBox.Show(king + " in check!");
                    NextTurn();
                } else
                {
                    GameOver();
                }
            }
        }

        /// <summary>
        /// Creates the list of special moves that are checked after each player's move.
        /// </summary>
        private void InitSpecialMoves()
        {
            // Make sure special moves can communicate with this game
            SpecialMove.Init(this);

            SpecialMoves = new List<SpecialMove>()
            {
                 new PawnPromotion(), new EnPassant(), new KingCheck(),
            };
        }

        /// <summary>
        /// Kicks off gameplay of Chess.
        /// </summary>
        private void BeginGame()
        {
            ActivePlayer = LightPlayer;
            NextTurn();
        }

        /// <summary>
        /// Checks all special moves.
        /// </summary>
        public void CheckSpecialMoves()
        {
            SpecialMoves.ForEach(m => m.Check());
        }

        /// <summary>
        /// Resets this ChessGame back to its initial starting context.
        /// </summary>
        public void Reset()
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

            // attempt to get all valid moves for each of the player pieces
            playerPieces.ForEach(p =>
            {
                List<ChessSquare> movesForPiece =
                    MoveController.GetValidMoves(p, ActivePlayer.KingPiece, GetOpponent());
                validMoves.AddRange(movesForPiece);
            });

            // if no pieces can move legally w/o removing king from check, we've hit CheckMate
            bool isCheckMate = (validMoves.Count == 0);
            return isCheckMate;
        }

        /// <summary>
        /// Advances GamePlay after a Player has made their move.
        /// </summary>
        public void Advance()
        {
            if (ActivePlayer.DidMove)
            {
                ++TurnCount;
                CheckSpecialMoves();
                NextTurn();
            }
        }

        /// <summary>
        /// Advances to the next Player so that they can take their turn.
        /// </summary>
        private void NextTurn()
        {
            ClearActivePlayer();
            NextPlayer();
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
        private void NextPlayer()
        {
            if (TurnCount > 0)
            {
                // Alternate between Light and Dark
                ActivePlayer = (ActivePlayer == LightPlayer) ? DarkPlayer : LightPlayer;
            } else
            {
                ActivePlayer = LightPlayer;
            }

            // Updates ChessMovement to only allow movement from pieces belonging
            // to the ActivePlayer
            MoveController.ActivePlayer = ActivePlayer;
        }

        /// <summary>
        /// Handles GameOver (CheckMate).
        /// </summary>
        private void GameOver()
        {
            // Display stats box
            MessageBox.Show(string.Format("CHECKMATE! {0} WINS!\n\nTotal Moves: {1}",
                GetOpponent(), TurnCount), "Game Over");

            RenderCheckMate();
            PromptPlayAgain();
        }

        /// <summary>
        /// Renders the view for CheckMate.
        /// </summary>
        private void RenderCheckMate()
        {
            Controller.SetViewEnabled(false);
        }

        /// <summary>
        /// Prompts user to play another game of chess or to exit
        /// the application.
        /// </summary>
        private void PromptPlayAgain()
        {
            PlayAgainDialog dlg = new PlayAgainDialog(this);
            dlg.Show();
        }
        
        /// <summary>
        /// Shows the pawn promotion dialog to allow player to select which piece
        /// they wish to promote their pawn to.
        /// </summary>
        public void PromptPromotion()
        {
            PromotionDialog dlg = new PromotionDialog(this);
            dlg.Show();
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
        /// Gets the opponent player from the specified player.
        /// </summary>
        /// <param name="player">Player to get opponent from</param>
        /// <returns>Opponent player</returns>
        public ChessPlayer GetOpponent()
        {
            ChessPlayer opponent = (ActivePlayer == LightPlayer) ? DarkPlayer : LightPlayer;
            return opponent;
        }
    }
}
