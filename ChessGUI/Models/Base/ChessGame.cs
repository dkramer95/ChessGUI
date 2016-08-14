using System;
using System.Collections.Generic;

namespace Chess.Models.Base
{
    /// <summary>
    /// This class allows for a gameplay to occur by allowing players to alternate
    /// and move their pieces around on the ChessBoard.
    /// </summary>
    public class ChessGame
    {
        private ChessBoard _board;
        private ChessPlayer _lightPlayer;
        private ChessPlayer _darkPlayer;

        // ChessPlayer who is currently in the middle of moving
        private ChessPlayer _activePlayer;
        private CommandProcessor _cmdProcessor;

        private bool _isGameOver;

        public ChessGame()
        {
            Init();
            BeginGame();
        }

        private void Init()
        {
            _board = new ChessBoard();
            _board.Init();
            _lightPlayer = new ChessPlayer(ChessColor.LIGHT, _board.LightPieces);
            _darkPlayer = new ChessPlayer(ChessColor.DARK, _board.DarkPieces);
            _cmdProcessor = new CommandProcessor(_board);

        }

        public void Play()
        {
            while (!_isGameOver)
            {
                // alternate and get moves between each player
                PromptMove();
            }
        }

        private void PromptMove()
        {
            PrintPrompt();
            ReadCommand();
        }

        private void PrintPrompt()
        {
            ColorizeConsole();
            Console.Write(_activePlayer + "'s move: ");
            Console.ResetColor();
        }

        /// <summary>
        /// Reads in a string on the current line in the console and attempts
        /// process it.
        /// </summary>
        private void ReadCommand()
        {
            string command = Console.ReadLine();
            if (_cmdProcessor.ProcessLine(command))
            {
                NextTurn();
            }
        }

        private void ColorizeConsole()
        {
            ChessColor playerColor = _activePlayer.Color;
            ConsoleColor bg = (playerColor == ChessColor.DARK) ? ConsoleColor.DarkGray : ConsoleColor.White;
            ConsoleColor fg = ConsoleColor.Black;
            Debug.SetConsoleColors(bg, fg);
        }

        private void BeginGame()
        {
            // fake out first time --> really is light player who goes first
            _activePlayer = _darkPlayer;

            NextTurn();
            _board.PrintDebug();
            Play();
        }

        /// <summary>
        /// Advances the turn to the next player.
        /// </summary>
        private void NextTurn()
        {
            _activePlayer.IsCurrentMove = false;
            _activePlayer = (_activePlayer == _lightPlayer) ? _darkPlayer : _lightPlayer;
            _activePlayer.IsCurrentMove = true;
            _cmdProcessor.CurrentPlayer = _activePlayer;
            _board.PrintDebug();

            if(IsKingInCheck())
            {
                Debug.PrintMsg("KING IS IN CHECK! YOU MUST MOVE TO SAFETY!");
                _board.PrintDebug();
            }
        }

        private bool IsKingInCheck()
        {
            // for each piece that belongs to the active player,
            // if the available moves contains an opponent king, that oppponent
            // is in check!!!

            bool inCheck = false;

            foreach (ChessPiece p in _activePlayer.Pieces)
            {
                List<ChessSquare> movesForPiece = p.GetAvailableMoves();
                
                foreach (ChessSquare s in movesForPiece)
                {
                    if (s.IsOccupied() && p.IsOpponent(s.Piece) && s.Piece.Symbol == 'K')
                    {
                        inCheck = true;
                        break;
                    }
                }
            }
            return inCheck;
        }

    }
}
