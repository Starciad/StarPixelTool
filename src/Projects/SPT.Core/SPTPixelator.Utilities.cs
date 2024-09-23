using SkiaSharp;

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
    }
}
