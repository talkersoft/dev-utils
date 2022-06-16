using System;

namespace TemplateGenerator.Helpers
{
    public static class ScreenRenderer
    {
        public static void WriteError(string title, string message)
        {
            var currentBackgroundColor = Console.BackgroundColor;
            var currentForegroundColor = Console.ForegroundColor;

            MakeSpace();
            Console.BackgroundColor = ConsoleColor.Red;
            Console.Write(title);
            Console.WriteLine();
            Console.BackgroundColor = ConsoleColor.DarkRed;
            Console.Write("\t");
            Console.Write(message);
            Console.BackgroundColor = currentBackgroundColor;
            Console.ForegroundColor = currentForegroundColor;
        }

        public static void WriteWarning(string title, string message)
        {
            var currentBackgroundColor = Console.BackgroundColor;
            var currentForegroundColor = Console.ForegroundColor;

            MakeSpace();
            Console.BackgroundColor = ConsoleColor.Yellow;
            Console.Write(title);
            Console.WriteLine();
            Console.BackgroundColor = ConsoleColor.Magenta;
            Console.Write("\t");
            Console.Write(message);
            Console.BackgroundColor = currentBackgroundColor;
            Console.ForegroundColor = currentForegroundColor;

        }

        public static void Write(string title, string message)
        {
            var currentBackgroundColor = Console.BackgroundColor;
            var currentForegroundColor = Console.ForegroundColor;

            MakeSpace();
            Console.BackgroundColor = ConsoleColor.Green;
            Console.ForegroundColor = ConsoleColor.DarkBlue;
            Console.Write(title);
            Console.WriteLine();
            Console.WriteLine();
            Console.BackgroundColor = ConsoleColor.DarkGreen;
            Console.ForegroundColor = ConsoleColor.DarkBlue;
            Console.Write("\t");
            Console.Write(message);
            MakeSpace();
            Console.BackgroundColor = currentBackgroundColor;
            Console.ForegroundColor = currentForegroundColor;

        }

        private static void MakeSpace() {
            var currentBackgroundColor = Console.BackgroundColor;
            var currentForegroundColor = Console.ForegroundColor;

            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.BackgroundColor = ConsoleColor.DarkBlue;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.WriteLine("-----------------------------------------------------------------------------------------------");
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine();
            Console.WriteLine();
            Console.BackgroundColor = currentBackgroundColor;
            Console.ForegroundColor = currentForegroundColor;

        }
    }
}
