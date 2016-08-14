using Chess.Models.Base;
using System;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ChessGUI.Views
{
    /// <summary>
    /// This class represents a graphical representation of a ChessPiece on top of
    /// a ChessSquareView. All ChessSquares have a ChessPieceView but it may or may
    /// not contain an actual image (i.e. the ChessSquare is not occupied.) In that
    /// case, the visual is simply transparent.
    /// </summary>
    public class ChessPieceView : Label
    {
        // Path to ChessPiece resource image 
        public string ImagePath { get; private set; }

        public ChessPieceView(string path)
        {
            Init();
            SetImagePath(path);
        }

        public ChessPieceView()
        {
            Init();
            Background = Brushes.Transparent;
        }

        /// <summary>
        /// Initializes the sizing of a ChessPieceView.
        /// </summary>
        private void Init()
        {
            Width = 40;
            Height = 40;
        }

        /// <summary>
        /// Utility method to extract the image path resource from the specified
        /// ChessPiece.
        /// </summary>
        /// <param name="piece"></param>
        /// <returns>image resource path string</returns>
        public static string GetChessPieceImage(ChessPiece piece)
        {
            string path = string.Format("res/{0}/{1}.png", piece.Color, piece);
            return path;
        }

        public void SetImageFromPiece(ChessPiece piece)
        {
            //SetImagePath(GetChessPieceImage(piece));
        }

        /// <summary>
        /// Sets the image path so that the image brush can properly render the image.
        /// </summary>
        /// <param name="path">path to image resource</param>
        public void SetImagePath(string path)
        {
            //ImageBrush imgBrush = new ImageBrush(new BitmapImage(new Uri(path, UriKind.Relative)));
            //imgBrush.Stretch = Stretch.Uniform;
            //Background = imgBrush;
        }

        /// <summary>
        /// Clears out the image and replaces it with a transparent background.
        /// </summary>
        public void Clear()
        {
            //Background = Brushes.Transparent;
        }
    }
}
