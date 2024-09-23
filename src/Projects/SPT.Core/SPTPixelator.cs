using SkiaSharp;

using SPT.Core.Enums;
using SPT.Core.Palettes;

using System;
using System.IO;

namespace SPT.Core
{
    /// <summary>
    /// Represents a tool for applying pixelation effects to an midia file.
    /// </summary>
    /// <remarks>
    /// This class handles the pixelation process on an input image, providing options for custom palettes, color space, and other configurable parameters.
    /// </remarks>
    /// <param name="inputFileStream">The input file stream to be pixelated.</param>
    /// <param name="outputFileStream">The output file stream where the pixelated image will be saved.</param>
    public sealed partial class SPTPixelator(FileStream inputFileStream, FileStream outputFileStream) : IDisposable
    {
        /// <summary>
        /// Gets or sets the pixelation factor, which determines the level of pixelation applied to the image.
        /// </summary>
        /// <value>
        /// The pixelation factor. A higher value results in larger pixels. Default is 16.
        /// </value>
        public required uint PixelateFactor
        {
            get => this.pixelateFactor;
            set => this.pixelateFactor = value;
        }

        /// <summary>
        /// Gets or sets the size of the palette used for pixelation.
        /// </summary>
        /// <value>
        /// The number of colors in the palette. Default is 8.
        /// </value>
        public uint PaletteSize
        {
            get => this.paletteSize;
            set => this.paletteSize = value;
        }

        /// <summary>
        /// Gets a value indicating whether a custom palette is set.
        /// </summary>
        /// <value>
        /// <c>true</c> if a custom palette is assigned; otherwise, <c>false</c>.
        /// </value>
        public bool HasCustomPalette => this.customPalette != null;

        /// <summary>
        /// Gets or sets the custom palette to be used during pixelation.
        /// </summary>
        /// <value>
        /// The <see cref="SPTPalette"/> object representing the custom color palette.
        /// </value>
        public SPTPalette CustomPalette
        {
            get => this.customPalette;
            set => this.customPalette = value;
        }

        /// <summary>
        /// Gets or sets the tolerance value for color matching during pixelation.
        /// </summary>
        /// <value>
        /// The color tolerance. A smaller value means more strict color matching. Default is 16.
        /// </value>
        public sbyte ColorTolerance
        {
            get => this.colorTolerance;
            set => this.colorTolerance = value;
        }

        /// <summary>
        /// Gets or sets the factor by which the output image will be upscaled.
        /// </summary>
        /// <value>
        /// The upscale factor. Must be greater than or equal to 1. Default is 1.
        /// </value>
        /// <exception cref="ArgumentException">
        /// Thrown if the value is less than 1.
        /// </exception>
        public uint UpscaleFactor
        {
            get => this.upscaleFactor;
            set
            {
                if (value == 0)
                {
                    throw new ArgumentException("The upscale factor must be greater than or equal to 1.", nameof(this.UpscaleFactor));
                }

                this.upscaleFactor = value;
            }
        }

        /// <summary>
        /// Gets or sets the color space type used during pixelation.
        /// </summary>
        /// <value>
        /// The <see cref="SPTColorSpaceType"/> indicating the color space (RGB, HSL, or HSV). Default is RGB.
        /// </value>
        public SPTColorSpaceType ColorSpaceType
        {
            get => this.colorSpaceType;
            set => this.colorSpaceType = value;
        }

        private readonly FileStream inputFileStream = inputFileStream;
        private readonly FileStream outputFileStream = outputFileStream;

        private SKBitmap bitmapInput;
        private SKBitmap bitmapOutput;

        private uint widthInput;
        private uint widthOutput;
        private uint heightInput;
        private uint heightOutput;

        private bool disposedValue;

        private uint pixelateFactor = 16;
        private uint paletteSize = 8;
        private SPTPalette customPalette = null;
        private sbyte colorTolerance = 16;
        private uint upscaleFactor = 1;
        private SPTColorSpaceType colorSpaceType = SPTColorSpaceType.RGB;

        private SKColor[] bitmapOutputColors = [];

        /// <summary>
        /// Initializes the pixelation process on the input file, creating a pixelated version of the file.
        /// </summary>
        public void InitializePixelation()
        {
            StartPixelationSystem();
            StartPixelationProcessRoutine();
        }

        /// <summary>
        /// Releases the resources used by the <see cref="SPTPixelator"/> class.
        /// </summary>
        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (!this.disposedValue)
            {
                if (disposing)
                {
                    ((IDisposable)this.bitmapInput).Dispose();
                    ((IDisposable)this.bitmapOutput).Dispose();

                    this.bitmapInput = null;
                    this.bitmapOutput = null;
                }

                this.disposedValue = true;
            }
        }
    }
}
