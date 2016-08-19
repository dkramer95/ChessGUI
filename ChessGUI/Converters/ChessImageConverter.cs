using Chess.Models.Base;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ChessGUI.Converters
{
    [Obsolete]
    class ChessImageConverter : IValueConverter
    {
        // Convert a chess piece to its appropriate image
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {

            if (targetType != typeof(Brush))
            {
                throw new Exception("Target type must be an image brush");
            }
            Brush brush = null;

            ChessPiece piece = value as ChessPiece;

            if (piece != null)
            {
                string imgPath = GetChessPieceImage(piece);
                ImageBrush imgBrush = new ImageBrush(new BitmapImage(new Uri(imgPath, UriKind.Relative)));
                imgBrush.Stretch = Stretch.Uniform;
                brush = imgBrush;
            }
            else
            {
                brush = Brushes.Transparent;
            }
            return brush;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Brushes.Black;
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
    }
}
