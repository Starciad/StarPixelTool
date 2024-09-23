using SkiaSharp;

using System.Collections.Generic;
using System.Linq;

namespace SPT.Core.Extensions
{
    internal static class SKBitmapExtensions
    {
        internal static SKColor GetAverageColorInArea(this SKBitmap bitmap, int x, int y)
        {
            SKColor[] colors = bitmap.GetNeighboringColors(x, y).Select(x => x.color).ToArray();

            int totalR = 0, totalG = 0, totalB = 0;
            int count = colors.Length;

            for (int i = 0; i < count; i++)
            {
                SKColor color = colors[i];

                totalR += color.Red;
                totalG += color.Green;
                totalB += color.Blue;
            }

            if (count > 0)
            {
                byte averageR = (byte)(totalR / count);
                byte averageG = (byte)(totalG / count);
                byte averageB = (byte)(totalB / count);

                return new SKColor(averageR, averageG, averageB, 255);
            }

            return SKColors.Empty;
        }

        internal static (SKColor color, int x, int y)[] GetNeighboringColors(this SKBitmap bitmap, int x, int y)
        {
            List<(SKColor, int, int)> colors = [];

            for (int i = -1; i < 2; i++)
            {
                for (int j = -1; j < 2; j++)
                {
                    int xPivot = x + i;
                    int yPivot = y + j;

                    if (bitmap.IsWithinBitmapBounds(xPivot, yPivot))
                    {
                        colors.Add((bitmap.GetPixel(xPivot, yPivot), xPivot, yPivot));
                    }
                }
            }

            return [.. colors];
        }

        private static bool IsWithinBitmapBounds(this SKBitmap bitmap, int x, int y)
        {
            return x >= 0 && x < bitmap.Width &&
                   y >= 0 && y < bitmap.Height;
        }
    }
}
