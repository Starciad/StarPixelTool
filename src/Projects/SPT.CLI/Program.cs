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

        private static uint pixelateFactor = 16;
        private static uint paletteSize = 8;
        private static SPTPalette customPallet = null;
        private static sbyte colorTolerance = 16;
        private static uint upscaleFactor = 1;
        private static SPTColorSpaceType colorSpaceType = SPTColorSpaceType.RGB;

        [STAThread]
        private static int Main(string[] args)
        {
            Console.InputEncoding = Encoding.Unicode;
            Console.OutputEncoding = Encoding.Unicode;

            SPTArgumentParser parser = new(args);

            RegisterCommands();
            ExecuteCommands(parser);
            StartPixalator();

            return 0;
        }
    }
}