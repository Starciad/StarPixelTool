using System;
using System.CommandLine;
using System.IO;

using SPT.Core;

namespace SPT
{
    internal static class Program
    {
        [MTAThread]
        private static int Main(string[] args)
        {
            Option<FileInfo> inputFileOption = new(
                name: "--input",
                description: "Input image file for Pixel Art transformation."
            );
            inputFileOption.AddAlias("-i");
            inputFileOption.IsRequired = true;

            Option<FileInfo> outputFileOption = new(
                name: "--output",
                description: "Output file for the transformed Pixel Art image."
            );
            outputFileOption.AddAlias("-o");
            outputFileOption.IsRequired = true;

            Option<int> pixelScaleOption = new(
                name: "--pixelScale",
                description: "Scale factor for the Pixel Art transformation. (>= 1)",
                getDefaultValue: () => 1
            );
            pixelScaleOption.AddAlias("-s");

            // =================================== //

            RootCommand rootCommand = new("Transform images into Pixel Art with various filters and effects.")
            {
                inputFileOption,
                outputFileOption,
                pixelScaleOption,
            };

            rootCommand.SetHandler((FileInfo input, FileInfo output, int pixelScale) =>
            {
                using SPTPixelator pixalator = new(input.Open(FileMode.Open, FileAccess.Read), output.Open(FileMode.OpenOrCreate, FileAccess.ReadWrite), pixelScale);

                pixalator.Start();
                pixalator.Save();

            }, inputFileOption, outputFileOption, pixelScaleOption);

            return rootCommand.Invoke(args);
        }
    }
}