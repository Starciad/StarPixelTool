using Figgle;

using SPT.Core;
using SPT.Core.Constants;
using SPT.Core.Palettes;
using SPT.Core.Palettes.Serializers;
using SPT.IO;
using SPT.Managers;
using SPT.Models;
using SPT.Terminal;

using System;
using System.CommandLine;
using System.CommandLine.Parsing;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace SPT.Commands
{
    internal static partial class SPTCommandBuilder
    {
        private static void InitializeTransformCommand(RootCommand root)
        {
            // ================================ //
            // Options
            Option<int> pixelateFactorOption = new(name: "--pixelateFactor", description: "Determines the intensity of pixelation. Must be a positive integer greater than 0.");
            Option<int> paletteSizeOption = new(name: "--paletteSize", description: "Defines the color variety in the output image. For custom palettes, this is automatically set to the palette's color count. Must be a positive integer greater than 0.");
            Option<int> colorToleranceOption = new(name: "--tolerance", description: "Controls the blending and unification of nearby colors during pixelation. Should be an integer between 0 and 255.");

            pixelateFactorOption.SetDefaultValue(16);
            paletteSizeOption.SetDefaultValue(8);
            colorToleranceOption.SetDefaultValue(1);

            pixelateFactorOption.AddAlias("-f");
            colorToleranceOption.AddAlias("-t");
            paletteSizeOption.AddAlias("-ps");

            pixelateFactorOption.AddValidator(PixelateFactorValidator);
            colorToleranceOption.AddValidator(ColorToleranceValidator);
            paletteSizeOption.AddValidator(PaletteSizeValidator);

            // ================================ //
            // Commands
            Command command = new("transform", "Transform images by applying various filters and pixelation effects.")
            {
                pixelateFactorOption,
                paletteSizeOption,
                colorToleranceOption,
            };

            command.SetHandler(Handler, pixelateFactorOption, paletteSizeOption, colorToleranceOption);
            root.AddCommand(command);

            // ================================ //
            // Methods
            // Handlers
            void Handler(int pixelateFactor, int paletteSize, int colorTolerance)
            {
                // Settings
                SPTFileSettings fileSettings = SPTSettingsManager.GetFileSettings();
                SPTPalettesSettings palettesSettings = SPTSettingsManager.GetPalettesSettings();

                using FileStream inputFs = File.Open(fileSettings.InputFilename, FileMode.Open, FileAccess.Read);
                using FileStream outputFs = File.Open(fileSettings.OutputFilename, FileMode.OpenOrCreate, FileAccess.ReadWrite);

                // Palettes
                SPTPalette customPalette = null;
                if (!string.IsNullOrWhiteSpace(palettesSettings.DefinedPalette))
                {
                    customPalette = SPTPaletteSerializer.Deserialize(Path.Combine(SPTDirectory.PalettesDirectory, palettesSettings.DefinedPalette));
                }

                // Pixelator
                using SPTPixelator pixalator = new(inputFs, outputFs)
                {
                    PixelateFactor = pixelateFactor,
                    PaletteSize = paletteSize,
                    ColorTolerance = colorTolerance,
                    CustomPalette = customPalette,
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

                DisplayFinishDetail(ConsoleColor.Green, "Runtime", $"{processStopwatch.Elapsed.TotalSeconds}s");
                DisplayFinishDetail(ConsoleColor.Green, "Input filename", fileSettings.InputFilename);
                DisplayFinishDetail(ConsoleColor.Green, "Output filename", fileSettings.OutputFilename);

                SPTTerminal.BreakLine();
                SPTTerminal.ApplyColor(ConsoleColor.Gray, "╚─━━━━━━░★░━━━━━━─╝");
                SPTTerminal.BreakLine();
                SPTTerminal.BreakLine();
            }

            // Validators
            void PixelateFactorValidator(OptionResult result)
            {
                if (result.Tokens.Count == 0)
                {
                    return;
                }

                if (int.Parse(result.Tokens.Single().Value) <= 0)
                {
                    result.ErrorMessage = "Pixelate factor must be greater than 0.";
                    return;
                }
            }

            void ColorToleranceValidator(OptionResult result)
            {
                if (result.Tokens.Count == 0)
                {
                    return;
                }

                if (int.Parse(result.Tokens.Single().Value) <= 0)
                {
                    result.ErrorMessage = "The value for color tolerance must be an integer numeric value between 0 and 255.";
                    return;
                }
            }

            void PaletteSizeValidator(OptionResult result)
            {
                if (result.Tokens.Count == 0)
                {
                    return;
                }

                if (int.Parse(result.Tokens.Single().Value) <= 0)
                {
                    result.ErrorMessage = "Palette size must be greater than 0.";
                    return;
                }
            }

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
    }
}
