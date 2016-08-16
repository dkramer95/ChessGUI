using Chess.Commands;
using Chess.Models.Base;
using System.Collections.Generic;
using System.IO;

namespace Chess
{
    /// <summary>
    /// First milestone assignment. This class handles receiving a filename as a command
    /// line argument, which contains chess directives.
    /// These directives include:
    /// 1) Place a piece on board
    /// 2) Move a single piece on the board
    /// 3) Move two pieces in a single turn
    /// </summary>
    public class CommandProcessor
    {
        private List<ChessCommand> _commands = new List<ChessCommand>()
        {
            new PlaceCommand(), new MoveDoubleCommand(), new MoveSingleCommand(),
            new GetMovesCommand(),
        };

        // the current player to execute commands against
        public ChessPlayer CurrentPlayer { get; set; }

        private ChessBoard _board;

        public CommandProcessor()
        {
            // create chess board for demo of placing and moving chess pieces
            _board = new ChessBoard();
            _board.Init();  // uncomment to load board
            _board.PrintDebug();
        }

        public CommandProcessor(ChessBoard board)
        {
            _board = board;
        }

        /// <summary>
        /// Creates a StreamReader and processes a text file line by line.
        /// </summary>
        /// <param name="filename">path to text file</param>
        public void ReadFile(string filename)
        {
            StreamReader fileReader = new StreamReader(filename);
            string line = null;

            while ((line = fileReader.ReadLine()) != null)
            {
                ProcessLine(line);
            }
            _board.PrintDebug();
            fileReader.Close();
        }

        /// <summary>
        /// Checks the line against all of the available ChessCommands and
        /// attempts to execute it.
        /// </summary>
        /// <param name="line">line containing the command to execute</param>
        public bool ProcessLine(string line)
        {
            bool isValidCmd = false;
            bool isValidMove = false;

            foreach(ChessCommand cmd in _commands)
            {
                if (cmd.IsMatch(line))
                {
                    Debug.PrintMsg("Matched: " + cmd);
                    isValidCmd = true;
                    cmd.CurrentPlayer = CurrentPlayer;
                    isValidMove = cmd.Execute(_board);
                    //Console.Clear();
                    _board.PrintDebug();
                    break;
                }
            }
            //  Line contained an invalid command!
            if (!isValidCmd)
            {
                Debug.PrintErr("Invalid command: [" + line + "]");
            }
            return isValidCmd && isValidMove;
        }
    }
}
