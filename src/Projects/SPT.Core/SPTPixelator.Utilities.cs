using SkiaSharp;

using SPT.Core.Colors;
using SPT.Core.Enums;

using System;
using System.Collections.Generic;
using System.Linq;

namespace SPT.Core
{
    public sealed partial class SPTPixelator
    {
        private SKColor GetMostCommonColor()
        {
            Dictionary<SKColor, int> colorFrequency = [];

            for (int y = 0; y < this.heightOutput; y++)
            {
                for (int x = 0; x < this.widthOutput; x++)
                {
                    SKColor color = this.bitmapOutput.GetPixel(x, y);

                    colorFrequency[color] = colorFrequency.TryGetValue(color, out int value) ? ++value : 1;
                }
            }

            return colorFrequency.OrderByDescending(c => c.Value).First().Key;
        }

        private bool IsSimilarColor(SKColor color1, SKColor color2)
        {
            return this.colorSpaceType switch
            {
                SPTColorSpaceType.RGB => SPTColorMath.DifferenceRGB(color1, color2) < this.colorTolerance,
                SPTColorSpaceType.HSL => SPTColorMath.DifferenceHSL(color1, color2) < this.colorTolerance,
                SPTColorSpaceType.HSV => SPTColorMath.DifferenceHSV(color1, color2) < this.colorTolerance,
                _ => throw new NotSupportedException("Unsupported color space."),
            };
        }
    }
}
