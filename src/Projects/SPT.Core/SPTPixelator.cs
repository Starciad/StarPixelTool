using SkiaSharp;

using SPT.Core.Colors;
using SPT.Core.Effects;
using SPT.Core.Extensions;
using SPT.Core.Palettes;

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
        public float UpscaleFactor
        {
            get => this.upscaleFactor;
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentException("The upscale factor must be greater than or equal to 1.", nameof(this.UpscaleFactor));
                }

                this.upscaleFactor = value;
            }
        }

        /// <summary>
        ///
        /// </summary>
        public SPTEffect[] Effects
        {
            get => this.effects;
            set => this.effects = value;
        }

        private SKBitmap bitmapInput;
        private SKBitmap bitmapOutput;

        private int widthInput;
        private int widthOutput;
        private int heightInput;
        private int heightOutput;

        private bool disposedValue;

        private int pixelateFactor = 16;
        private int paletteSize = 8;
        private SPTPalette customPalette;
        private sbyte colorTolerance = 16;
        private float upscaleFactor = 1;
        private SPTEffect[] effects = [];

        private SKColor[] bitmapOutputColors = [];

        private readonly Random random = new();

        #region System
        /// <summary>
        /// Initializes the pixelation process on the input image, creating a pixelated version.
        /// </summary>
        public void InitializePixelation()
        {
            StartPixelatorSystem();
            StartPixelationProcess();
        }
        #endregion

        #region Processing
        private void StartPixelatorSystem()
        {
            // Files
            this.bitmapInput = SKBitmap.Decode(inputFile);
            this.widthInput = this.bitmapInput.Width;
            this.heightInput = this.bitmapInput.Height;

            this.widthOutput = this.widthInput / this.pixelateFactor;
            this.heightOutput = this.heightInput / this.pixelateFactor;
            this.bitmapOutput = new SKBitmap(this.widthOutput, this.heightOutput);

            // Settings
            if (this.HasCustomPalette)
            {
                this.paletteSize = this.customPalette.Size;
            }
        }
        private void StartPixelationProcess()
        {
            ApplyPixelation();
            ApplyColorReduction();
            ApplyCustomPalette();
            ApplyEffects();
            ApplyUpscale();
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
                    SKColor color = this.bitmapInput.GetAverageColorInArea(inputPosX, inputPosY);

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
            List<SKColor> colorsPool = [.. this.bitmapOutputColors];

            reducedColors.Add(GetMostCommonColor());

            while (reducedColors.Count < this.paletteSize && colorsPool.Count > 0)
            {
                SKColor farthestColor = colorsPool[0];
                double maxDistance = 0;

                foreach (SKColor candidate in colorsPool)
                {
                    double minDistanceToPalette = reducedColors.Min(existingColor =>
                        SPTColorMath.Difference(existingColor, candidate)
                    );

                    if (minDistanceToPalette > maxDistance)
                    {
                        maxDistance = minDistanceToPalette;
                        farthestColor = candidate;
                    }
                }

                reducedColors.Add(farthestColor);
                _ = colorsPool.Remove(farthestColor);
            }

            // Apply the new reduced palette
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
        private void ApplyEffects()
        {
            for (int i = 0; i < this.effects.Length; i++)
            {
                SPTEffect effect = this.effects[i];
                effect.ApplyEffect(this.bitmapOutput);
            }
        }
        private void ApplyUpscale()
        {
            SKImageInfo info = this.bitmapOutput.Info;

            int resizeWidth = (int)Math.Round(info.Width * this.upscaleFactor);
            int resizeHeight = (int)Math.Round(info.Height * this.upscaleFactor);

            this.bitmapOutput = this.bitmapOutput.Resize(info.WithSize(resizeWidth, resizeHeight), SKFilterQuality.None);
        }
        #endregion

        #region Tools
        private SKColor GetMostCommonColor()
        {
            Dictionary<SKColor, int> colorFrequency = [];

            for (int y = 0; y < this.heightOutput; y++)
            {
                for (int x = 0; x < this.widthOutput; x++)
                {
                    SKColor color = this.bitmapOutput.GetPixel(x, y);

                    if (colorFrequency.ContainsKey(color))
                    {
                        colorFrequency[color]++;
                    }
                    else
                    {
                        colorFrequency[color] = 1;
                    }
                }
            }

            return colorFrequency.OrderByDescending(c => c.Value).First().Key;
        }
        #endregion

        #region EXPORT
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

        #region Dispose
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
        #endregion
    }
}
