using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Chess.Models.Base
{
    /// <summary>
    /// Utility class for printing debug messages to the console.
    /// </summary>
    public class Debug
    {
        public static bool SHOW_MESSAGES = true;

        // foreground, background color styles for different debug types
        public static ConsoleColor[] WARNING_COLORS = { ConsoleColor.Yellow,  ConsoleColor.Black };
        public static ConsoleColor[] ERROR_COLORS   = { ConsoleColor.DarkRed, ConsoleColor.White };


        private static void Print(string msg)
        {
            if (SHOW_MESSAGES)
            {
                //Console.WriteLine(msg);
                MessageBox.Show(msg);
            }
        }

        public static void PrintColored(ConsoleColor bg, ConsoleColor fg, string txt)
        {
            SetConsoleColors(bg, fg);
            Print(txt);
            Console.ResetColor();
        }

        private static void PrintColored(ConsoleColor[] colors, string txt)
        {
            PrintColored(colors[0], colors[1], txt);
        }

        public static void SetConsoleColors(ConsoleColor bg, ConsoleColor fg)
        {
            Console.BackgroundColor = bg;
            Console.ForegroundColor = fg;
        }

        public static void PrintMsg(string msg)
        {
            Print(msg);
        }

        public static void PrintErr(string err)
        {
            PrintColored(ERROR_COLORS, err);
        }

        public static void PrintWarning(string warning)
        {
            PrintColored(WARNING_COLORS, warning);
        }
    }
}
