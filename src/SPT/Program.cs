﻿using System;
using System.CommandLine;
using System.IO;

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

            Option<int> toleranceOption = new(
                name: "--tolerance",
                description: "Tolerance level for color blending. Reduces color variation in the output. (0 to 255)"
            );
            toleranceOption.AddAlias("-t");

            // =================================== //

            RootCommand rootCommand = new("Transform images into Pixel Art with various filters and effects.")
            {
                inputFileOption,
                outputFileOption,
                pixelScaleOption,
                toleranceOption,
            };

            rootCommand.SetHandler((FileInfo input, FileInfo output, int pixelScale, int tolerance) =>
            {
                using PixelArtConverter converter = new(input, output, pixelScale, tolerance);

                converter.Start();
                converter.Export();

            }, inputFileOption, outputFileOption, pixelScaleOption, toleranceOption);

            return rootCommand.Invoke(args);
        }
    }
}