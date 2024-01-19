using SkiaSharp;

using SPT.Core.Colors;
using SPT.Core.Extensions;
using SPT.Core.Palette;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SPT.Core
{
    /// <summary>
    ///
    /// </summary>
    /// <remarks>
    ///
    /// </remarks>
    /// <param name="inputFile"></param>
    /// <param name="outputFile"></param>
    public sealed class SPTPixelator(FileStream inputFile, FileStream outputFile) : IDisposable
    {
        /// <summary>
        ///
        /// </summary>
        public required int PixelateFactor
        {
            get => this.pixelateFactor;
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(value), "Pixelate factor must be greater than 0.");
                }

                this.pixelateFactor = value;
            }
        }

        /// <summary>
        ///
        /// </summary>
        public int PaletteSize
        {
            get => this.paletteSize;
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(value), "Palette size must be greater than 0.");
                }

                this.paletteSize = value;
            }
        }

        /// <summary>
        ///
        /// </summary>
        public SPTPalette CustomPalette
        {
            get => this.customPalette;
            set
            {
                if (value.IsEmpty)
                {
                    throw new Exception("The defined custom color palette does not contain any colors.");
                }

                this.customPalette = value;
            }
        }

        private SKBitmap bitmapInput;
        private SKBitmap bitmapOutput;

        private int widthInput;
        private int widthOutput;
        private int heightInput;
        private int heightOutput;

        private bool disposedValue;

        private int pixelateFactor;
        private int paletteSize;
        private SPTPalette customPalette;
        private readonly double colorTolerance;

        private SKColor[] bitmapOutputColors;

        #region System
        /// <summary>
        /// Initializes the pixelation process on the input image, creating a pixelated version.
        /// </summary>
        public void InitializePixelation()
        {
            InitializePixelator();
            StartPixelationProcess();
        }

        /// <summary>
        /// Exports the pixelated image to the specified output file.
        /// </summary>
        /// <exception cref="IOException">Thrown if the export operation fails. Check the output file path and try again.</exception>
        public void ExportPixelatedImage()
        {
            if (!this.bitmapOutput.Encode(outputFile, SKEncodedImageFormat.Png, default))
            {
                throw new IOException("Failed to export the pixelated image. Check the output file path and try again.");
            }
        }
        #endregion

        #region Processing
        private void InitializePixelator()
        {
            // Files
            this.bitmapInput = SKBitmap.Decode(inputFile);
            this.widthInput = this.bitmapInput.Width;
            this.heightInput = this.bitmapInput.Height;

            this.widthOutput = this.widthInput / this.pixelateFactor;
            this.heightOutput = this.heightInput / this.pixelateFactor;
            this.bitmapOutput = new SKBitmap(this.widthOutput, this.heightOutput);

            // Settings
            if (this.customPalette != null || !this.customPalette.IsEmpty)
            {
                this.paletteSize = this.customPalette.Size;
            }
        }
        private void StartPixelationProcess()
        {
            ApplyPixelation();
            ApplyColorReduction();
            ApplyCustomPalette();
        }

        private void ApplyPixelation()
        {
            List<SKColor> colors = [];

            int inputPosX = 0;
            int inputPosY = 0;

            for (int y = 0; y < this.heightOutput; y++)
            {
                for (int x = 0; x < this.widthOutput; x++)
                {
                    SKColor color = this.bitmapInput.GetAverageColorInArea(inputPosX, inputPosY, 3);

                    this.bitmapOutput.SetPixel(x, y, color);
                    colors.Add(color);

                    inputPosX += this.pixelateFactor;
                }

                inputPosX = 0;
                inputPosY += this.pixelateFactor;
            }

            this.bitmapOutputColors = [.. colors.Distinct()];
        }
        private void ApplyColorReduction()
        {
            if (this.paletteSize <= 0)
            {
                throw new InvalidOperationException("Palette size must be greater than 0.");
            }

            List<SKColor> reducedColors = [];
            for (int i = 1; i < this.bitmapOutputColors.Length && reducedColors.Count < this.paletteSize; i++)
            {
                SKColor currentColor = this.bitmapOutputColors[i];
                bool isSimilarColor = false;

                foreach (SKColor paletteColor in reducedColors)
                {
                    if (SPTColorUtility.Difference(currentColor, paletteColor) < this.colorTolerance)
                    {
                        isSimilarColor = true;
                        break;
                    }
                }

                if (!isSimilarColor)
                {
                    reducedColors.Add(currentColor);
                }
            }

            // Reduce the number of colors present in the bitmap using the new, temporary palette.
            SPTPalette reducedPalette = new([.. reducedColors]);
            for (int y = 0; y < this.heightOutput; y++)
            {
                for (int x = 0; x < this.widthOutput; x++)
                {
                    this.bitmapOutput.SetPixel(x, y, reducedPalette.GetClosestColor(this.bitmapOutput.GetPixel(x, y)));
                }
            }
        }
        private void ApplyCustomPalette()
        {
            if (this.customPalette == null || this.customPalette.IsEmpty)
            {
                return;
            }

            for (int y = 0; y < this.heightOutput; y++)
            {
                for (int x = 0; x < this.widthOutput; x++)
                {
                    this.bitmapOutput.SetPixel(x, y, this.customPalette.GetClosestColor(this.bitmapOutput.GetPixel(x, y)));
                }
            }
        }
        #endregion

        #region Dispose
        private void Dispose(bool disposing)
        {
            if (!this.disposedValue)
            {
                if (disposing)
                {
                    ((IDisposable)this.bitmapInput).Dispose();
                    ((IDisposable)this.bitmapOutput).Dispose();

                    inputFile.Close();
                    outputFile.Close();
                }

                this.disposedValue = true;
            }
        }
        void IDisposable.Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
