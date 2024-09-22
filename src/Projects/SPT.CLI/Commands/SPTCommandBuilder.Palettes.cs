using SPT.IO;

using System;
using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.Parsing;
using System.IO;
using System.Linq;

namespace SPT.Commands
{
    internal static partial class SPTCommandBuilder
    {
        private static void InitializePalettesCommand(RootCommand root)
        {
            // ================================ //
            // Options
            Option<string> definePaletteFileOption = new(name: "--define", description: "Specify the path (relative or complete) to a file containing the palette settings that will be used in the pixalization process.");
            Option<bool> removeDefinedPaletteOption = new(name: "--remove", description: "Removes the currently defined palette.");
            Option<bool> showAllPalettesOption = new(name: "--showAll", description: "Show all available palettes.");

            definePaletteFileOption.AddAlias("-d");
            removeDefinedPaletteOption.AddAlias("-r");
            showAllPalettesOption.AddAlias("-sa");

            definePaletteFileOption.AddValidator(DefinePaletteFileValidator);

            // ================================ //
            // Commands
            Command command = new("palettes", "Configures and checks information related to system color palettes.")
            {
                definePaletteFileOption,
                showAllPalettesOption,
                removeDefinedPaletteOption,
            };

            command.SetHandler(Handler, definePaletteFileOption, showAllPalettesOption, removeDefinedPaletteOption);
            root.AddCommand(command);

            // ================================ //
            // Methods
            // Handlers
            void Handler(string definedPalette, bool showAllPalettes, bool removePalette)
            {
                SPTPalettesSettings palettesSettings = new();

                if (removePalette)
                {
                    palettesSettings.DefinedPalette = string.Empty;
                }
                else if (!string.IsNullOrWhiteSpace(definedPalette))
                {
                    palettesSettings.DefinedPalette = definedPalette;
                }

                if (showAllPalettes)
                {
                    List<string> palettesFiles = [];

                    foreach (string extension in SPTPaletteFileCompatibility.Extensions)
                    {
                        palettesFiles.AddRange(Directory.EnumerateFiles(SPTDirectory.PalettesDirectory, string.Concat('*', extension), SearchOption.AllDirectories));
                    }

                    SPTTerminal.BreakLine();
                    SPTTerminal.ApplyColor(ConsoleColor.Green, "[ Showing all color palettes installed on the device. ]");
                    Console.WriteLine(Environment.NewLine);
                    if (palettesFiles.Count == 0)
                    {
                        SPTTerminal.ApplyColor(ConsoleColor.Red, "Unable to find any color palette file.");
                        SPTTerminal.BreakLine();
                        SPTTerminal.ApplyColor(ConsoleColor.Cyan, string.Concat("Directory searched: ", SPTDirectory.PalettesDirectory));
                        SPTTerminal.BreakLine();
                    }
                    else if (palettesFiles.Count == 1)
                    {
                        WriteLineFilePalette(palettesFiles[0]);
                    }
                    else
                    {
                        foreach (string file in palettesFiles)
                        {
                            WriteLineFilePalette(file);
                        }
                    }

                    SPTTerminal.BreakLine();
                }

                SPTSettingsManager.CreatePalettesSettings(new SPTPalettesSettings()
                {
                    DefinedPalette = definedPalette
                });
            }

            // Validators
            void DefinePaletteFileValidator(OptionResult result)
            {
                if (result.Tokens.Count == 0)
                {
                    return;
                }
                else
                {
                    string filename = Path.Combine(SPTDirectory.PalettesDirectory, result.Tokens.Single().Value);

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
                        else if (!SPTPaletteFileCompatibility.Check(extension))
                        {
                            result.ErrorMessage = $"Files with the extension '{extension}' are not compatible with the program. The only compatible ones are:{Environment.NewLine}{SPTPaletteFileCompatibility.GetCompatibleTypesLabels()}";
                            return;
                        }
                    }
                }
            }

            // Helpers
            void WriteLineFilePalette(string paletteFilename)
            {
                SPTTerminal.ApplyColor(ConsoleColor.Magenta, "[•] ");
                Console.Write(string.Concat(Path.GetFileName(paletteFilename), " "));
                SPTTerminal.ApplyColor(ConsoleColor.DarkYellow, string.Concat('(', paletteFilename, ')'));
                SPTTerminal.BreakLine();
            }
        }
    }
}