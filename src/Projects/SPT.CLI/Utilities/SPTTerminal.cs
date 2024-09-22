using System;
using System.IO;

namespace SPT.CLI.Utilities
{
    internal static class SPTTerminal
    {
        internal static void ApplyColor(ConsoleColor color, string value)
        {
            Console.ForegroundColor = color;
            Console.Write(value);
            Console.ForegroundColor = ConsoleColor.White;
        }

        internal static void BreakLine()
        {
            Console.WriteLine();
        }

        internal static void SetForegroundColor(ConsoleColor color)
        {
            Console.ForegroundColor = color;
        }

        internal static void ApplyColorGradient(StringReader reader)
        {
            string line;

            while ((line = reader.ReadLine()) != null)
            {
                for (int i = 0; i < line.Length; i++)
                {
                    SetForegroundColor(GetGradientColor(i, line.Length));
                    Console.Write(line[i]);
                }

                BreakLine();
            }

            Console.ResetColor();
        }

        private static ConsoleColor GetGradientColor(int index, int length)
        {
            double ratio = (double)index / (length - 1);
            int colorIntensity = (int)(ratio * 15);
            return (ConsoleColor)colorIntensity;
        }
    }
}