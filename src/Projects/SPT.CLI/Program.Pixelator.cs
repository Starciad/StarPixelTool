using Figgle;

using SPT.CLI.Utilities;
using SPT.Core;
using SPT.Core.Constants;

using System;
using System.Diagnostics;

namespace SPT.CLI
{
    internal static partial class Program
    {
        private static void StartPixalator()
        {
            using SPTPixelator pixalator = ConfigurePixalator();
            DisplayTitleInfo();
            Stopwatch processStopwatch = StartProcessingTimer();
            DisplayProcessingStart();
            ExecutePixelationProcess(pixalator);
            DisplayProcessingFinish(processStopwatch);
            DisposeFileStreams();
        }

        private static SPTPixelator ConfigurePixalator()
        {
            return new SPTPixelator(inputFileStream, outputFileStream)
            {
                PixelateFactor = pixelateFactor,
                PaletteSize = paletteSize,
                ColorTolerance = colorTolerance,
                CustomPalette = customPallet,
                UpscaleFactor = upscaleFactor,
                ColorSpaceType = colorSpaceType,
            };
        }

        private static void DisplayTitleInfo()
        {
            string line = new('-', Console.WindowWidth - 1);

            Console.WriteLine(line);
            SPTTerminal.ApplyColorGradient(new(FiggleFonts.Ogre.Render(SPTProjectConstants.Name)));
            SPTTerminal.BreakLine();
            SPTTerminal.ApplyColor(ConsoleColor.Yellow, $"v{SPTProjectConstants.Version}");
            SPTTerminal.ApplyColor(ConsoleColor.Cyan, $" - {SPTProjectConstants.Name} - (c) Starciad <davilsfernandes.starciad.comu@gmail.com>");
            Console.WriteLine();
            Console.WriteLine("SPT is a utility that allows you to pixelate images, allowing you to apply dozens of different filters and settings.");
            Console.WriteLine();
            Console.WriteLine(line);
            Console.WriteLine();
            Console.WriteLine("Use the '--help' or '-h' command for more details on what can be done!");
        }

        private static Stopwatch StartProcessingTimer()
        {
            Stopwatch processStopwatch = new();
            processStopwatch.Start();
            return processStopwatch;
        }

        private static void DisplayProcessingStart()
        {
            SPTTerminal.BreakLine();
            SPTTerminal.ApplyColor(ConsoleColor.Gray, "╔─━━━━━━░★░━━━━━━─╗");
            SPTTerminal.BreakLine();
            SPTTerminal.BreakLine();
            SPTTerminal.ApplyColor(ConsoleColor.Blue, "PROCESSING");
            SPTTerminal.BreakLine();

            DisplayProcessingStep(ConsoleColor.Yellow, "Preparing application.");
            DisplayProcessingStep(ConsoleColor.Yellow, "Starting the pixelization process.");
        }

        private static void ExecutePixelationProcess(SPTPixelator pixalator)
        {
            pixalator.InitializePixelation();
            pixalator.ExportPixelatedImage();

            SPTTerminal.BreakLine();
            SPTTerminal.ApplyColor(ConsoleColor.Gray, "╚─━━━━━━░★░━━━━━━─╝");
            SPTTerminal.BreakLine();
        }

        private static void DisplayProcessingFinish(Stopwatch processStopwatch)
        {
            processStopwatch.Stop();

            SPTTerminal.BreakLine();
            SPTTerminal.ApplyColor(ConsoleColor.Gray, "╔─━━━━━━░★░━━━━━━─╗");
            SPTTerminal.BreakLine();
            SPTTerminal.BreakLine();
            SPTTerminal.ApplyColor(ConsoleColor.Yellow, "STATS");
            SPTTerminal.BreakLine();

            DisplayFinishDetail(ConsoleColor.Green, "Runtime", $"{processStopwatch.Elapsed.TotalSeconds}s");
            DisplayFinishDetail(ConsoleColor.Green, "Input filename", inputFileStream.Name);
            DisplayFinishDetail(ConsoleColor.Green, "Output filename", outputFileStream.Name);

            SPTTerminal.BreakLine();
            SPTTerminal.ApplyColor(ConsoleColor.Gray, "╚─━━━━━━░★░━━━━━━─╝");
            SPTTerminal.BreakLine();
            SPTTerminal.BreakLine();
        }

        private static void DisplayProcessingStep(ConsoleColor color, string step)
        {
            SPTTerminal.ApplyColor(color, "[•] ");
            Console.WriteLine(step);
        }

        private static void DisplayFinishDetail(ConsoleColor color, string label, string value)
        {
            SPTTerminal.ApplyColor(color, "[•] ");
            SPTTerminal.ApplyColor(ConsoleColor.White, $"{label}: ");
            Console.WriteLine(value);
        }

        private static void DisposeFileStreams()
        {
            inputFileStream.Dispose();
            outputFileStream.Dispose();
        }
    }
}
