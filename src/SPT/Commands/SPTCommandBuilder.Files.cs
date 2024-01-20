using MessagePack;

using SPT.Constants;
using SPT.Core.IO.Palettes;
using SPT.Core.IO.Pixelization;
using SPT.IO;
using SPT.Managers;
using SPT.Models;
using SPT.Terminal;

using System;
using System.CommandLine;
using System.CommandLine.Parsing;
using System.IO;
using System.Linq;

namespace SPT.Commands
{
    internal static partial class SPTCommandBuilder
    {
        private static void InitializeFilesCommand(RootCommand root)
        {
            // ================================ //
            // Options
            Option<string> inputFilenameOption = new(name: "--input", description: "Specifies the source image file to undergo the pixelation process.");
            Option<string> outputFilenameOption = new(name: "--output", description: "Sets the destination file for the resulting pixelated image.");

            inputFilenameOption.IsRequired = true;
            outputFilenameOption.IsRequired = true;

            inputFilenameOption.AddAlias("-i");
            outputFilenameOption.AddAlias("-o");

            inputFilenameOption.AddValidator(InputFilenameValidator);
            outputFilenameOption.AddValidator(OutputFilenameValidator);

            // ================================ //
            // Commands
            Command command = new("files", "Specifies file-related settings for the pixelization process.")
            {
                inputFilenameOption,
                outputFilenameOption,
            };

            command.SetHandler(Handler, inputFilenameOption, outputFilenameOption);
            root.AddCommand(command);

            // ================================ //
            // Methods
            // Handlers
            void Handler(string inputFilename, string outputFilename)
            {
                if (string.IsNullOrWhiteSpace(inputFilename) || string.IsNullOrWhiteSpace(outputFilename))
                {
                    throw new ArgumentException("Input or output filename cannot be null or a blank space.",
                        nameof(inputFilename) + ", " + nameof(outputFilename));
                }
                if (!File.Exists(inputFilename))
                {
                    throw new FileNotFoundException("The input file cannot be found.", inputFilename);
                }

                SPTSettingsManager.CreateFileSettings(new SPTFileSettings()
                {
                    InputFilename = inputFilename,
                    OutputFilename = outputFilename
                });
            }

            // Validators
            void InputFilenameValidator(OptionResult result)
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
                        string extension = Path.GetExtension(filename).ToLower();

                        if (string.IsNullOrEmpty(extension))
                        {
                            result.ErrorMessage = "The input file has no extension, making its type unknown.";
                            return;
                        }
                        else if (!SPTPixelizationFileCompatibility.Check(extension))
                        {
                            result.ErrorMessage = $"Files with the extension '{extension}' are not compatible with the program. The only compatible ones are:{Environment.NewLine}{SPTPixelizationFileCompatibility.GetCompatibleTypesLabels()}";
                            return;
                        }
                    }
                }
            }
            void OutputFilenameValidator(OptionResult result)
            {
                if (result.Tokens.Count == 0)
                {
                    result.ErrorMessage = "No output file was specified.";
                    return;
                }

                string filename = result.Tokens.Single().Value;
                string extension = Path.GetExtension(filename).ToLower();

                if (string.IsNullOrEmpty(extension))
                {
                    result.ErrorMessage = "The output file extension has not been set.";
                    return;
                }
                else if (!SPTPixelizationFileCompatibility.Check(extension))
                {
                    result.ErrorMessage = $"The output file you defined with the extension '{extension}' is not compatible with the program. The only compatible ones are:{Environment.NewLine}{SPTPixelizationFileCompatibility.GetCompatibleTypesLabels()}.";
                    return;
                }
                else if (!extension.Equals(Path.GetExtension(result.GetValueForOption(inputFilenameOption))?.ToLower()))
                {
                    result.ErrorMessage = "The output file extension must be the same as the input file extension.";
                    return;
                }
            }
        }
    }
}
