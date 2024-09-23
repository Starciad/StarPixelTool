using SkiaSharp;

using SPT.CLI.Interactivity;
using SPT.Core.Enums;
using SPT.Core.Palettes;

using System;
using System.IO;
using System.Text;

namespace SPT.CLI
{
    internal static partial class Program
    {
        private static readonly SPTCommandRegistry commandRegistry = new();

        private static FileStream inputFileStream;
        private static FileStream outputFileStream;

        // Transform
        private static uint pixelateFactor = 16;
        private static sbyte colorTolerance = 16;
        private static uint upscaleFactor = 1;
        private static SPTColorSpaceType colorSpaceType = SPTColorSpaceType.RGB;

        // Color Palette
        private static SPTPalette customPallet = null;
        private static uint paletteSize = 8;

        [STAThread]
        private static int Main(string[] args)
        {
            Console.InputEncoding = Encoding.Unicode;
            Console.OutputEncoding = Encoding.Unicode;

            SPTArgumentParser parser = new(args);

            if (args.Length == 0)
            {
                DisplayTitleInfo();
            }
            else
            {
                RegisterCommands();
                ExecuteCommands(parser);
                StartPixalator();
            }

            return 0;
        }
    }
}