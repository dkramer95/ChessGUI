using Chess.Models.Base;
using ChessGUI.Views;
using System.Windows;
using System.Windows.Media;
using System.Windows.Input;
using System.Collections.Generic;
using System;
using System.Windows.Data;
using ChessGUI.Converters;

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

        private void Init()
        {
            BoardModel.Init();
            CreateGrid();
            BoardView.KeyDown += BoardView_KeyDown;
        }


        private void BoardView_KeyDown(object sender, KeyEventArgs e)
        {
            // show preview of moves we can make
            if (e.Key == Key.F5)
            {
                ShowMovesPreview();
            }
        }

        private void ShowMovesPreview()
        {
            if (ChessMovement.Start != null)
            {
                ChessSquare start = ChessMovement.Start.DataContext as ChessSquare;
                List<ChessSquare> moves = start.Piece.GetAvailableMoves();
                moves.ForEach(s => SquareViewFromSquare(s).TogglePreview());
            }
        }

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
                //BoardView.Children.Add(squareView);
                BoardView.AddSquare(squareView);
                AddClickEvents(square, squareView);
            });
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


            ///// TESTING ///
            //// Bind image square to board model square
            //ChessPieceView pieceView = squareView.PieceView;
            //Binding pieceViewBinding = new Binding("Piece");
            //squareView.DataContext = squareModel;
            //pieceView.DataContext = squareModel;
            //pieceViewBinding.Mode = BindingMode.TwoWay;
            //pieceViewBinding.Converter = new ChessImageConverter();
            //pieceView.SetBinding(ChessPieceView.BackgroundProperty, pieceViewBinding);
            ///// TESTING ///
        }

        /// <summary>
        /// Clears out any movement.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Square_RightClick(object sender, MouseButtonEventArgs e)
        {
            ChessMovement.Clear();
        }

        /// <summary>
        /// Sets piece to move or the location to move the piece to.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Square_Click(object sender, RoutedEventArgs e)
        {
            ChessSquareView squareView = sender as ChessSquareView;
            ChessMovement.Move(squareView);
        }
    }
}
