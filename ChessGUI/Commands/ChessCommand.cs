using Chess.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Chess.Commands
{

    /// <summary>
    /// Base class for all ChessCommands.
    /// </summary>
    public abstract class ChessCommand
    {
        protected Match _match;

        // the current player to execute commands against
        public ChessPlayer CurrentPlayer { get; set; }

        /// <summary>
        /// The pattern we are using to check matches against.
        /// </summary>
        public abstract string Pattern { get; }

        /// <summary>
        /// Checks to see if the cmd string matches the defined Pattern. If it
        /// does, this match is updated so that we can process it later.
        /// </summary>
        /// <param name="cmd">cmd string to check against pattern</param>
        /// <returns>true if match was found</returns>
        public bool IsMatch(string cmd)
        {
            bool isMatch = Regex.IsMatch(cmd, Pattern, RegexOptions.IgnoreCase);
            _match = isMatch ? Regex.Match(cmd, Pattern, RegexOptions.IgnoreCase) : null;
            return isMatch;
        }

        /// <summary>
        /// Gets the ChessSquare from the specified file and rank indexes, using the
        /// current match instance.
        /// </summary>
        /// <param name="board">ChessBoard context</param>
        /// <param name="fileIndex">index of file in current match context</param>
        /// <param name="rankIndex">index of rank in current match context</param>
        /// <returns>ChessSquare from file/rank in match</returns>
        protected ChessSquare GetSquareFromMatch(ChessBoard board, int fileIndex, int rankIndex)
        {
            char file = char.Parse(_match.Groups[fileIndex].Value);
            int rank = int.Parse(_match.Groups[rankIndex].Value);
            return board.SquareAt(file, rank);
        }

        /// <summary>
        /// Gets the ChessColor from the specified color index, using the current
        /// match instance.
        /// </summary>
        /// <param name="colorIndex">index of color in current match context</param>
        /// <returns>ChessColor from color in match</returns>
        protected ChessColor GetColorFromMatch(int colorIndex)
        {
            char colorSymbol = char.Parse(_match.Groups[colorIndex].Value);
            return ChessUtils.ColorFromSymbol(colorSymbol);
        }

        /// <summary>
        /// Method to be implemented for executing a command based on a match
        /// with the pattern.
        /// <param name="board">ChessBoard context</param>
        /// </summary>
        public abstract bool Execute(ChessBoard board);
    }
}
