using SkiaSharp;

using SPT.Core.Colors;
using SPT.Core.Extensions;
using SPT.Core.Palettes;

using System;
using System.Collections.Generic;
using System.Linq;

namespace SPT.Core
{
    public sealed partial class SPTPixelator
    {
        private void StartPixelatorSystem()
        {
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

        private void StartPixelationProcess()
        {
            ApplyPixelation();
            ApplyColorReduction();
            ApplyCustomPalette();
            ApplyUpscale();
        }

        private void ApplyPixelation()
        {
            List<SKColor> colors = [];

            uint inputPosX = 0;
            uint inputPosY = 0;

            for (int y = 0; y < this.heightOutput; y++)
            {
                for (int x = 0; x < this.widthOutput; x++)
                {
                    SKColor color = this.bitmapInput.GetAverageColorInArea((int)inputPosX, (int)inputPosY);

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

        private void ApplyUpscale()
        {
            SKImageInfo info = this.bitmapOutput.Info;
            int resizeWidth = (int)Math.Round((double)info.Width * this.upscaleFactor);
            int resizeHeight = (int)Math.Round((double)info.Height * this.upscaleFactor);

            info = info.WithSize(resizeWidth, resizeHeight);

            this.bitmapOutput = this.bitmapOutput.Resize(info, SKFilterQuality.None);
        }
    }
}
