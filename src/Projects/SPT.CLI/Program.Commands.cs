using SPT.CLI.Interactivity;
using SPT.Core.Enums;
using SPT.Core.Palettes;
using SPT.Core.Palettes.Serializers;
using SPT.Core.Pixelization;

using System;
using System.Collections.Generic;
using System.IO;

namespace SPT.CLI
{
    internal static partial class Program
    {
        private static void RegisterCommands()
        {
            commandRegistry.RegisterCommand(new SPTCommand(
                "help",
                "Displays available commands and their descriptions.",
                parser =>
                {
                    commandRegistry.DisplayHelp();
                    Environment.Exit(0);
                },
                "h", "?"
            ));

            commandRegistry.RegisterCommand(new SPTCommand(
                "input",
                "Specifies the input file. Must be a valid file path and compatible with the program's supported formats.",
                parser =>
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
                        Console.WriteLine($"Files with the extension '{inputFileExtension}' are not compatible with the program. Supported formats are: {Environment.NewLine}{SPTPixelizationFileCompatibility.GetCompatibleTypesLabels()}");
                        Environment.Exit(1);
                    }

                    inputFileStream = File.Open(inputFilename, FileMode.Open, FileAccess.Read, FileShare.Read);
                },
                "i"
            ));

            commandRegistry.RegisterCommand(new SPTCommand(
                "output",
                "Specifies the output file. Must include a valid extension compatible with the program's supported formats.",
                parser =>
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
                        Console.WriteLine($"Files with the extension '{outputFileExtension}' are not compatible with the program. Supported formats are: {Environment.NewLine}{SPTPixelizationFileCompatibility.GetCompatibleTypesLabels()}");
                        Environment.Exit(1);
                    }

                    outputFileStream = File.Open(outputFilename, FileMode.OpenOrCreate, FileAccess.Write, FileShare.Write);
                },
                "o"
            ));

            commandRegistry.RegisterCommand(new SPTCommand(
                "customPallet",
                "Specifies a custom color palette file. The file must exist and be in a supported format.",
                parser =>
                {
                    string filename = parser.GetOption("customPallet");
                    string fileExtension = Path.GetExtension(filename)?.ToLower();

                    if (!File.Exists(filename))
                    {
                        Console.WriteLine("The specified color palette file does not exist.");
                        Environment.Exit(1);
                    }

                    if (string.IsNullOrEmpty(fileExtension))
                    {
                        Console.WriteLine("The color palette file does not have any extension.");
                        Environment.Exit(1);
                    }

                    if (!SPTPaletteFileCompatibility.Check(fileExtension))
                    {
                        Console.WriteLine($"Files with the extension '{fileExtension}' are not compatible with the program. Supported formats are: {Environment.NewLine}{SPTPaletteFileCompatibility.GetCompatibleTypesLabels()}");
                        Environment.Exit(1);
                    }

                    customPallet = SPTPaletteSerializer.Deserialize(filename);
                },
                "pallet", "cp"
            ));

            commandRegistry.RegisterCommand(new SPTCommand(
                "pixelateFactor",
                "Determines the intensity of pixelation. Must be a positive integer greater than 0.",
                parser =>
                {
                    if (!uint.TryParse(parser.GetOption("pixelateFactor"), out uint value) || value == 0)
                    {
                        Console.WriteLine("Invalid value for pixelateFactor. It must be a positive integer.");
                        Environment.Exit(1);
                    }

                    pixelateFactor = value;
                },
                "pf", "pix"
            ));

            commandRegistry.RegisterCommand(new SPTCommand(
                "paletteSize",
                "Defines the color variety in the output image. Must be a positive integer greater than 0.",
                parser =>
                {
                    if (!uint.TryParse(parser.GetOption("paletteSize"), out uint value) || value == 0)
                    {
                        Console.WriteLine("Invalid value for paletteSize. It must be a positive integer.");
                        Environment.Exit(1);
                    }

                    paletteSize = value;
                },
                "ps"
            ));

            commandRegistry.RegisterCommand(new SPTCommand(
                "colorTolerance",
                "Controls the blending and unification of nearby colors during pixelation. Must be an integer between 0 and 255.",
                parser =>
                {
                    if (!sbyte.TryParse(parser.GetOption("colorTolerance"), out sbyte value))
                    {
                        Console.WriteLine("Invalid value for colorTolerance. It must be an integer between 0 and 255.");
                        Environment.Exit(1);
                    }

                    colorTolerance = value;
                },
                "ct", "tolerance"
            ));

            commandRegistry.RegisterCommand(new SPTCommand(
                "upscaleFactor",
                "Specifies the upscale factor for the image. Must be a positive integer.",
                parser =>
                {
                    if (!uint.TryParse(parser.GetOption("upscaleFactor"), out uint value) || value == 0)
                    {
                        Console.WriteLine("Invalid value for upscaleFactor. It must be a positive integer.");
                        Environment.Exit(1);
                    }

                    upscaleFactor = value;
                },
                "us", "scale"
            ));

            commandRegistry.RegisterCommand(new SPTCommand(
                "colorSpaceType",
                "Specifies the color space to be used. Available options: RGB, HSL, HSV.",
                parser =>
                {
                    string colorSpace = parser.GetOption("colorSpaceType").ToLower();
                    switch (colorSpace)
                    {
                        case "rgb":
                            colorSpaceType = SPTColorSpaceType.RGB;
                            break;

                        case "hsl":
                            colorSpaceType = SPTColorSpaceType.HSL;
                            break;

                        case "hsv":
                            colorSpaceType = SPTColorSpaceType.HSV;
                            break;

                        default:
                            Console.WriteLine("Invalid color space type. Available options: RGB, HSL, HSV.");
                            Environment.Exit(1);
                            break;
                    }
                },
                "cs", "space"
            ));
        }

        private static void ExecuteCommands(SPTArgumentParser parser)
        {
            foreach (KeyValuePair<string, string> option in parser.GetAllOptions())
            {
                SPTCommand command = commandRegistry.GetCommand(option.Key);
                command?.Execute(parser);
            }
        }
    }
}
