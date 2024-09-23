using SkiaSharp;

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
    public sealed partial class SPTPixelator : IDisposable
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

        private readonly FileStream inputFileStream;
        private readonly FileStream outputFileStream;

        private SKBitmap bitmapInput;
        private SKBitmap bitmapOutput;

        private readonly uint widthInput;
        private readonly uint widthOutput;
        private readonly uint heightInput;
        private readonly uint heightOutput;

        private bool disposedValue;

        private uint pixelateFactor = 16;
        private uint paletteSize = 8;
        private SPTPalette customPalette;
        private sbyte colorTolerance = 16;
        private uint upscaleFactor = 1;

        private SKColor[] bitmapOutputColors = [];

        public SPTPixelator(FileStream inputFileStream, FileStream outputFileStream)
        {
            // Streams
            this.inputFileStream = inputFileStream;
            this.outputFileStream = outputFileStream;

            // Files
            this.bitmapInput = SKBitmap.Decode(this.inputFileStream);
            this.widthInput = (uint)this.bitmapInput.Width;
            this.heightInput = (uint)this.bitmapInput.Height;

            this.widthOutput = this.widthInput / this.pixelateFactor;
            this.heightOutput = this.heightInput / this.pixelateFactor;
            this.bitmapOutput = new SKBitmap((int)this.widthOutput, (int)this.heightOutput);

            // Settings
            if (this.HasCustomPalette)
            {
                this.paletteSize = this.customPalette.Size;
            }
        }

        /// <summary>
        /// Initializes the pixelation process on the input image, creating a pixelated version.
        /// </summary>
        public void InitializePixelation()
        {
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
