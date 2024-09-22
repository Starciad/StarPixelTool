using Figgle;

using SPT.CLI.Utilities;
using SPT.Core;
using SPT.Core.Constants;
using SPT.Core.Palettes;
using SPT.Core.Palettes.Serializers;
using SPT.Core.Pixelization;

using System;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace SPT.CLI
{
    internal static class Program
    {
        internal static string BaseDirectory => AppDomain.CurrentDomain.BaseDirectory;

        private static FileStream inputFileStream;
        private static FileStream outputFileStream;

        private static int pixelateFactor = 2;
        private static int paletteSize = 32;
        private static SPTPalette customPallet = null;
        private static sbyte colorTolerance = 16;
        private static int upscaleFactor = 1;

        [MTAThread]
        private static int Main(string[] args)
        {
            Console.InputEncoding = Encoding.Unicode;
            Console.OutputEncoding = Encoding.Unicode;

            SPTArgumentParser parser = new(args);

            ExecuteCommands(parser);
            ExecuteProgram(parser);

            return 0;
        }

        private static void ExecuteCommands(SPTArgumentParser parser)
        {
            return;
        }

        private static void ExecuteProgram(SPTArgumentParser parser)
        {
            // FILES
            if (parser.HasOption("input"))
            {
                string inputFilename = parser.GetOption("input");
                string inputFileExtension = Path.GetExtension(inputFilename);

                if (string.IsNullOrEmpty(inputFilename))
                {
                    Console.WriteLine("No input file was specified.");
                    Environment.Exit(1);
                }

                if (!File.Exists(inputFilename))
                {
                    Console.WriteLine("The specified input file does not exist.");
                    Environment.Exit(1);
                }

                if (string.IsNullOrEmpty(inputFileExtension))
                {
                    Console.WriteLine("The input file has no extension, making its type unknown.");
                    Environment.Exit(1);
                }

                if (!SPTPixelizationFileCompatibility.Check(inputFileExtension))
                {
                    Console.WriteLine($"Files with the extension '{inputFileExtension}' are not compatible with the program. The only compatible ones are:{Environment.NewLine}{SPTPixelizationFileCompatibility.GetCompatibleTypesLabels()}");
                    Environment.Exit(1);
                }

                inputFileStream = File.Open(inputFilename, FileMode.Open, FileAccess.Read, FileShare.Read);
            }

            if (parser.HasOption("output"))
            {
                string outputFilename = parser.GetOption("output");
                string outputFileExtension = Path.GetExtension(outputFilename);

                if (string.IsNullOrEmpty(outputFilename))
                {
                    Console.WriteLine("No output file was specified.");
                    Environment.Exit(1);
                }

                if (string.IsNullOrEmpty(outputFileExtension))
                {
                    Console.WriteLine("The output file has no extension, making its type unknown.");
                    Environment.Exit(1);
                }

                if (!SPTPixelizationFileCompatibility.Check(outputFileExtension))
                {
                    Console.WriteLine($"Files with the extension '{outputFileExtension}' are not compatible with the program. The only compatible ones are:{Environment.NewLine}{SPTPixelizationFileCompatibility.GetCompatibleTypesLabels()}");
                    Environment.Exit(1);
                }

                outputFileStream = File.Open(parser.GetOption("output"), FileMode.OpenOrCreate, FileAccess.Write, FileShare.Write);
            }

            // PALLET
            if (parser.HasOption("customPallet"))
            {
                string filename = parser.GetOption("customPallet");
                string fileExtension = Path.GetExtension(filename)?.ToLower();

                if (!File.Exists(filename))
                {
                    Console.WriteLine("The required color palette file does not exist.");
                    Environment.Exit(1);
                }

                if (string.IsNullOrEmpty(fileExtension))
                {
                    Console.WriteLine("The color palette file does not have any extension.");
                    Environment.Exit(1);
                }

                if (!SPTPaletteFileCompatibility.Check(fileExtension))
                {
                    Console.WriteLine($"Files with the extension '{fileExtension}' are not compatible with the program. The only compatible ones are:{Environment.NewLine}{SPTPaletteFileCompatibility.GetCompatibleTypesLabels()}");
                    Environment.Exit(1);
                }

                customPallet = SPTPaletteSerializer.Deserialize(filename);
            }

            // TRANSFORM
            // Determines the intensity of pixelation. Must be a positive integer greater than 0.
            if (parser.HasOption("pixelateFactor"))
            {
                if (int.TryParse(parser.GetOption("pixelateFactor"), out int value))
                {
                    if (value <= 0)
                    {
                        Console.WriteLine("Pixelate factor must be greater than 0.");
                        Environment.Exit(1);
                    }
                }
                else
                {
                    Console.WriteLine("Pixalate Factor value error.");
                }

                pixelateFactor = value;
            }

            // Defines the color variety in the output image. For custom palettes, this is automatically set to the palette's color count. Must be a positive integer greater than 0.
            if (parser.HasOption("paletteSize"))
            {
                if (int.TryParse(parser.GetOption("paletteSize"), out int value))
                {
                    if (value <= 0)
                    {
                        Console.WriteLine("Palette size must be greater than 0.");
                        Environment.Exit(1);
                    }
                }
                else
                {
                    Console.WriteLine("Palette Size value error.");
                    Environment.Exit(1);
                }

                paletteSize = value;
            }

            // Controls the blending and unification of nearby colors during pixelation. Should be an integer between 0 and 255.
            if (parser.HasOption("colorTolerance"))
            {
                if (!sbyte.TryParse(parser.GetOption("colorTolerance"), out sbyte value))
                {
                    Console.WriteLine("The value for color tolerance must be an integer numeric value between 0 and 255.");
                    Environment.Exit(1);
                }

                colorTolerance = value;
            }

            //
            if (parser.HasOption("upscaleFactor"))
            {
                if (int.TryParse(parser.GetOption("upscaleFactor"), out int value))
                {
                    if (value <= 0)
                    {
                        Console.WriteLine("Upscale factor must be greater than 0.");
                        Environment.Exit(1);
                    }
                }
                else
                {
                    Console.WriteLine("Upscale factor value error.");
                    Environment.Exit(1);
                }

                upscaleFactor = value;
            }

            StartPixalator();
        }

        private static void StartPixalator()
        {
            // Pixelator
            using SPTPixelator pixalator = new(inputFileStream, outputFileStream)
            {
                PixelateFactor = pixelateFactor,
                PaletteSize = paletteSize,
                ColorTolerance = colorTolerance,
                CustomPalette = customPallet,
                UpscaleFactor = upscaleFactor,
                // Effects = [.. effects]
            };

            // Infos
            Stopwatch processStopwatch = new();
            processStopwatch.Start();

            // Special
            string line = new('-', Console.WindowWidth - 1);

            // Title
            Console.WriteLine(line);
            SPTTerminal.ApplyColorGradient(new(FiggleFonts.Swampland.Render(SPTProjectConstants.Name)));
            SPTTerminal.BreakLine();
            SPTTerminal.ApplyColor(ConsoleColor.Yellow, $"v{SPTProjectConstants.Version}");
            SPTTerminal.ApplyColor(ConsoleColor.Cyan, $" - {SPTProjectConstants.Name} - (c) Starciad <davilsfernandes.starciad.comu@gmail.com>");
            SPTTerminal.BreakLine();
            Console.WriteLine(line);

            // Process
            SPTTerminal.BreakLine();
            SPTTerminal.ApplyColor(ConsoleColor.Gray, "╔─━━━━━━░★░━━━━━━─╗");
            SPTTerminal.BreakLine();
            SPTTerminal.BreakLine();
            SPTTerminal.ApplyColor(ConsoleColor.Blue, "PROCESSING");
            SPTTerminal.BreakLine();

            DisplayProcessingStep(ConsoleColor.Yellow, "Preparing application.");
            DisplayProcessingStep(ConsoleColor.Yellow, "Starting the pixelization process.");

            pixalator.InitializePixelation();
            pixalator.ExportPixelatedImage();

            SPTTerminal.BreakLine();
            SPTTerminal.ApplyColor(ConsoleColor.Gray, "╚─━━━━━━░★░━━━━━━─╝");
            SPTTerminal.BreakLine();

            // Finish
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

            inputFileStream.Dispose();
            outputFileStream.Dispose();

            // Special
            void DisplayProcessingStep(ConsoleColor color, string step)
            {
                SPTTerminal.ApplyColor(color, "[•] ");
                Console.WriteLine(step);
            }

            void DisplayFinishDetail(ConsoleColor color, string label, string value)
            {
                SPTTerminal.ApplyColor(color, "[•] ");
                SPTTerminal.ApplyColor(ConsoleColor.White, $"{label}: ");
                Console.WriteLine(value);
            }
        }

        // SPT is a utility that allows you to pixelate images, allowing you to apply dozens of different filters and settings.
    }
}