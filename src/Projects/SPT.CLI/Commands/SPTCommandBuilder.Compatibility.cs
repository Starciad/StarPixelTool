using SPT.Core.IO.Palettes;
using SPT.Core.IO.Pixelization;
using SPT.Terminal;

using System;
using System.CommandLine;

namespace SPT.Commands
{
    internal static partial class SPTCommandBuilder
    {
        private static void InitializeCompatibilityCommand(RootCommand root)
        {
            // ================================ //
            // Options
            Option<bool> showSupportedFileFormatsOption = new(name: "--show-file-formats", description: "Display supported file formats.");
            Option<bool> showSupportedPaletteFormatsOption = new(name: "--show-palette-formats", description: "Display supported color palette formats.");

            showSupportedFileFormatsOption.AddAlias("-sff");
            showSupportedPaletteFormatsOption.AddAlias("-spf");

            // ================================ //
            // Commands
            Command compatibilityCommand = new("compatibility", "View relevant information about compatibility with SPT-specific elements.")
            {
                showSupportedFileFormatsOption,
                showSupportedPaletteFormatsOption,
            };

            compatibilityCommand.SetHandler(Handler, showSupportedFileFormatsOption, showSupportedPaletteFormatsOption);
            root.AddCommand(compatibilityCommand);

            // ================================ //
            // Methods
            // Handlers
            void Handler(bool showSupportedFileFormats, bool showSupportedPaletteFormats)
            {
                if (showSupportedFileFormats)
                {
                    SPTTerminal.BreakLine();
                    SPTTerminal.ApplyColor(ConsoleColor.Blue, "[ Showing all supported file formats. ]");
                    SPTTerminal.BreakLine();
                    Console.WriteLine(SPTPixelizationFileCompatibility.GetCompatibleTypesLabels());
                }

                if (showSupportedPaletteFormats)
                {
                    SPTTerminal.BreakLine();
                    SPTTerminal.ApplyColor(ConsoleColor.Blue, "[ Showing all supported color palette formats. ]");
                    SPTTerminal.BreakLine();
                    Console.WriteLine(SPTPaletteFileCompatibility.GetCompatibleTypesLabels());
                }

                SPTTerminal.BreakLine();
            }
        }
    }
}