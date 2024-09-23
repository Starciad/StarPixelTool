using SkiaSharp;

using SPT.Core.Enums;
using SPT.Core.Palettes;

using System;
using System.IO;

namespace SPT.Core
{
    /// <summary>
    ///
    /// </summary>
    /// <remarks>
    ///
    /// </remarks>
    /// <param name="inputFileStream"></param>
    /// <param name="outputFileStream"></param>
    public sealed partial class SPTPixelator(FileStream inputFileStream, FileStream outputFileStream) : IDisposable
    {
        /// <summary>
        ///
        /// </summary>
        public required uint PixelateFactor
        {
            get => this.pixelateFactor;
            set => this.pixelateFactor = value;
        }

        /// <summary>
        ///
        /// </summary>
        public uint PaletteSize
        {
            get => this.paletteSize;
            set => this.paletteSize = value;
        }

        /// <summary>
        ///
        /// </summary>
        public bool HasCustomPalette => this.customPalette != null;

        /// <summary>
        ///
        /// </summary>
        public SPTPalette CustomPalette
        {
            get => this.customPalette;
            set => this.customPalette = value;
        }

        /// <summary>
        ///
        /// </summary>
        public sbyte ColorTolerance
        {
            get => this.colorTolerance;
            set => this.colorTolerance = value;
        }

        /// <summary>
        ///
        /// </summary>
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
        /// 
        /// </summary>
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
        /// Initializes the pixelation process on the input image, creating a pixelated version.
        /// </summary>
        public void InitializePixelation()
        {
            StartPixelationSystem();
            StartPixelationProcessRoutine();
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

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
