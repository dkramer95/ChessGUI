using Chess.Models.Base;
using ChessGUI.Views;
using System.Windows;
using System.Windows.Media;
using System.Windows.Input;
using System.Text;
using System.Collections.Generic;
using Chess.Models.Pieces;

namespace ChessGUI.Controllers
{
    /// <summary>
    /// Controller class for controlling interaction between the ChessBoard model
    /// and the ChessBoardView.
    /// </summary>
    public class ChessBoardController
    {
        public ChessBoardView BoardView { get; set; }
        public ChessBoard BoardModel { get; set; }


        public ChessBoardController()
        {
            BoardView = new ChessBoardView();
            BoardModel = new ChessBoard();
            Init();
        }

        /// <summary>
        /// Initializes the model and the view.
        /// </summary>
        private void Init()
        {
            BoardModel.Init();
            CreateGrid();
            BoardView.KeyDown += BoardView_KeyDown;
            BoardView.Focus();
        }

        public void ToggleCheck(KingChessPiece king)
        {
            ChessSquareView kingSquareView = SquareViewFromSquare(king.Location);
            if (king.InCheck)
            {
                kingSquareView.SetCheck();
            } else
            {
                kingSquareView.ResetBackground();
            }
        }

        /// <summary>
        /// Debug utility to verify that the the view is correctly representing
        /// the model, by printing out every ChessSquare and its contents.
        /// </summary>
        public void PrintBoardDebug()
        {
            StringBuilder sb = new StringBuilder();
            List<ChessSquare> squares = BoardModel.Squares;
            squares.Sort((a, b) => a.Name.CompareTo(b.Name));

            squares.ForEach(s =>
            {
                sb.Append(string.Format("Square {0} contains {1}\n", s, s.Piece));
            });
            MessageBox.Show(sb.ToString(), "Board Info");
        }

        /// <summary>
        /// Clears out all ChessSquareView previews, if they're currently highlighted.
        /// </summary>
        public void ClearPreviews()
        {
            BoardView.Squares.ForEach(s => s.ResetBackground());
        }

        /// <summary>
        /// Updates the SquareView of the specifed square. If the specified
        /// piece is not null, it assigns its image to the square view, otherwise
        /// the square piece is cleared out.
        /// </summary>
        /// <param name="squareToUpdate">ChessSquare to update</param>
        /// <param name="piece">Piece to assign to ChessSquareView</param>
        public void UpdateSquareView(ChessSquare squareToUpdate, ChessPiece piece)
        {
            ChessSquareView squareView = SquareViewFromSquare(squareToUpdate);

            if (piece != null)
            {
                squareView.PieceView.SetImageFromPiece(piece);
            }
            else
            {
                squareView.PieceView.Clear();
            }
        }

        /// <summary>
        /// Disables the entire view. This is used when CheckMate has been reached.
        /// <param name="value">bool value for enabled or disabled</param>
        /// </summary>
        public void SetViewEnabled(bool value)
        {
            BoardView.Squares.ForEach(s => s.IsEnabled = value);
        }

        /// <summary>
        /// Returns the view associated with the ChessSquare model.
        /// </summary>
        /// <param name="square">ChessSquare model</param>
        /// <returns>the view associated with the ChessSquare model</returns>
        public ChessSquareView SquareViewFromSquare(ChessSquare square)
        {
            int index = BoardModel.Squares.IndexOf(square);
            ChessSquareView squareView = BoardView.Squares[index];

            return squareView;
        }

        /// <summary>
        /// Creates the visual ChessBoard grid with appropriate data bindings.
        /// </summary>
        private void CreateGrid()
        {
            // Create a graphical representation of each square from the BoardModel
            BoardModel.Squares.ForEach(square =>
            {
                Brush squareColor = ChessBoardView.GetBrushFromChessColor(square.Color);
                ChessSquareView squareView = new ChessSquareView(squareColor);

                // Assign image if square has piece
                if (square.IsOccupied())
                {
                    string imgPath = ChessPieceView.GetChessPieceImage(square.Piece);
                    squareView.PieceView.SetImagePath(imgPath);
                }
                // Add Square to the BoardView uniform grid
                BoardView.AddSquare(squareView);
                AddClickEvents(square, squareView);
            });
        }

        /// <summary>
        /// Highlights all squares that the active MovePiece can move to.
        /// </summary>
        /// <param name="squareView">Square that we clicked on</param>
        private void ShowMovesPreview(ChessSquareView squareView)
        {
            // We clicked on the piece that we want to move
            // not the ending location
            if (squareView == MoveController.Start)
            {
                ChessPiece movePiece = MoveController.MovePiece;
                List<ChessSquare> moves = MoveController.GetCurrentValidMoves();
                moves.ForEach(s => SquareViewFromSquare(s).TogglePreview());
            }
        }

        /// <summary>
        /// Hot keys support for debug operations.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BoardView_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F8)
            {
                PrintBoardDebug();
            }
        }

        /// <summary>
        /// Adds click events to the ChessBoard for handling ChessPiece movement.
        /// </summary>
        /// <param name="squareModel">Model for the view</param>
        /// <param name="squareView">SquareView to add model and events to</param>
        private void AddClickEvents(ChessSquare squareModel, ChessSquareView squareView)
        {
            squareView.DataContext = squareModel;
            squareView.Click += Square_Click;
            squareView.MouseRightButtonDown += Square_RightClick;
        }

        /// <summary>
        /// Clears out any movement.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Square_RightClick(object sender, MouseButtonEventArgs e)
        {
            MoveController.Clear();
        }

        /// <summary>
        /// Sets piece to move or the location to move the piece to.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Square_Click(object sender, RoutedEventArgs e)
        {
            ChessSquareView squareView = sender as ChessSquareView;
            MoveController.Move(squareView);
            ShowMovesPreview(squareView);

            MoveController.Game.Advance();
        }
    }
}
