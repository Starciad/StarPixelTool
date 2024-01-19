using Figgle;

using SPT.Core;
using SPT.Core.Constants;
using SPT.Core.IO.Pixelization;
using SPT.Core.Palettes.Serializers;
using SPT.Terminal;

using System;
using System.CommandLine;
using System.CommandLine.Parsing;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace SPT.Commands
{
    internal static class SPTCommandBuilder
    {
        internal static void Initialize(RootCommand root)
        {
            Option<string> inputFilenameOption = new(name: "--input", description: "Specifies the source image file to undergo the pixelation process.");
            Option<string> outputFilenameOption = new(name: "--output", description: "Sets the destination file for the resulting pixelated image.");
            Option<int> pixelateFactorOption = new(name: "--pixelateFactor", description: "Determines the intensity of pixelation. Must be a positive integer greater than 0.");
            Option<int> paletteSizeOption = new(name: "--paletteSize", description: "Defines the color variety in the output image. For custom palettes, this is automatically set to the palette's color count. Must be a positive integer greater than 0.");
            Option<int> colorToleranceOption = new(name: "--tolerance", description: "Controls the blending and unification of nearby colors during pixelation. Should be an integer between 0 and 255.");
            Option<string> paletteFilenameOption = new(name: "--palette", description: "Specifies the color palette for pixelation. By default, the path is relative to the 'Palettes' directory. Currently supports gpl (GIMP Palette) format.");

            inputFilenameOption.IsRequired = true;
            outputFilenameOption.IsRequired = true;

            pixelateFactorOption.SetDefaultValue(1);
            paletteSizeOption.SetDefaultValue(8);
            colorToleranceOption.SetDefaultValue(1);

            inputFilenameOption.AddAlias("-i");
            outputFilenameOption.AddAlias("-o");
            pixelateFactorOption.AddAlias("-pf");
            paletteSizeOption.AddAlias("-ps");
            colorToleranceOption.AddAlias("-t");
            paletteFilenameOption.AddAlias("-p");

            inputFilenameOption.AddValidator((OptionResult result) =>
            {
                if (result.Tokens.Count == 0)
                {
                    result.ErrorMessage = "No input file was specified.";
                    return;
                }
                else
                {
                    string filename = result.Tokens.Single().Value;

                    if (!File.Exists(filename))
                    {
                        result.ErrorMessage = "The specified input file does not exist.";
                        return;
                    }
                    else
                    {
                        string extension = Path.GetExtension(filename)?.ToLower();

                        if (string.IsNullOrEmpty(extension))
                        {
                            result.ErrorMessage = "The input file has no extension, making its type unknown.";
                            return;
                        }
                        else if (!SPTFilePixelizationCompatibility.Check(filename))
                        {
                            result.ErrorMessage = $"Files with the extension '{extension}' are not compatible with the program. The only compatible ones are:{Environment.NewLine}{SPTFilePixelizationCompatibility.GetCompatibleTypesLabels()}";
                            return;
                        }
                    }
                }
            });
            outputFilenameOption.AddValidator((OptionResult result) =>
            {
                if (result.Tokens.Count == 0)
                {
                    result.ErrorMessage = "No output file was specified.";
                    return;
                }

                string filename = result.Tokens.Single().Value;
                string extension = Path.GetExtension(filename)?.ToLower();

                if (string.IsNullOrEmpty(extension))
                {
                    result.ErrorMessage = "The output file extension has not been set.";
                    return;
                }
                else if (!SPTFilePixelizationCompatibility.Check(filename))
                {
                    result.ErrorMessage = $"The output file you defined with the extension '{extension}' is not compatible with the program. The only compatible ones are:{Environment.NewLine}{SPTFilePixelizationCompatibility.GetCompatibleTypesLabels()}.";
                    return;
                }
                else if (!extension.Equals(Path.GetExtension(result.GetValueForOption(inputFilenameOption))?.ToLower()))
                {
                    result.ErrorMessage = "The output file extension must be the same as the input file extension.";
                    return;
                }
            });
            pixelateFactorOption.AddValidator((OptionResult result) =>
            {
                if (result.Tokens.Count == 0)
                {
                    result.ErrorMessage = "Pixelate factor did not have a specified value.";
                    return;
                }

                if (int.Parse(result.Tokens.Single().Value) <= 0)
                {
                    result.ErrorMessage = "Pixelate factor must be greater than 0.";
                    return;
                }
            });
            paletteSizeOption.AddValidator((OptionResult result) =>
            {
                if (result.Tokens.Count == 0)
                {
                    result.ErrorMessage = "The value for the color palette size was not specified.";
                    return;
                }

                if (int.Parse(result.Tokens.Single().Value) <= 0)
                {
                    result.ErrorMessage = "Palette size must be greater than 0.";
                    return;
                }
            });
            colorToleranceOption.AddValidator((OptionResult result) =>
            {
                if (result.Tokens.Count == 0)
                {
                    result.ErrorMessage = "The value for color tolerance was not specified.";
                    return;
                }

                if (int.Parse(result.Tokens.Single().Value) <= 0)
                {
                    result.ErrorMessage = "The value for color tolerance must be an integer numeric value between 0 and 255.";
                    return;
                }
            });
            paletteFilenameOption.AddValidator((OptionResult result) =>
            {
                if (result.Tokens.Count == 0)
                {
                    result.ErrorMessage = "No color palette was specified.";
                    return;
                }
                else
                {
                    string filename = Path.Combine(Directory.GetCurrentDirectory(), "Palettes", result.Tokens.Single().Value);

                    if (!File.Exists(filename))
                    {
                        result.ErrorMessage = "The required color palette file does not exist.";
                        return;
                    }
                    else
                    {
                        string extension = Path.GetExtension(filename)?.ToLower();

                        if (string.IsNullOrEmpty(extension))
                        {
                            result.ErrorMessage = "The color palette file does not have any extension.";
                            return;
                        }
                        else if (!extension.Equals(".gpl", StringComparison.CurrentCultureIgnoreCase))
                        {
                            result.ErrorMessage = $"Files with the extension '{extension}' are not compatible with the program. Just GIMP palette (.gpl) file.";
                            return;
                        }
                    }
                }
            });

            root.AddOption(inputFilenameOption);
            root.AddOption(outputFilenameOption);
            root.AddOption(pixelateFactorOption);
            root.AddOption(paletteSizeOption);
            root.AddOption(colorToleranceOption);
            root.AddOption(paletteFilenameOption);

            root.SetHandler(Handler, inputFilenameOption, outputFilenameOption, pixelateFactorOption, paletteSizeOption, colorToleranceOption, paletteFilenameOption);
        }

        internal static void Handler(string inputFilename, string outputFilename, int pixelateFactor, int paletteSize, int colorTolerance, string paletteFilename)
        {
            using SPTPixelator pixalator = new(File.Open(inputFilename, FileMode.Open, FileAccess.Read), File.Open(outputFilename, FileMode.OpenOrCreate, FileAccess.ReadWrite))
            {
                PixelateFactor = pixelateFactor,
                PaletteSize = paletteSize,
                ColorTolerance = colorTolerance,
                CustomPalette = GPLSerializer.Deserialize(Path.Combine(Directory.GetCurrentDirectory(), "Palettes", paletteFilename)),
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

            DisplayProcessingStep("Preparing application.");
            DisplayProcessingStep("Starting the pixelization process.");

            #region [ PIXALATOR ]
            pixalator.InitializePixelation();
            pixalator.ExportPixelatedImage();
            #endregion

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

            DisplayFinishDetail("Runtime", $"{processStopwatch.Elapsed.TotalSeconds}s");
            DisplayFinishDetail("Input filename", inputFilename);
            DisplayFinishDetail("Output filename", outputFilename);

            SPTTerminal.BreakLine();
            SPTTerminal.ApplyColor(ConsoleColor.Gray, "╚─━━━━━━░★░━━━━━━─╝");
            SPTTerminal.BreakLine();
            SPTTerminal.BreakLine();
        }

        private static void DisplayProcessingStep(string step)
        {
            SPTTerminal.ApplyColor(ConsoleColor.Green, "[•] ");
            Console.WriteLine(step);
        }

        private static void DisplayFinishDetail(string label, string value)
        {
            SPTTerminal.ApplyColor(ConsoleColor.Green, "[•] ");
            SPTTerminal.ApplyColor(ConsoleColor.White, $"{label}: ");
            Console.WriteLine(value);
        }
    }
}